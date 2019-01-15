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
        private readonly IScoreDisplay[] _critterScorePanels;
        private int levelIndex = -1;
        private Level currentLevel;

        private Timer levelCheckTimer = new Timer();

        public Competition(Arena arena, IScoreDisplay[] critterScorePanels)
        {
            _arena = arena;
            _critterScorePanels = critterScorePanels;
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
                currentLevel.Launch(_critterScorePanels);
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
