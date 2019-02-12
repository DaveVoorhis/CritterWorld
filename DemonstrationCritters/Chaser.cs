using CritterController;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
                                messagesToBody.Enqueue("SCAN");
                                break;
                            case "SCAN":
                                Scan(message, messagesToBody);
                                break;
                            case "REACHED_DESTINATION":
                                messagesToBody.Enqueue("RANDOM_DESTINATION");
                                break;
                            case "FIGHT":
                                messagesToBody.Enqueue("RANDOM_DESTINATION");
                                break;
                            case "BUMP":
                                messagesToBody.Enqueue("RANDOM_DESTINATION");
                                break;
                            case "SEE":
                                See(message, messagesToBody);
                                break;
                            case "SENSE":
                                Console.WriteLine(message);
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
                switch (thingAttributes[0])
                {
                    case "Food":
                        break;
                    case "Gift":
                        break;
                    case "Bomb":
                        break;
                    case "EscapeHatch":
                        break;
                    case "Terrain":
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
                switch (thingAttributes[0])
                {
                    case "Food":
                        break;
                    case "Gift":
                        break;
                    case "EscapeHatch":
                        break;
                    case "Terrain":
                        break;
                }
            }

        }

        public void Shutdown()
        {
            
        }
    }
}
