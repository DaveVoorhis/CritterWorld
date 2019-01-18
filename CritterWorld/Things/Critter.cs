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

namespace CritterWorld
{
    public class Critter : PolygonSprite
    {
        public const int maxThinkTimeMilliseconds = 1000;
        public const int maxThinkTimeOverrunViolations = 5;
        public const float movementEnergyConsumptionFactor = 250;  // the higher this is, the less movement consumes energy
        public const float eatingAddsEnergy = 50.0F;       // each piece of food adds this much energy; maximum 100
        public const float eatingAddsHealth = 10.0F;
        public const float fightingDeductsHealth = 0.5F;
        public const float bumpingTerrainDeductsHealth = 0.25F;

        public int Number { get; private set; }

        public string Name { get; set; } = GetRandomName();
        public string Author { get; set; } = "Critterworld";

        public string NameAndAuthor { get { return Name + " by " + Author; } }

        public int EscapedCount { get; private set; }
        public int BombedCount { get; private set; }
        public int CrashedCount { get; private set; }
        public int TerminatedCount { get; private set; }
        public int StarvedCount { get; private set; }
        public int FatallyInjuredCount { get; private set; }

        public int OverallScore { get; private set; }
        public int CurrentScore { get; private set; }

        public float Energy { get; private set; }
        public float Health { get; private set; }

        public bool IsEscaped { get; private set; }

        public string DeadReason { get; private set; } = null;
        public bool IsDead { get { return DeadReason != null; } }

        private int thinkTimeOverrunViolations = 0;
        private int moveCount = 0;

        private Thread thinkThread = null;
        private bool stopped = true;

        private readonly bool selectedToTestCrash = false;

        private static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public static string GetRandomName()
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

        public Critter(int critterNumber) : base(new CritterBody().GetBody(1))
        {
            Number = critterNumber;

            LineWidth = 1;
            Color = Sprite.RandomColor(127);
            FacingAngle = 90;

            selectedToTestCrash = (rnd.Next(10) == 5);

            Processors += sprite =>
            {
                TargetMover spriteMover = (TargetMover)Mover;
                if (spriteMover == null || (spriteMover.SpeedX == 0 && spriteMover.SpeedY == 0))
                {
                    return;
                }
                spriteMover.TargetFacingAngle = (int)Sprite.GetAngle(spriteMover.SpeedX, spriteMover.SpeedY) + 90;
            };
        }

        private static string ToQuoted(string input)
        {
            return "\"" + input.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t") + "\"";
        }

        private string lastMessage = "";

        public void Log(string message, Exception exception = null)
        {
            string msg = ToQuoted(Name) + ", " + ToQuoted(Author) + ", " + ToQuoted(message) + ", " + ((exception == null) ? ToQuoted("") : ToQuoted(exception.StackTrace));
            if (!msg.Equals(lastMessage))
            {
                lastMessage = msg;
                Critterworld.Log(msg);
            }
        }

        public void Escaped()
        {
            Log("escaped");
            EscapedCount++;
            OverallScore += CurrentScore;
            Kill();
            IsEscaped = true;
        }

        public void Scored()
        {
            Log("scored");
            CurrentScore++;
        }

        public void Ate()
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
        }

        public void ConsumeEnergy(float consumption)
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

