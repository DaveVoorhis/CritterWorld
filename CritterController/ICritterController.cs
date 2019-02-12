using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterController
{
    public delegate void Send(string message);

    public interface ICritterController
    {
        string Name { get; }
        Send Responder { get; set; }
        void Receive(string message);
        void LaunchUI();
    }
}
