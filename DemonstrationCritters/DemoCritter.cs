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
    public class DemoCritter
    {
        protected bool Debugging = false;

        protected static Point PointFrom(string coordinate)
        {
            string[] coordinateParts = coordinate.Substring(1, coordinate.Length - 2).Split(',');
            string rawX = coordinateParts[0].Substring(2);
            string rawY = coordinateParts[1].Substring(2);
            int x = int.Parse(rawX);
            int y = int.Parse(rawY);
            return new Point(x, y);
        }

        protected void Log(string msg)
        {
            if (Debugging)
            {
                Console.WriteLine(Name + ":" + msg);
            }
        }

        protected void Send(string message)
        {
            Responder.Invoke(message);
        }

        public string Name { get; set; }

        public Send Responder { get; set; } 

        public DemoCritter(string name)
        {
            Name = name;
        }

    }
}
