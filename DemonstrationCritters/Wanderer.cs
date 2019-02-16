using CritterController;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemonstrationCritters
{
    public class Wanderer : ICritterController
    {
        public string Name { get; set; }

        public Send Responder { get; set; }

        public Send Logger { get; set; }

        public string Filepath { get; set; }

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
            Log("Message from body for " + Name + ": " + message);
            string[] msgParts = message.Split(':');
            string notification = msgParts[0];
            switch (notification)
            {
                case "LAUNCH":
                case "REACHED_DESTINATION":
                case "FIGHT":
                case "BUMP":
                    Send("RANDOM_DESTINATION");
                    break;
                case "ERROR":
                    Log(message);
                    break;
            }
        }
    }
}
