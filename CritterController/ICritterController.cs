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
        void Launch(ConcurrentQueue<String> messagesFromBody, ConcurrentQueue<String> messagesToBody);
        void Shutdown();
    }
}
