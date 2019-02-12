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
        public string Name { get; set; }

        public Chaser(string name)
        {
            Name = name;
        }

        private static Point PointFrom(string coordinate)
        {
            string[] coordinateParts = coordinate.Substring(1, coordinate.Length - 2).Split(',');
            string rawX = coordinateParts[0].Substring(2);
            string rawY = coordinateParts[1].Substring(2);
            int x = int.Parse(rawX);
            int y = int.Parse(rawY);
            return new Point(x, y);
        }

        public void Launch(ConcurrentQueue<string> messagesFromBody, ConcurrentQueue<string> messagesToBody)
        {
            Thread t = new Thread(() => 
            {
                while (true)
                {
                    while (messagesFromBody.TryDequeue(out string message))
                    {
                        // Console.WriteLine("Message from body for " + Name + ": " + message);
                        string[] msgParts = message.Split(':');
                        string notification = msgParts[0];
                        switch (notification)
                        {
                            case "LAUNCH":
                                messagesToBody.Enqueue("RANDOM_DESTINATION");
                                messagesToBody.Enqueue("SCAN");
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

        private void See(string message, ConcurrentQueue<string> messagesToBody)
        {
            string[] newlinePartition = message.Split('\n');
            string[] whatISee = newlinePartition[1].Split('\t');
            foreach (string thing in whatISee)
            {
                string[] thingAttributes = thing.Split(':');
                Point location = PointFrom(thingAttributes[1]);
                switch (thingAttributes[0])
                {
                    case "Food":
                        Console.WriteLine("Food is at " + location);
                        break;
                    case "Gift":
                        Console.WriteLine("Gift is at " + location);
                        break;
                    case "Bomb":
                        Console.WriteLine("Bomb is at " + location);
                        break;
                    case "EscapeHatch":
                        Console.WriteLine("EscapeHatch is at " + location);
                        break;
                    case "Terrain":
                        Console.WriteLine("Terrain is at " + location);
                        break;
                    case "Critter":
                        int critterNumber = int.Parse(thingAttributes[2]);
                        string nameAndAuthor = thingAttributes[3];
                        string strength = thingAttributes[4];
                        Console.WriteLine("Critter at " + location + " is #" + critterNumber + " who is " + nameAndAuthor + " with strength " + strength);
                        break;
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
                        Console.WriteLine("Escape hatch is at " + location);
                        break;
                }
            }

        }

        public void Shutdown()
        {
            
        }
    }
}
