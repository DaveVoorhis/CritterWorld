using CritterController;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void Launch(BlockingCollection<string> messagesFromBody, BlockingCollection<string> messagesToBody)
        {
            
        }

        public void Shutdown()
        {
            
        }
    }
}
