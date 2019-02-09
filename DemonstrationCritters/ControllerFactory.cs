using CritterController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonstrationCritters
{
    class ControllerFactory : ICritterControllerFactory
    {
        public string Author => "Dave Voorhis";

        public ICritterController[] GetCritterControllers()
        {
            return new ICritterController[] { new Wanderer("Wanderer1"), new Wanderer("Wanderer2"), new Wanderer("Wanderer3") };
        }
    }
}
