using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterController
{
    public interface ICritterControllerFactory
    {
        string Author { get; }
        ICritterController[] GetCritterControllers();
    }
}
