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
        public event EventHandler<EventArgs> Finished; 

        private List<Level> levels = new List<Level>();
        private readonly Arena _arena;

        private int levelIndex = -1;
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

        public void NextLevel()
        {
            levelCheckTimer.Stop();
            currentLevel?.Shutdown();
            levelIndex++;
            if (levelIndex >= levels.Count)
            {
                Finished?.Invoke(this, new EventArgs());
                currentLevel = null;
            }
            else
            {
                currentLevel = levels[levelIndex];
                currentLevel.Launch();
                levelCheckTimer.Start();
            }
        }

        public void Launch()
        {
            levelIndex = -1;
            NextLevel();
            levelCheckTimer.Interval = 5000;
            levelCheckTimer.AutoReset = true;
            levelCheckTimer.Elapsed += (e, evt) =>
            {
                if (currentLevel.CountOfActiveCritters <= 1)
                {
                    NextLevel();
                }
            };
            levelCheckTimer.Start();
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
