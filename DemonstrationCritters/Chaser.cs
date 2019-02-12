using CritterController;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CritterController
{
    public class Chaser : ICritterController
    {
        private readonly bool Debugging = false;

        private static Point PointFrom(string coordinate)
        {
            string[] coordinateParts = coordinate.Substring(1, coordinate.Length - 2).Split(',');
            string rawX = coordinateParts[0].Substring(2);
            string rawY = coordinateParts[1].Substring(2);
            int x = int.Parse(rawX);
            int y = int.Parse(rawY);
            return new Point(x, y);
        }

        private Point goal = new Point(-1, -1);

        public string Name { get; set; }

        public Chaser(string name)
        {
            Name = name;
        }

        private void Log(string msg)
        {
            if (Debugging)
            {
                Console.WriteLine(msg);
            }
        }

        public void Launch(ConcurrentQueue<string> messagesFromBody, ConcurrentQueue<string> messagesToBody)
        {
            Thread t = new Thread(() => 
            {
                while (true)
                {
                    while (messagesFromBody.TryDequeue(out string message))
                    {
                        Log("Message from body for " + Name + ": " + message);
                        string[] msgParts = message.Split(':');
                        string notification = msgParts[0];
                        switch (notification)
                        {
                            case "LAUNCH":
                                messagesToBody.Enqueue("RANDOM_DESTINATION");
                                messagesToBody.Enqueue("SCAN:1");
                                break;
                            case "SCAN":
                                Scan(message, messagesToBody);
                                break;
                            case "REACHED_DESTINATION":
                            case "FIGHT":
                            case "BUMP":
                                messagesToBody.Enqueue("RANDOM_DESTINATION");
                                break;
                            case "SEE":
                                See(message, messagesToBody);
                                break;
                            case "ERROR":
                                Console.WriteLine(message);
                                break;
                        }
                    }
                    Thread.Sleep(5);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        private void SetDestination(Point coordinate, int speed, ConcurrentQueue<string> messagesToBody)
        {
            string message = "SET_DESTINATION:" + coordinate.X + ":" + coordinate.Y + ":" + speed;
            messagesToBody.Enqueue(message);
        }

        private void See(string message, ConcurrentQueue<string> messagesToBody)
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
                        SetDestination(goal, 5, messagesToBody);
                    }
                }
                else
                {
                    Point location = PointFrom(thingAttributes[1]);
                    switch (thingAttributes[0])
                    {
                        case "Food":
                            Log("Food is at " + location);
                            SetDestination(location, 5, messagesToBody);
                            break;
                        case "Gift":
                            Log("Gift is at " + location);
                            SetDestination(location, 5, messagesToBody);
                            break;
                        case "Bomb":
                            Log("Bomb is at " + location);
                            break;
                        case "EscapeHatch":
                            SetDestination(location, 5, messagesToBody);
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
                                SetDestination(location, 10, messagesToBody);
                            }
                            break;
                    }
                }
            }
        }

        private void Scan(string message, ConcurrentQueue<string> messagesToBody)
        {
            string[] newlinePartition = message.Split('\n');
            string[] whatISee = newlinePartition[1].Split('\t');
            foreach (string thing in whatISee)
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

        public void Shutdown()
        {
            
        }
    }
}
