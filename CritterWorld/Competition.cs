using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CritterWorld
{
    class Competition
    {
        private List<Level> levels = new List<Level>();
        private Arena _arena;

        private Level currentLevel;

        private Timer levelCheckTimer = new Timer();

        public Competition(Arena arena)
        {
            _arena = arena;
        }

        public void Add(Level level)
        {
            levels.Add(level);
            level.Arena = _arena;
        }

        public void Launch()
        {
            int levelIndex = 0;
            if (levelIndex > levels.Count)
            {
                return;
            }
            currentLevel = levels[levelIndex];

            levelCheckTimer.Interval = 5000;
            levelCheckTimer.AutoReset = true;
            levelCheckTimer.Elapsed += (e, evt) =>
            {
                if (currentLevel.CountOfActiveCritters <= 1)
                {
                    currentLevel.Shutdown();
                    levelIndex++;
                    if (levelIndex > levels.Count)
                    {
                        return;
                    }
                    currentLevel = levels[levelIndex];
                    currentLevel.Launch();
                }
            };
            levelCheckTimer.Start();
            currentLevel.Launch();
        }

        public void Shutdown()
        {
            if (currentLevel != null)
            {
                currentLevel.Shutdown();
            }
            levelCheckTimer.Stop();
        }
    }
}
