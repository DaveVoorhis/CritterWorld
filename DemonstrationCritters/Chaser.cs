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
    public class Chaser : DemoCritter, ICritterController
    {
        Point goal = new Point(-1, -1);
        Form settings = null;

        private void SetDestination(Point coordinate, int speed)
        {
            string message = "SET_DESTINATION:" + coordinate.X + ":" + coordinate.Y + ":" + speed;
            Send(message);
        }

        public Chaser(string name) : base(name)
        {
            Debugging = false;
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
                    Send("RANDOM_DESTINATION");
                    Send("SCAN:1");
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
                case "ERROR":
                    Console.WriteLine(message);
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
                    Log("I see nothing. Aim for the escape hatch.");
                    if (goal != new Point(-1, -1))
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
                            break;
                        case "EscapeHatch":
                            SetDestination(location, 5);
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
                        break;
                }
            }
        }

    }
}
