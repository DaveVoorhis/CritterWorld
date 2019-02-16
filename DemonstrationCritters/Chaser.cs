using CritterController;
using System;
using System.Drawing;
using System.IO;

namespace DemonstrationCritters
{
    public class Chaser : ICritterController
    {
        Point goal = new Point(-1, -1);
        System.Timers.Timer getInfoTimer;
        bool headingForGoal = false;

        public string Name { get; set; }

        public Send Responder { get; set; }

        public Send Logger { get; set; }

        public string Filepath { get; set; }

        public int EatSpeed { get; set; } = 5;

        public int HeadForExitSpeed { get; set; } = 5;

        private static Point PointFrom(string coordinate)
        {
            string[] coordinateParts = coordinate.Substring(1, coordinate.Length - 2).Split(',');
            string rawX = coordinateParts[0].Substring(2);
            string rawY = coordinateParts[1].Substring(2);
            int x = int.Parse(rawX);
            int y = int.Parse(rawY);
            return new Point(x, y);
        }

        private void Log(string message)
        {
            if (Logger == null)
            {
                Console.WriteLine(message);
            }
            else
            {
                Logger.Invoke(message);
            }
        }

        private void Send(string message)
        {
            Responder.Invoke(message);
        }

        private void SetDestination(Point coordinate, int speed)
        {
            Send("SET_DESTINATION:" + coordinate.X + ":" + coordinate.Y + ":" + speed);
        }

        private void Tick()
        {
            Send("GET_LEVEL_TIME_REMAINING:1");
        }

        private void LoadSettings()
        {
            string fileName = "Chaser.cfg";
            string fileSpec = Filepath + "/" + fileName;
            try
            {
                using (StreamReader reader = new StreamReader(fileSpec))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] lineParts = line.Split('=');
                        switch (lineParts[0])
                        {
                            case "EatSpeed":
                                EatSpeed = int.Parse(lineParts[1]);
                                break;
                            case "HeadForExitSpeed":
                                HeadForExitSpeed = int.Parse(lineParts[1]);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log("Reading configuration " + fileSpec + " failed due to " + e);
            }
        }

        public void SaveSettings()
        {
            string fileName = "Chaser.cfg";
            string fileSpec = Filepath + "/" + fileName;
            try
            {
                using (StreamWriter writer = new StreamWriter(fileSpec, false)) {
                    writer.WriteLine("EatSpeed=" + EatSpeed);
                    writer.WriteLine("HeadForExitSpeed=" + HeadForExitSpeed);
                }
            }
            catch (Exception e)
            {
                Log("Writing configuration " + fileSpec + " failed due to " + e);
            }
        }

        public Chaser(string name)
        {
            Name = name;
        }

        public void LaunchUI()
        {
            ChaserSettings settings = new ChaserSettings(this);
            settings.Show();
            settings.Focus();
        }

        public void Receive(string message)
        {
            Log("Message from body for " + Name + ": " + message);
            string[] msgParts = message.Split(':');
            string notification = msgParts[0];
            switch (notification)
            {
                case "LAUNCH":
                    LoadSettings();
                    headingForGoal = false;
                    Send("STOP");
                    Send("SCAN:1");
                    getInfoTimer = new System.Timers.Timer();
                    getInfoTimer.Interval = 2000;
                    getInfoTimer.Elapsed += (obj, evt) => Tick();
                    getInfoTimer.Start();
                    break;
                case "SHUTDOWN":
                    getInfoTimer.Stop();
                    break;
                case "SCAN":
                    Scan(message);
                    break;
                case "REACHED_DESTINATION":
                case "FIGHT":
                case "BUMP":
                    Send("RANDOM_DESTINATION");
                    break;
                case "SEE":
                    See(message);
                    break;
                case "LEVEL_TIME_REMAINING":
                    int secondsRemaining = int.Parse(msgParts[2]);
                    if (secondsRemaining < 30)
                    {
                        Log("Now heading for goal.");
                        headingForGoal = true;
                        SetDestination(goal, HeadForExitSpeed);
                    }
                    break;
                case "ERROR":
                    Log(message);
                    break;
            }
        }

        private void See(string message)
        {
            string[] newlinePartition = message.Split('\n');
            string[] whatISee = newlinePartition[1].Split('\t');
            foreach (string thing in whatISee)
            {
                string[] thingAttributes = thing.Split(':');
                if (thingAttributes[0] == "Nothing")
                {
                    Log("I see nothing. Maybe aim for the escape hatch.");
                    if (headingForGoal && goal != new Point(-1, -1))
                    {
                        SetDestination(goal, HeadForExitSpeed);
                    }
                }
                else
                {
                    Point location = PointFrom(thingAttributes[1]);
                    switch (thingAttributes[0])
                    {
                        case "Food":
                            Log("Food is at " + location);
                            SetDestination(location, EatSpeed);
                            break;
                        case "Gift":
                            Log("Gift is at " + location);
                            SetDestination(location, EatSpeed);
                            break;
                        case "Bomb":
                            Log("Bomb is at " + location);
                            Send("RANDOM_DESTINATION");
                            break;
                        case "EscapeHatch":
                            if (headingForGoal)
                            {
                                SetDestination(location, HeadForExitSpeed);
                            }
                            Log("EscapeHatch is at " + location);
                            break;
                        case "Terrain":
                            Log("Terrain is at " + location);
                            break;
                        case "Critter":
                            int critterNumber = int.Parse(thingAttributes[2]);
                            string nameAndAuthor = thingAttributes[3];
                            string strength = thingAttributes[4];
                            bool isDead = thingAttributes[5] == "Dead";
                            Log("Critter at " + location + " is #" + critterNumber + " who is " + nameAndAuthor + " with strength " + strength + " is " + (isDead ? "dead" : "alive"));
                            if (strength == "Weak" && !isDead)
                            {
                                SetDestination(location, 10);
                            }
                            break;
                    }
                }
            }
        }

        private void Scan(string message)
        {
            string[] newlinePartition = message.Split('\n');
            string[] whatISense = newlinePartition[1].Split('\t');
            foreach (string thing in whatISense)
            {
                string[] thingAttributes = thing.Split(':');
                Point location = PointFrom(thingAttributes[1]);
                switch (thingAttributes[0])
                {
                    case "EscapeHatch":
                        Log("Escape hatch is at " + location);
                        goal = location;
                        SetDestination(location, 5);
                        break;
                }
            }
        }

    }
}
