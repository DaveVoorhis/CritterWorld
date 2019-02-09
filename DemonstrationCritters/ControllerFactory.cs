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
            List<ICritterController> controllers = new List<ICritterController>();
            for (int i = 0; i < 50; i++)
            {
                controllers.Add(new Wanderer("Wanderer" + (i + 1)));
            }
            return controllers.ToArray();
        }
    }
}
