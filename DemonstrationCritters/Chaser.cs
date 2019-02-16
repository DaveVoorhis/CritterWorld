using CritterController;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemonstrationCritters
{
    public class Chaser : ICritterController
    {
        Point goal = new Point(-1, -1);
        Form settings = null;
        System.Timers.Timer getInfoTimer;
        bool headingForGoal = false;

        public string Name { get; set; }

        public Send Responder { get; set; }

        public Send Logger { get; set; }

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
            Logger.Invoke(message);
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

        public Chaser(string name)
        {
            Name = name;
        }

        public void LaunchUI()
        {
            if (settings == null)
            {
                settings = new ChaserSettings();
            }
            settings.Visible = !settings.Visible;
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
                        SetDestination(goal, 5);
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
                        SetDestination(goal, 5);
                    }
                }
                else
                {
                    Point location = PointFrom(thingAttributes[1]);
                    switch (thingAttributes[0])
                    {
                        case "Food":
                            Log("Food is at " + location);
                            SetDestination(location, 5);
                            break;
                        case "Gift":
                            Log("Gift is at " + location);
                            SetDestination(location, 5);
                            break;
                        case "Bomb":
                            Log("Bomb is at " + location);
                            Send("RANDOM_DESTINATION");
                            break;
                        case "EscapeHatch":
                            if (headingForGoal)
                            {
                                SetDestination(location, 5);
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
