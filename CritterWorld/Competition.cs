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
        public event EventHandler<EventArgs> FinishedLevel;

        private List<Level> levels = new List<Level>();
        private readonly Arena _arena;
        private int levelIndex = -1;
        private Level currentLevel;
        private readonly Action _LoadCritters;

        private Timer levelCheckTimer = new Timer();

        public Competition(Arena arena, Action LoadCritters)
        {
            _arena = arena;
            _LoadCritters = LoadCritters;
        }

        public void Add(Level level)
        {
            levels.Add(level);
            level.Arena = _arena;
        }

        public void NextLevel()
        {
            levelCheckTimer.Stop();
            _arena.Shutdown();
            levelIndex++;
            if (levelIndex >= levels.Count)
            {
                Finished?.Invoke(this, new EventArgs());
                currentLevel = null;
            }
            else
            {
                currentLevel = levels[levelIndex];
                currentLevel.Setup();
                _LoadCritters();
                _arena.Launch();
                levelCheckTimer.Start();
                FinishedLevel?.Invoke(this, new EventArgs());
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
                if (currentLevel.CountOfActiveCritters == 0)
                {
                    NextLevel();
                }
            };
            levelCheckTimer.Start();
        }

        public void Shutdown()
        {
            _arena.Shutdown();
            levelCheckTimer.Stop();
        }
    }
}