        public void FightWith(string opponent)
        {
            Log("fought with " + opponent);
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

        public void Bump()
        {
            Log("bumped into terrain");
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

        public void FatallyInjured()
        {
            FatallyInjuredCount++;
            DeadReason = "fatally injured";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        public void Starved()
        {
            StarvedCount++;
            DeadReason = "starved";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        public void Bombed()
        {
            BombedCount++;
            DeadReason = "bombed";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        public void Crashed()
        {
            CrashedCount++;
            DeadReason = "crashed";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        public void Terminated()
        {
            TerminatedCount++;
            DeadReason = "terminated for spending too long thinking";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        protected internal void Think(Random random)
        {
            // Do things here.
            int rand = random.Next(0, 2500);
            if (rand == 25)
            {
                Sound.PlayArc();
                Sprite shockwave = new ShockWaveSprite(5, 20, 10, Color.DarkBlue, Color.LightBlue);
                shockwave.Position = Position;
                shockwave.Mover = new SlaveMover(this);
                Engine.AddSprite(shockwave);
            }
            else if (rand == 26 && selectedToTestCrash)
            {
                throw new FormatException("test exception");
            }
        }

        public void ClearDestination()
        {
            AssignDestination((int)X, (int)Y);
        }

        public void AssignDestination(int destX, int destY)
        {
            TargetMover mover = (TargetMover)Mover;
            if (mover == null)
            {
                return;
            }
            mover.Speed = rnd.Next(10) + 1;
            mover.Target = new Point(destX, destY);
            mover.StopAtTarget = true;
        }

        public void AssignRandomDestination()
        {
            int destX = rnd.Next(Surface.Width);
            int destY = rnd.Next(Surface.Height);
            AssignDestination(destX, destY);
        }

        // Bounce back to position before most recent move. 
        // Invoke after a collision to prevent "embedding" or slowly 
        // creeping through obstacles when a collision is detected.
        public void Bounceback()
        {
            ((TargetMover)Mover)?.Bounceback();
        }

        public long TotalThinkTime { get; private set; } = 0;

        public long ThinkCount { get; private set; } = 0;

        public double AverageThinkTime
        {
            get
            {
                if (ThinkCount == 0)
                {
                    return double.NaN;
                }
                else
                {
                    return (double)TotalThinkTime / (double)ThinkCount;
                }
            }
        }

        // Something has crashed, burned out or blown up. Stop thinking, moving, or doing anything except
        // emit smoke for a while.
        public void StopAndSmoke(Color startColor, Color endColor)
        {
            Mover = null;
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
        public TextSprite CreateNumberPlate()
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

        private TextSprite numberPlate = null;

        // Attach a number plate to this Critter.
        public void AttachNumberPlate()
        {
            if (numberPlate != null)
            {
                return;
            }
            numberPlate = CreateNumberPlate();
            numberPlate.Mover = new SlaveMover(this);
            Engine.AddSprite(numberPlate);
        }

        private int numberPlateIncrement = 1;

        // Launch this Critter.
        public void Startup()
        {
            Health = 100;
            Energy = 100;
            IsEscaped = false;
            DeadReason = null;

            if (thinkThread != null)
            {
                return;
            }

            TargetMover spriteMover = new TargetMover();
            spriteMover.SpriteReachedTarget += (sender, spriteEvent) => AssignRandomDestination();
            spriteMover.SpriteMoved += (sender, spriteEvent) =>
            {
                ConsumeEnergy(spriteEvent.Distance * spriteEvent.Speed / movementEnergyConsumptionFactor);
                if (moveCount-- == 0)
                {
                    IncrementFrame();
                    moveCount = 5 - Math.Min(5, (int)spriteMover.Speed);
                }
            };
            Mover = spriteMover;

            AttachNumberPlate();

            thinkThread = new Thread(() =>
            {
                stopped = false;
                Log("launched");
                Stopwatch stopwatch = new Stopwatch();
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                while (Surface != null && !Surface.IsDisposed && !Surface.Disposing && !Dead && !stopped)
                {
                    if (Surface.Active)
                    {
                        try
                        {
                            if (numberPlate != null)
                            {
                                numberPlate.Position = Position;
                                numberPlate.Alpha += (byte)numberPlateIncrement;
                                if (numberPlate.Alpha == 255)
                                {
                                    numberPlateIncrement = -1;
                                }
                                else if (numberPlate.Alpha == 0)
                                {
                                    numberPlateIncrement = 1;
                                }
                            }

                            stopwatch.Reset();
                            stopwatch.Start();
                            Think(rnd);
                            stopwatch.Stop();
                            long elapsed = stopwatch.ElapsedMilliseconds;
                            if (elapsed > 1000)
                            {
                                if (thinkTimeOverrunViolations >= maxThinkTimeOverrunViolations)
                                {
                                    Log("Was warned " + thinkTimeOverrunViolations + " times about thinking for too long. Now you may not think again.");
                                    Terminated();
                                    Sound.PlayCrash();
                                    StopAndSmoke(Color.DarkGreen, Color.LightGreen);
                                    break;
                                }
                                else
                                {
                                    Log("Warning #" + (++thinkTimeOverrunViolations) + " you have exceeded the maximum think time of " + maxThinkTimeMilliseconds + " by " + (elapsed - maxThinkTimeMilliseconds) + " milliseconds.");
                                }
                            }
                            TotalThinkTime += elapsed;
                            ThinkCount++;
                        }
                        catch (Exception e)
                        {
                            Crashed();
                            Sound.PlayCrash();
                            Log("Crashed due to exception whilst thinking: " + e, e);
                            StopAndSmoke(Color.Aquamarine, Color.Blue);
                            break;
                        }
                    }
                    Thread.Sleep(5);
                }
                thinkThread = null;
            });
            thinkThread.Name = Name;
            thinkThread.Start();

            Surface.Disposed += (e, evt) => thinkThread?.Abort();
            Died += (e, evt) => thinkThread?.Abort();

            AssignRandomDestination();
        }

        // Shut down this Critter.
        public void Shutdown()
        {
            if (numberPlate != null)
            {
                numberPlate.Color = Color.LightGray;
                numberPlate.FillColor = Color.LightGray;
                numberPlate.Alpha = 255;
            }
            Mover = null;
            stopped = true;
        }

        // True if this critter is stopped or dead
        public bool Stopped
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
            }
            base.Kill();
        }
    }
}
