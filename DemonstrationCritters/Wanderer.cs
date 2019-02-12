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
    public class Wanderer : DemoCritter, ICritterController
    {
        public Wanderer(string name) : base(name)
        {
            Debugging = false;
        }

        public void LaunchUI()
        {
            // TODO - need to provide this.
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
                    if (Debugging)
                    {
                        Send("DEBUG:1");
                    }
                    break;
                case "REACHED_DESTINATION":
                case "FIGHT":
                case "BUMP":
                    Send("RANDOM_DESTINATION");
                    break;
                case "ERROR":
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
