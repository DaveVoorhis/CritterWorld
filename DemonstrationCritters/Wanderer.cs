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
    public class Wanderer : ICritterController
    {
        public string Name { get; set; }

        public Wanderer(string name)
        {
            Name = name;
        }

        public void Launch(ConcurrentQueue<string> messagesFromBody, ConcurrentQueue<string> messagesToBody)
        {
            Thread t = new Thread(() => 
            {
                while (true)
                {
                    while (messagesFromBody.TryDequeue(out string msg))
                    {
                        // Console.WriteLine("Message from body for " + Name + ": " + msg);
                        string[] msgParts = msg.Split(':');
                        switch (msgParts[0])
                        {
                            case "LAUNCHED":
                                messagesToBody.Enqueue("RANDOM_DESTINATION");
                                messagesToBody.Enqueue("DEBUG:1");
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
                        }
                    }
                    Thread.Sleep(5);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void Shutdown()
        {
            
        }
    }
}
