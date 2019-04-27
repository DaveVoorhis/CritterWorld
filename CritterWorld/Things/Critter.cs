using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SCG.TurboSprite;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using CritterController;

namespace CritterWorld
{
    public class Critter : PolygonSprite, IVisible
    {
        public const float sightDistance = 100.0F;     // how far can critter see?
        public const float movementEnergyConsumptionFactor = 250;  // the higher this is, the less movement consumes energy
        public const float eatingAddsEnergy = 50.0F;       // each piece of food adds this much energy; maximum 100
        public const float eatingAddsHealth = 10.0F;
        public const float fightingDeductsHealth = 0.5F;
        public const float bumpingTerrainDeductsHealth = 0.25F;

        public int Number { get; private set; }

        public string Name { get; set; } = GetRandomName();
        public string Author { get; set; } = "Critterworld";

        public string NameAndAuthor { get { return Name + " by " + Author; } }

        public string NumberNameAndAuthor { get { return Number + ":" + NameAndAuthor; } }

        public int EscapedCount { get; private set; }
        public int BombedCount { get; private set; }
        public int CrashedCount { get; private set; }
        public int StarvedCount { get; private set; }
        public int FatallyInjuredCount { get; private set; }

        public int OverallScore { get; private set; }
        public int CurrentScore { get; private set; }

        public float Energy { get; private set; }
        public float Health { get; private set; }

        public bool IsEscaped { get; private set; }

        public string DeadReason { get; private set; } = null;
        public bool IsDead { get { return DeadReason != null; } }

        public bool Debugging { get; set; } = false;

        public ConcurrentQueue<string> MessagesFromBody { get; } = new ConcurrentQueue<string>();
        public ConcurrentQueue<string> MessagesToBody { get; } = new ConcurrentQueue<string>();

        private int moveCount = 0;

        private bool stopped = true;

        private static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        private TextSprite numberPlate = null;
        private int numberPlateIncrement = 1;

        private ICritterController controller = null;
        Thread controllerThread = null;
        bool controllerThreadRunning = true;

