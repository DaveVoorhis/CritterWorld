using SCG.TurboSprite;
using SCG.TurboSprite.SpriteMover;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace CritterWorld
{
    public partial class Critterworld : Form
    {
        // Arena dimensions
        public static int ArenaWidth { get; } = 1100;
        public static int ArenaHeight { get; } = 825;

        // Default name of log file.
        public string LogFileName { get; private set; } = "log.csv";

        // Level duration in seconds.
        public const int LevelDurationInSeconds = 60 * 3;

        // Start time of current level. This plus levelDuration is when the level shall end.
        DateTime levelStartTime;

        // Approximate time remaining on this level in seconds. (Use of static is questionable... But simplifies getting this value in Critter.)
        public static int LevelTimeRemaining { get; private set; }

        // Maximum number of Critters running at the same time.
        const int maxCrittersRunning = 25;

        // Total number of Critters loaded to run
        int critterCount = 0;

        // Current heat number
        int heatNumber = 0;

        // Logging thread and logging flag. Set logging to false to shut down logging thread.
        bool logging = false;
        Thread logThread = null;

        // Log message queue.
        static BlockingCollection<LogEntry> logMessageQueue = new BlockingCollection<LogEntry>();

        // Run this level
        int levelNumber = 0;

        // The current running Level.
        Level level;

        // If in free-run mode, how many cycles have run?
        int cycleCounter = 0;

        // Display update thread and flag. Set displayUpdating to false to shut down the update thread.
        bool displayUpdating = false;
        Thread displayUpdateThread = null;

        // To time how long "GAME OVER" is displayed before switching to attract mode
        Timer gameOverTimer = null;

        // True if level timer is running.
        bool levelTimerRunning = false;

        // Preserves window layout when going full screen
        Size oldSize;
        Point oldLocation;
        FormWindowState oldState;
        FormBorderStyle oldStyle;

        // Environment state
        bool isFullScreen = false;
        bool exiting = false;
        bool IsCompetition { get; set; } = false;

        // Critter loader
        CritterLoader critterLoader = new CritterLoader();

        // True if a Critter has escaped and we need to sort the Leader Board.
        bool leaderBoardNeedsSorted;

        // Fullscreen mode.
        bool Fullscreen
        {
            get
            {
                return isFullScreen;
            }
            set
            {
                if (value)
                {
                    oldSize = Size;
                    oldState = WindowState;
                    oldStyle = FormBorderStyle;
                    oldLocation = Location;
                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = FormBorderStyle.None;
                    Bounds = Screen.PrimaryScreen.Bounds;
                    isFullScreen = true;
                }
                else
                {
                    Location = oldLocation;
                    WindowState = oldState;
                    FormBorderStyle = oldStyle;
                    Size = oldSize;
                    isFullScreen = false;
                }
            }
        }

        private void ClearScorePanel()
        {
            panelScore.Controls.OfType<CritterScorePanel>().ToList().ForEach(scorePanel => scorePanel.SetCritter(null));
        }

        private void AddCrittersToArena()
        {
            ClearScorePanel();
            for (int i = 0; i < maxCrittersRunning; i++)
            {
                if (critterBindingSourceWaiting.Count != 0)
                {
                    Critter critter = (Critter)critterBindingSourceWaiting.List[0];
                    critterBindingSourceWaiting.RemoveAt(0);
                    arena.AddCritter(critter);
                    CritterScorePanel scorePanel = (CritterScorePanel)panelScore.Controls[i];
                    scorePanel.SetCritter(critter);
                }
            }
        }

        private void ShowLevelInformationInStatusBar()
        {
            string levelInfo;
            if (IsCompetition)
            {
                levelInfo = "Competition Level " + (levelNumber + 1) + " of " + Levels.TheLevels.Length;
            }
            else
            {
                levelInfo = "Free-run Cycle " + cycleCounter + " - Level " + (levelNumber + 1);
            }
            if (critterCount <= maxCrittersRunning)
            {
                labelLevelInfo.Text = levelInfo;
            }
            else
            {
                labelLevelInfo.Text = levelInfo + " - Heat " + heatNumber + " of " + (int)Math.Ceiling(critterCount / (double)maxCrittersRunning);
            }
        }

        private void Launch()
        {
            level = Levels.TheLevels[levelNumber];
            level.Arena = arena;
            level.Setup();

            ShowLevelInformationInStatusBar();
            AddCrittersToArena();
            LevelTimerStart();
            arena.Launch();
        }

        private void Shutdown()
        {
            LevelTimerStop();
            if (gameOverTimer != null)
            {
                gameOverTimer.Stop();
            }
            arena.Shutdown();
            level = null;
            ClearScorePanel();
        }

        private void LoadCrittersIntoWaitingRoom()
        {
            critterCount = 0;
            critterBindingSourceWaiting.Clear();
            List<Critter> loadedCritters = critterLoader.LoadCritters(IsCompetition);
            if (loadedCritters.Count == 0)
            {
                DisplayGameOver();
                MessageBox.Show("Unable to load any Critters. Check system log for details.", "Load Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                loadedCritters.ForEach(critter =>
                {
                    critterBindingSourceWaiting.Add(critter);
                    if (IsCompetition)
                    {
                        critterBindingSourceLeaderboard.Add(critter);
                    }
                    critterCount++;
                });
            }
        }

        private void NextLevel()
        {
            Shutdown();
            levelNumber++;
            heatNumber = 0;
            if (levelNumber >= Levels.TheLevels.Length)
            {
                if (IsCompetition)
                {
                    DisplayGameOver();
                }
                else
                {
                    levelNumber = 0;
                    cycleCounter++;
                    StartLevel();
                }
            }
            else
            {
                if (IsCompetition)
                {
                    // copy Critters from Leader Board, because they're all there
                    critterBindingSourceLeaderboard.OfType<Critter>().ToList().ForEach(critter => 
                    {
                        critter.Reset();
                        critterBindingSourceWaiting.Add(critter);
                    });
                }
                else
                {
                    LoadCrittersIntoWaitingRoom();
                    if (critterCount == 0)
                    {
                        DisplayGameOver();
                    }
                }
                NextHeat();
            }
        }

        private void NextHeat()
        {
            if (critterBindingSourceWaiting.Count == 0)
            {
                heatNumber = 1;
                NextLevel();
            }
            else
            {
                heatNumber++;
                Shutdown();
                Launch();
            }
        }

        private void StartLevel()
        {
            heatNumber = 0;
            Shutdown();
            LoadCrittersIntoWaitingRoom();
            if (critterCount > 0)
            {
                NextHeat();
            }
        }

        private void ExitApplication()
        {
            exiting = true;
            DisplayGameOver();
        }

        private void Start()
        {
            critterBindingSourceLeaderboard.Clear();
            critterBindingSourceWaiting.Clear();
            levelNumber = 0;
            menuNextHeat.Enabled = true;
            menuNextLevel.Enabled = true;
            menuStop.Enabled = true;
            StartLevel();
        }

        private void StartFreerun()
        {
            IsCompetition = false;
            cycleCounter = 1;
            Start();
        }

        private void StartCompetition()
        {
            IsCompetition = true;
            Start();
        }

        private void ToggleFullscreen()
        {
            Fullscreen = !Fullscreen;
            menuFullScreen.Checked = Fullscreen;
        }

        private void Stop()
        {
            critterBindingSourceWaiting.Clear();
            DisplayGameOver();
        }

        private void ShowProperties()
        {
            PropertiesDialog.Launch();
        }

        private void DisplayGameOver()
        {
            menuNextHeat.Enabled = false;
            menuNextLevel.Enabled = false;
            menuStop.Enabled = false;
            labelLevelInfo.Text = "";
            Shutdown();
            TextSprite splashText = new TextSprite((exiting) ? "GOODBYE!" : "GAME OVER", "Arial", 1, FontStyle.Regular)
            {
                Mover = new TextGrower(1, 100, 3)
            };
            arena.AddSprite(splashText);
            splashText.Position = new Point(arena.Width / 2, arena.Height / 2);
            if (gameOverTimer == null)
            {
                gameOverTimer = new Timer
                {
                    Interval = (exiting) ? 1000 : 5000
                };
                gameOverTimer.Tick += (sender, e) => DisplaySplashOrExit();
            }
            gameOverTimer.Start();
            arena.Launch();
        }

        private void DisplayCritterworldText()
        {
            Sprite splashText = new TextSprite("CritterWorld", "Arial", 150, FontStyle.Regular);
            arena.AddSprite(splashText);
            splashText.Position = new Point(arena.Width / 2, arena.Height / 2 - 100);
            TextTwitcher splashTextTwitcher = new TextTwitcher
            {
                PositionTwitchRange = 2,
                SizeTwitchPercentage = 0
            };
            splashText.Mover = splashTextTwitcher;
        }

        private void DisplayVersion()
        {
            TextSprite splashTextVersion = new TextSprite("2", "Arial", 250, FontStyle.Bold);
            arena.AddSprite(splashTextVersion);
            splashTextVersion.Position = new Point(arena.Width / 2, arena.Height / 2 + 150);
            splashTextVersion.Color = Color.Green;
            TextTwitcher splashTextVersionTwitcher = new TextTwitcher
            {
                PositionTwitchRange = 3,
                SizeTwitchPercentage = 50
            };
            splashTextVersion.Mover = splashTextVersionTwitcher;
        }

        private void DisplayWanderingCritter()
        {
            PolygonSprite wanderer = new PolygonSprite((new CritterBody()).GetBody());
            wanderer.Color = Sprite.RandomColor(127);
            wanderer.Processors += sprite =>
            {
                if (sprite.Mover is TargetMover spriteMover && (spriteMover.SpeedX != 0 || spriteMover.SpeedY != 0))
                {
                    spriteMover.TargetFacingAngle = (int)Sprite.GetAngle(spriteMover.SpeedX, spriteMover.SpeedY) + 90;
                }
            };
            int margin = 20;
            int speed = 4;
            int moveCount = 0;
            Route route = new Route(wanderer);
            route.SpriteMoved += (sender, spriteEvent) =>
            {
                if (moveCount-- == 0)
                {
                    wanderer.IncrementFrame();
                    moveCount = 5 - Math.Min(5, speed);
                }
            };
            route.Add(margin, margin, speed);
            route.Add(arena.Width - margin, margin, speed);
            route.Add(arena.Width - margin, arena.Height / 2 - margin, speed);
            route.Add(margin, arena.Height / 2 - margin, speed);
            route.Add(margin, arena.Height - margin, speed);
            route.Add(arena.Width - margin, arena.Height - margin, speed);
            route.Add(arena.Width - margin, arena.Height / 2 - margin, speed);
            route.Add(margin, arena.Height / 2 - margin, speed);
            route.Repeat = true;
            arena.AddSprite(wanderer);
            route.Start();
        }

        private void DisplaySplashOrExit()
        {
            Shutdown();
            if (exiting)
            {
                ShutdownLogger();
                ShutdownDisplayUpdate();
                Application.Exit();
            }
            else
            {
                DisplayCritterworldText();
                DisplayVersion();
                DisplayWanderingCritter();
                arena.Launch();
            }
        }

        private void Tick()
        {
            double secondsElapsed = (DateTime.Now - levelStartTime).TotalSeconds;
            LevelTimeRemaining = Math.Max(LevelDurationInSeconds - (int)secondsElapsed, 0);
            levelTimeoutProgress.Value = LevelTimeRemaining * 100 / LevelDurationInSeconds;
            if (secondsElapsed >= LevelDurationInSeconds || (secondsElapsed > 5 && level.CountOfActiveCritters == 0))
            {
                NextHeat();
            }
        }

        private void LevelTimerStart()
        {
            levelStartTime = DateTime.Now;
            levelTimeoutProgress.Value = 100;
            levelTimerRunning = true;
        }

        private void LevelTimerStop()
        {
            levelTimerRunning = false;
            levelTimeoutProgress.Value = 0;
        }

        // Use explicit layout to get around issues with HiDPI displays.
        private void ForceLayout()
        {
            levelTimeoutProgress.Width = (int)(Width * 0.75);

            int heightOfDisplayArea = ClientRectangle.Height - statusStrip.Height - menuStrip.Height;

            textLog.Bounds = new Rectangle(0, arena.Bottom, arena.Width, heightOfDisplayArea - arena.Height);

            int widthToRightOfArena = ClientRectangle.Width - arena.Width;
            panelScore.Bounds = new Rectangle(arena.Right, menuStrip.Height, widthToRightOfArena / 2, heightOfDisplayArea);

            labelLeaderboard.Location = new Point(panelScore.Right, menuStrip.Height);

            dataGridViewLeaderboard.Bounds = new Rectangle(panelScore.Right, labelLeaderboard.Bottom, widthToRightOfArena / 2, (heightOfDisplayArea - labelLeaderboard.Bounds.Height) / 2);

            labelWaiting.Location = new Point(panelScore.Right, dataGridViewLeaderboard.Bottom);

            dataGridViewWaiting.Bounds = new Rectangle(panelScore.Right, labelWaiting.Bottom, ClientRectangle.Width - arena.Width - panelScore.Width, heightOfDisplayArea - labelWaiting.Height - dataGridViewLeaderboard.Height - labelLeaderboard.Height);
        }

        // Use explicit layout to get around issues with HiDPI displays.
        private void ForceInitialLayout()
        {
            arena.Width = ArenaWidth;
            arena.Height = ArenaHeight;

            Location = new Point(20, 20);
            Width = Screen.PrimaryScreen.Bounds.Width - 20;
            Height = Screen.PrimaryScreen.Bounds.Height - 20;

            ForceLayout();
        }

        private void Critterworld_Resize(object sender, EventArgs e)
        {
            ForceLayout();
        }

        public static void Log(LogEntry logEntry)
        {
            logMessageQueue.Add(logEntry);
        }

        private void AppendToLogDisplay(string text)
        {
            textLog.Invoke(new Action(() =>
            {
                if (textLog.Text.Length > 128000)
                {
                    textLog.Text = "..." + textLog.Text.Substring(64000);
                }
                textLog.AppendText(text);
            }));
        }

        private void RetrieveAndDisplayLogMessages()
        {
            foreach (LogEntry logEntry in logMessageQueue.GetConsumingEnumerable())
            {
                if (!logging)
                {
                    break;
                }
                try
                {
                    using (StreamWriter output = File.AppendText(LogFileName))
                    {
                        output.WriteLine(logEntry.ToCSV());
                    }
                }
                catch (Exception e)
                {
                    AppendToLogDisplay("=== Unable to write to log file " + LogFileName + " due to " + e.Message + " ===\r\n");
                }
                AppendToLogDisplay(logEntry + "\r\n");
            }
        }

        private void LaunchLogger()
        {
            logging = true;
            logThread = new Thread(() => RetrieveAndDisplayLogMessages());
            logThread.IsBackground = true;
            logThread.Start();
        }

        public void ShutdownLogger()
        {
            logging = false;
            Log(new LogEntry(0, "LogEnd", "Critterworld", "End of log"));
        }

        private void CreateCritterScorePanels()
        {
            for (int i = 0; i < maxCrittersRunning; i++)
            {
                CritterScorePanel scorePanel = new CritterScorePanel();
                scorePanel.Location = new Point(0, scorePanel.Height * i);
                panelScore.Controls.Add(scorePanel);
            }
        }

        private void UpdateCritterScorePanels()
        {
            panelScore.Controls.OfType<CritterScorePanel>().ToList().ForEach(scorePanel => scorePanel.CritterUpdate());
        }

        private void SortLeaderboard()
        {
            // Kludge to sort leaderboard. Replace this later.
            IList<Critter> unsortedCritters = (IList<Critter>)critterBindingSourceLeaderboard.List;
            IEnumerable<Critter> sortedCritters = unsortedCritters.OrderByDescending(order => order.OverallScore).ToList();
            critterBindingSourceLeaderboard.Clear();
            sortedCritters.ToList().ForEach(critter => critterBindingSourceLeaderboard.Add(critter));
            leaderBoardNeedsSorted = false;
        }

        private void NotifyNeedToSortLeaderboard()
        {
            leaderBoardNeedsSorted = true;
        }

        private void UpdateDisplay()
        {
            while (displayUpdating)
            {
                if (IsHandleCreated)
                {
                    Invoke(new Action(() =>
                    {
                        UpdateCritterScorePanels();
                        if (leaderBoardNeedsSorted)
                        {
                            SortLeaderboard();
                        }
                        if (levelTimerRunning)
                        {
                            Tick();
                        }
                    }));
                }
                Thread.Sleep(250);
            }
        }

        private void LaunchDisplayUpdate()
        {
            displayUpdating = true;
            displayUpdateThread = new Thread(() => UpdateDisplay());
            displayUpdateThread.Start();
        }

        private void ShutdownDisplayUpdate()
        {
            displayUpdating = false;
        }

        private void MenuStart_Click(object sender, EventArgs e)
        {
            StartFreerun();
        }

        private void MenuCompetionStart_Click(object sender, EventArgs e)
        {
            StartCompetition();
        }

        private void MenuFullScreen_Click(object sender, EventArgs e)
        {
            ToggleFullscreen();
        }

        private void MenuNextLevel_Click(object sender, EventArgs e)
        {
            NextLevel();
        }

        private void MenuNextHeat_Click(object sender, EventArgs e)
        {
            NextHeat();
        }

        private void MenuStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void MenuProperties_Click(object sender, EventArgs e)
        {
            ShowProperties();
        }

        public Critterworld()
        {
            InitializeComponent();

            labelLevelInfo.Text = "";

            CreateCritterScorePanels();

            ForceInitialLayout();

            menuFullScreen.Checked = Fullscreen;
            menuFullScreen.ImageScaling = ToolStripItemImageScaling.None;       // fix alignment problem on HiDPI displays

            Closing += (sender, e) =>
            {
                e.Cancel = true;
                ExitApplication();
            };

            arena.CritterEscaped += (sender, e) => NotifyNeedToSortLeaderboard();

            labelVersion.Text = Version.VersionName;

            DisplaySplashOrExit();

            LaunchDisplayUpdate();
            LaunchLogger();
        }
    }

}
