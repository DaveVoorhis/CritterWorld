using System;
using System.Collections.Generic;

namespace CritterWorld
{
    public class CritterLoader
    {
        public CritterLoader()
        {
        }

        public List<Critter> LoadCritters()
        {
            var critters = new List<Critter>();
            for (int i = 0; i < 50; i++)
            {
                Critter critter = new Critter(i + 1);
                critters.Add(critter);
            }
            return critters;
        }
    }
}