        private static void CritterProcessor(Sprite sprite)
        {
            if (sprite is Critter critter)
            {
                if (critter.numberPlate != null)
                {
                    if (critter.stopped)
                    {
                        critter.numberPlate.Color = Color.LightGray;
                        critter.numberPlate.FillColor = Color.LightGray;
                        critter.numberPlate.Alpha = 255;
                    }
                    else
                    {
                        critter.numberPlate.Position = critter.Position;
                        critter.numberPlate.Alpha += (byte)critter.numberPlateIncrement;
                        if (critter.numberPlate.Alpha == 255)
                        {
                            critter.numberPlateIncrement = -1;
                        }
                        else if (critter.numberPlate.Alpha == 0)
                        {
                            critter.numberPlateIncrement = 1;
                        }
                    }
                }

                if (critter.IsDead)
                {
                    return;
                }

                if (sprite.Mover is TargetMover mover && (mover.SpeedX != 0 || mover.SpeedY != 0))
                {
                    mover.TargetFacingAngle = (int)GetAngle(mover.SpeedX, mover.SpeedY) + 90;
                }

                critter.See();

                while (critter.MessagesToBody.TryDequeue(out string message))
                {
                    critter.LogDebugging("Message from controller: " + message);
                    try
                    {
                        string[] commandParts = message.Split(':');
                        switch (commandParts[0])
                        {
                            case "SCAN":
                                critter.Scan(int.Parse(commandParts[1]));
                                break;
                            case "GET_LEVEL_DURATION":
                                critter.GetLevelDuration(int.Parse(commandParts[1]));
                                break;
                            case "GET_LEVEL_TIME_REMAINING":
                                critter.GetLevelTimeRemaining(int.Parse(commandParts[1]));
                                break;
                            case "GET_HEALTH":
                                critter.GetHealth(int.Parse(commandParts[1]));
                                break;
                            case "GET_ENERGY":
                                critter.GetEnergy(int.Parse(commandParts[1]));
                                break;
                            case "GET_LOCATION":
                                critter.GetLocation(int.Parse(commandParts[1]));
                                break;
                            case "GET_SPEED":
                                critter.GetSpeed(int.Parse(commandParts[1]));
                                break;
                            case "GET_ARENA_SIZE":
                                critter.GetArenaSize(int.Parse(commandParts[1]));
                                break;
                            case "SET_DESTINATION":
                                critter.SetDestination(int.Parse(commandParts[1]), int.Parse(commandParts[2]), int.Parse(commandParts[3]));
                                break;
                            case "RANDOM_DESTINATION":
                                critter.AssignRandomDestination();
                                break;
                            case "STOP":
                                critter.ClearDestination();
                                break;
                            case "SET_SPEED":
                                critter.SetSpeed(int.Parse(commandParts[1]));
                                break;
                            default:
                                critter.Notify("ERROR: Unknown command:" + commandParts[0]);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        critter.Notify("ERROR: Possible invalid command syntax in " + message + " caused " + e);
                    }
                }
            }
        }

        public void LaunchSettingsUI()
        {
            try
            {
                controller.LaunchUI();
            }
            catch (Exception e)
            {
                Notify("ERROR: Unable to launch UI: " + e);
            }
        }

        private void See()
        {
            string sensorReading = string.Join("\t", Engine.SpriteArray
                .OfType<IVisible>()
                .Cast<Sprite>()
                .Where(sprite => sprite != this && GetDistance(sprite, this) <= sightDistance)
                .Cast<ISignature>()
                .Select(thing => thing.SensorSignature));
            if (sensorReading.Length == 0)
            {
                sensorReading = "Nothing";
            }
            Notify("SEE:\n" + sensorReading);
        }

        private void Scan(int requestNumber)
        {
            string sensorReading = string.Join("\t", Engine.SpriteArray
                .OfType<ISensable>()
                .Cast<ISignature>()
                .Select(thing => thing.SensorSignature));
            Notify("SCAN:" + requestNumber + "\n" + sensorReading);
        }

        private void GetLevelDuration(int requestNumber)
        {
            Notify("LEVEL_DURATION:" + requestNumber + ":" + Critterworld.LevelDurationInSeconds);
        }

        private void GetLevelTimeRemaining(int requestNumber)
        {
            Notify("LEVEL_TIME_REMAINING:" + requestNumber + ":" + Critterworld.LevelTimeRemaining);
        }

        private void GetHealth(int requestNumber)
        {
            Notify("HEALTH:" + requestNumber + ":" + Health + ":" + HealthStatus);
        }

        private void GetEnergy(int requestNumber)
        {
            Notify("ENERGY:" + requestNumber + ":" + Energy);
        }

        private void GetLocation(int requestNumber)
        {
            Notify("LOCATION:" + requestNumber + ":" + Position);
        }

        private void GetSpeed(int requestNumber)
        {
            if (Mover is TargetMover theMover)
            {
                Notify("SPEED:" + requestNumber + ":" + theMover.Speed + ":" + theMover.SpeedX + ":" + theMover.SpeedY);
            }
            else
            {
                Notify("SPEED:" + requestNumber + ":" + 0 + ":" + 0 + ":" + 0);
            }
        }

        private void GetArenaSize(int requestNumber)
        {
            Notify("ARENA_SIZE:" + requestNumber + ":" + Critterworld.ArenaWidth + ":" + Critterworld.ArenaHeight);
        }

        private void SetDestination(int destX, int destY, int speed)
        {
            if (destX < 0 || destX > Critterworld.ArenaWidth - 1 || destY < 0 || destY > Critterworld.ArenaHeight - 1 || speed < 1 || speed > 10)
            {
                Notify("Error: SET_DESTINATION: Invalid value in destination coordinate " + destX + ", " + destY + " at speed " + speed);
            }
            else
            {
                AssignDestination(destX, destY, speed);
            }
        }

        private void SetSpeed(int speed)
        {
            if (Mover is TargetMover theMover)
            {
                if (speed < 1 || speed > 10)
                {
                    Notify("Error: SET_SPEED: Invalid speed " + speed);
                }
                else
                {
                    theMover.Speed = speed;
                }
            }
        }

        private string HealthStatus
        {
            get
            {
                if (Health > 75)
                {
                    return "Strong";
                }
                else if (Health > 50)
                {
                    return "Ok";
                }
                else if (Health > 25)
                {
                    return "Adequate";
                }
                else
                {
                    return "Weak";
                }
            }
        }

        public string SensorSignature
        {
            get { return "Critter" + ":" + Position + ":" + NumberNameAndAuthor + ":" + HealthStatus + ":" + (IsDead ? "Dead" : "Alive"); }
        }

        internal static string GetRandomName()
        {
            string[] consonants = { "b", "c", "d", "f", "ff", "g", "gh", "h", "j", "l", "m", "l", "n", "p", "ph", "q", "r", "s", "th", "tt", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "y", "ee", "ea", "io", "oi", "ae" };
            string name = "";
            int len = rnd.Next(2, 5);
            while (len-- > 0)
            {
                name += consonants[rnd.Next(consonants.Length)];
                name += vowels[rnd.Next(vowels.Length)];
            }
            return name[0].ToString().ToUpper() + name.Substring(1);
        }

        internal Critter(int critterNumber, ICritterController critterController) : base(new CritterBody().GetBody(1))
        {
            Number = critterNumber;
            controller = critterController;

            LineWidth = 1;
            Color = Sprite.RandomColor(127);

            Reset();

            Processors += CritterProcessor;
        }

        // Send message to log unconditionally
        private void Log(String message, Exception exception = null)
        {
            LogEntry newLogEntry = new LogEntry(Number, Name, Author, message, exception);
            Critterworld.Log(newLogEntry);
        }

        // Send message to log if debugging is true
        private void LogDebugging(string message)
        {
            if (Debugging)
            {
                Log(message);
            }
        }

        // Send message to controller
        private void Notify(string message)
        {
            LogDebugging("Message to controller: " + message);
            MessagesFromBody.Enqueue(message);
        }

        private void AssignSpeed(int speed)
        {
            if (Mover is TargetMover mover)
            {
                mover.Speed = speed;
            }
        }

        private void AssignDestination(int destX, int destY, int speed)
        {
            if (Mover is TargetMover mover)
            {
                mover.Speed = speed;
                mover.Target = new Point(destX, destY);
                mover.StopAtTarget = true;
            }
        }

        private void ClearDestination()
        {
            AssignDestination((int)X, (int)Y, 0);
        }

        private void AssignRandomDestination()
        {
            int destX = rnd.Next(Surface.Width);
            int destY = rnd.Next(Surface.Height);
            AssignDestination(destX, destY, rnd.Next(10) + 1);
        }

        internal void Reset()
        {
            numberPlate = null;
            FacingAngle = 90;
            CurrentScore = 0;
            Health = 100;
            Energy = 100;
            IsEscaped = false;
            DeadReason = null;
            Dead = false;
            moveCount = 0;
            numberPlateIncrement = 1;
        }

        internal void Escaped()
        {
            Log("escaped");
            Notify("ESCAPE:" + Position.ToString());
            EscapedCount++;
            OverallScore += CurrentScore;
            Kill();
            IsEscaped = true;
        }

        internal void Scored()
        {
            Log("scored");
            CurrentScore++;
            Notify("SCORED:" + Position.ToString() + ":" + CurrentScore);
        }

        internal void Ate()
        {
            Log("ate");
            if (Energy + eatingAddsEnergy > 100)
            {
                Energy = 100;
            }
            else
            {
                Energy += eatingAddsEnergy;
            }
            if (Health + eatingAddsHealth > 100)
            {
                Health = 100;
            }
            else
            {
                Health += eatingAddsHealth;
            }
            Notify("ATE:" + Position.ToString() + ":" + Energy + ":" + Health);
        }

        internal void ConsumeEnergy(float consumption)
        {
            if (Energy - consumption <= 0)
            {
                Energy = 0;
                Starved();
                Shutdown();
            }
            else
            {
                Energy -= consumption;
            }
        }

        internal void FightWith(string opponent)
        {
            Notify("FIGHT:" + Position.ToString() + ":" + opponent);
            if (Health - fightingDeductsHealth <= 0)
            {
                Health = 0;
                FatallyInjured();
                Shutdown();
            }
            else
            {
                Health -= fightingDeductsHealth;
            }
        }

        internal void Bump()
        {
            Notify("BUMP:" + Position.ToString());
            if (Health - bumpingTerrainDeductsHealth <= 0)
            {
                Health = 0;
                FatallyInjured();
                Shutdown();
            }
            else
            {
                Health -= bumpingTerrainDeductsHealth;
            }
        }

        internal void FatallyInjured()
        {
            Notify("FATALITY:" + Position.ToString());
            FatallyInjuredCount++;
            DeadReason = "fatally injured";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        internal void Starved()
        {
            Notify("STARVED:" + Position.ToString());
            StarvedCount++;
            DeadReason = "starved";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        internal void Bombed()
        {
            Notify("BOMBED:" + Position.ToString());
            BombedCount++;
            DeadReason = "bombed";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        internal void Crashed(Exception e)
        {
            Sound.PlayCrash();
            StopAndSmoke(Color.DarkBlue, Color.LightBlue);
            Notify("CRASHED:" + Position.ToString() + ":" + e.ToString());
            CrashedCount++;
            DeadReason = "crashed";
            Log(DeadReason, e);
            Health = 0;
            Energy = 0;
        }

        internal void ShowShockwave()
        {
            Sound.PlayArc();
            Sprite shockwave = new ShockWaveSprite(5, 20, 50, Color.DarkBlue, Color.LightBlue);
            shockwave.Position = Position;
            shockwave.Mover = new SlaveMover(this);
            Engine?.AddSprite(shockwave);
        }

        // Bounce back to position before most recent move. 
        // Invoke after a collision to prevent "embedding" or slowly 
        // creeping through obstacles when a collision is detected.
        internal void Bounceback()
        {
            if (Mover is TargetMover mover)
            {
                mover.Bounceback();
            }
        }

        // Something has crashed, burned out or blown up. Stop thinking, moving, or doing anything except
        // emit smoke for a while.
        internal void StopAndSmoke(Color startColor, Color endColor)
        {
            Mover = new NullMover();
            Shutdown();
            ParticleFountainSprite smoke = new ParticleFountainSprite(20, startColor, endColor, 1, 10, 10)
            {
                Position = Position
            };
            Engine?.AddSprite(smoke);
            System.Timers.Timer smokeTimer = new System.Timers.Timer
            {
                Interval = 1000,
                AutoReset = true
            };
            smokeTimer.Elapsed += (sender2, e2) =>
            {
                if (smoke.EndDiameter >= 2)
                {
                    smoke.EndDiameter -= 1;
                    smoke.Radius -= 1;
                }
                else
                {
                    smoke.Kill();
                    smokeTimer.Stop();
                }
            };
            smokeTimer.Start();
        }

        // Create a number plate for this Critter at a given position
        private TextSprite CreateNumberPlate()
        {
            return new TextSprite(Number.ToString(), "Arial", 14, FontStyle.Regular)
            {
                Position = Position,
                IsFilled = true,
                Color = Color.White,
                FillColor = Color.White,
                Alpha = 200
            };
        }

        // Attach a number plate to this Critter.
        private void AttachNumberPlate()
        {
            if (numberPlate != null)
            {
                return;
            }
            numberPlate = CreateNumberPlate();
            numberPlate.Mover = new SlaveMover(this);
            Engine?.AddSprite(numberPlate);
        }

        private static void MoveHandler(object sender, SpriteMoveEventArgs mover)
        {
            if (mover.Sprite is Critter critter)
            {
                critter.ConsumeEnergy(mover.Distance * mover.Speed / movementEnergyConsumptionFactor);
                if (critter.moveCount-- == 0)
                {
                    critter.IncrementFrame();
                    critter.moveCount = 5 - Math.Min(5, (int)mover.Speed);
                }
            }
        }

        // Launch this Critter.
        internal void Launch()
        {
            controllerThread = new Thread(() =>
            {
                Reset();

                AttachNumberPlate();

                TargetMover spriteMover = new TargetMover();
                spriteMover.SpriteReachedTarget += (sender, spriteEvent) => Notify("REACHED_DESTINATION:" + spriteEvent.Sprite.Position.ToString());
                spriteMover.SpriteMoved += MoveHandler;
                Mover = spriteMover;

                stopped = false;

                Log("launched");

                Notify("LAUNCH:" + Position.ToString());

                controllerThreadRunning = true;
                while (controllerThreadRunning)
                {
                    controller.Responder = messageToBody => MessagesToBody.Enqueue(messageToBody);
                    controller.Logger = logMessage => LogDebugging(logMessage);
                    while (MessagesFromBody.TryDequeue(out string messageFromBody))
                    {
                        try
                        {
                            controller.Receive(messageFromBody);
                        }
                        catch (Exception e)
                        {
                            Crashed(e);
                        }
                    }
                    Thread.Sleep(5);
                }
                // Clear message queues
                string ignore;
                while (MessagesFromBody.TryDequeue(out ignore)) ;
                while (MessagesToBody.TryDequeue(out ignore)) ;
            });
            controllerThread.IsBackground = true;
            controllerThread.Start();
        }

        // Shut down this Critter.
        internal void Shutdown()
        {
            if (stopped)
            {
                return;
            }

            Mover = new NullMover();
            stopped = true;

            Log("shutdown");

            Notify("SHUTDOWN:" + Position.ToString());

            controllerThreadRunning = false;
        }

        // True if this critter is stopped or dead
        internal bool Stopped
        {
            get
            {
                return Dead || stopped;
            }
        }

        public override void Kill()
        {
            Shutdown();
            if (numberPlate != null)
            {
                numberPlate.Kill();
                numberPlate = null;
            }
            base.Kill();
        }

        public override string ToString()
        {
            return NumberNameAndAuthor;
        }
    }
}
