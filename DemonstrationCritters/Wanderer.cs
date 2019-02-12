using CritterController;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemonstrationCritters
{
    public class Wanderer : ICritterController
    {
        private readonly bool Debugging = false;

        public string Name { get; set; }

        public Send Responder { get; set; }

        public Wanderer(string name)
        {
            Name = name;
        }

        public void LaunchUI()
        {
            // TODO - need to provide this.
        }

        public void Receive(string message)
        {
            if (Debugging)
            {
                Console.WriteLine("Message from body for " + Name + ": " + message);
            }
            string[] msgParts = message.Split(':');
            string notification = msgParts[0];
            switch (notification)
            {
                case "LAUNCH":
                    Responder.Invoke("RANDOM_DESTINATION");
                    if (Debugging)
                    {
                        Responder.Invoke("DEBUG:1");
                    }
                    break;
                case "REACHED_DESTINATION":
                case "FIGHT":
                case "BUMP":
                    Responder.Invoke("RANDOM_DESTINATION");
                    break;
                case "ERROR":
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
