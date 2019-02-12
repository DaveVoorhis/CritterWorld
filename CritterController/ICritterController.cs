using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterController
{
    public interface ICritterController
    {
        string Name { get; }
        void Receive(string message, ConcurrentQueue<string> sender);
        void LaunchUI();
    }
}
