using SCG.TurboSprite;
using SCG.TurboSprite.SpriteMover;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace CritterWorld
{
    public partial class Critterworld : Form
    {
        // Default name of log file.
        public string LogFileName { get; private set; } = "log.csv";

        // Level duration in seconds.
        const int levelDurationInSeconds = 60 * 3;

        // Start time of current level. This plus levelDuration is when the level shall end.
        DateTime levelStartTime;

        // Maximum number of Critters running at the same time.
        const int maxCrittersRunning = 25;

        // Total number of Critters loaded to run
        int critterCount = 0;

        // Current heat number
        int heatNumber = 0;

        // Available levels
        private Level[] levels = new Level[]
        {
            new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background00.png"), new Point(345, 186)),
            new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background01.png"), new Point(319, 247)),
            new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background02.png"), new Point(532, 32)),
            new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background03.png"), new Point(504, 269)),
            new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background04.png"), new Point(183, 279)),
            new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background05.png"), new Point(457, 440)),
            new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background06.png"), new Point(280, 360))
        };

        // Logging thread and logging flag. Set logging to false to shut down logging thread.
        private bool logging = false;
        private Thread logThread = null;

        // Log message queue.
        private static BlockingCollection<LogEntry> logMessageQueue = new BlockingCollection<LogEntry>();

        // Run this level
        private int levelNumber = 0;

        // The current running Level.
        private Level level;

        // Display update thread and flag. Set displayUpdating to false to shut down the update thread.
        private bool displayUpdating = false;
        private Thread displayUpdateThread = null;

        // To switch over from "GAME OVER" to splash
        private Timer gameOverTimer = null;

        // True if level timer is running.
        private bool levelTimerRunning = false;

        // Preserves window layout when going full screen
        private Size oldSize;
        private Point oldLocation;
        private FormWindowState oldState;
        private FormBorderStyle oldStyle;

        // Environment state
        private bool isFullScreen = false;
        private bool exiting = false;
        private bool IsCompetition { get; set; } = false;

        // Critter loader
        private CritterLoader critterLoader = new CritterLoader();

        // True if a Critter has escaped and we need to sort the Leader Board.
        private bool leaderBoardNeedsSorted;

        private bool Fullscreen
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
            foreach (Control control in panelScore.Controls)
            {
                CritterScorePanel scorePanel = (CritterScorePanel)control;
                scorePanel.SetCritter(null);
            }
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

        private void Launch()
        {
            level = levels[levelNumber];
            level.Arena = arena;
            level.Setup();

            labelLevelInfo.Text = "Level " + (levelNumber + 1) + " of " + levels.Length + " - Heat " + heatNumber + " of " + critterCount / maxCrittersRunning;

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
            List<Critter> critters = critterLoader.LoadCritters();
            foreach (Critter critter in critters)
            {
                critterBindingSourceWaiting.Add(critter);
                if (IsCompetition)
                {
                    critterBindingSourceLeaderboard.Add(critter);
                }
                critterCount++;
            }
        }

        private void NextLevel()
        {
            Shutdown();
            levelNumber++;
            heatNumber = 0;
            if (levelNumber >= levels.Length)
            {
                if (IsCompetition)
                {
                    DisplayGameOver();
                }
                else
                {
                    levelNumber = 0;
                    StartLevel();
                }
            }
            else
            {
                if (IsCompetition)
                {
                    // copy Critters from Leader Board, because they're all there
                    foreach (Critter critter in critterBindingSourceLeaderboard)
                    {
                        critter.Reset();
                        critterBindingSourceWaiting.Add(critter);
                    }
                }
                else
                {
                    LoadCrittersIntoWaitingRoom();
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
            NextHeat();
        }

        private void ExitApplication()
        {
            exiting = true;
            DisplayGameOver();
        }

        private void MenuStart_Click(object sender, EventArgs e)
        {
            critterBindingSourceLeaderboard.Clear();
            critterBindingSourceWaiting.Clear();
            IsCompetition = false;
            levelNumber = 0;
            StartLevel();
        }

        private void MenuCompetionStart_Click(object sender, EventArgs e)
        {
            critterBindingSourceLeaderboard.Clear();
            critterBindingSourceWaiting.Clear();
            IsCompetition = true;
            levelNumber = 0;
            StartLevel();
        }

        private void MenuFullScreen_Click(object sender, EventArgs e)
        {
            Fullscreen = !Fullscreen;
            menuFullScreen.Checked = Fullscreen;
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
            critterBindingSourceWaiting.Clear();
            DisplayGameOver();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void DisplayGameOver()
        {
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
                TargetMover spriteMover = (TargetMover)sprite.Mover;
                if (spriteMover == null || (spriteMover.SpeedX == 0 && spriteMover.SpeedY == 0))
                {
                    return;
                }
                spriteMover.TargetFacingAngle = (int)Sprite.GetAngle(spriteMover.SpeedX, spriteMover.SpeedY) + 90;
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
            int secondsRemaining = Math.Max(levelDurationInSeconds - (int)secondsElapsed, 0);
            levelTimeoutProgress.Value = secondsRemaining * 100 / levelDurationInSeconds;
            if (secondsElapsed >= levelDurationInSeconds || (secondsElapsed > 5 && level.CountOfActiveCritters == 0))
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
            arena.Width = 1024;
            arena.Height = 768;

            Location = new Point(20, 20);
            Width = Screen.PrimaryScreen.Bounds.Width - 50;
            Height = Screen.PrimaryScreen.Bounds.Height - 50;

            ForceLayout();
        }

        private void Critterworld_Resize(object sender, EventArgs e)
        {
            ForceLayout();
        }

        internal static void Log(LogEntry logEntry)
        {
            logMessageQueue.Add(logEntry);
        }

        private void AppendToLogDisplay(string text)
        {
            textLog.Invoke(new Action(() => {
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
            foreach (Control control in panelScore.Controls)
            {
                CritterScorePanel scorePanel = (CritterScorePanel)control;
                scorePanel.CritterUpdate();
            }
        }

        private void SortLeaderboard()
        {
            // Kludge to sort leaderboard. Replace this later.
            IList<Critter> unsortedCritters = (IList<Critter>)critterBindingSourceLeaderboard.List;
            IEnumerable<Critter> sortedCritters = unsortedCritters.OrderByDescending(order => order.OverallScore).ToList();
            critterBindingSourceLeaderboard.Clear();
            foreach (Critter critter in sortedCritters)
            {
                critterBindingSourceLeaderboard.Add(critter);
            }
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

        public Critterworld()
        {
            InitializeComponent();

            labelLevelInfo.Text = "";

            CreateCritterScorePanels();

            ForceInitialLayout();

            menuFullScreen.Checked = Fullscreen;
            menuFullScreen.ImageScaling = ToolStripItemImageScaling.None;       // fix alignment problem on HiDPI displays

            FormClosing += (sender, e) => ExitApplication();

            arena.CritterEscaped += (sender, e) => NotifyNeedToSortLeaderboard();

            labelVersion.Text = Version.VersionName;

            DisplaySplashOrExit();

            LaunchDisplayUpdate();
            LaunchLogger();
        }

    }

    internal class LogEntry
    {
        public LogEntry(int critterNumber, string critterName, string author, string eventMessage, Exception exception = null)
        {
            Timestamp = DateTime.Now;
            CritterNumber = critterNumber;
            CritterName = critterName;
            Author = author;
            EventMessage = eventMessage;
            Exception = exception;
        }

        public DateTime Timestamp { get; }
        public int CritterNumber { get; }
        public string CritterName { get; }
        public string Author { get; }
        public string EventMessage { get; }
        public Exception Exception { get; }

        private static string ToQuoted(string input)
        {
            return "\"" + input.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t") + "\"";
        }

        public override string ToString()
        {
            return Timestamp.ToString("o", CultureInfo.CurrentCulture) + ": #" + CritterNumber + " " + CritterName + " by " + Author + " " + EventMessage + ((Exception != null) ? " due to exception: " + Exception.StackTrace : "");
        }

        public string ToCSV()
        {
            return Timestamp.ToString("o", CultureInfo.CurrentCulture) + ", " + CritterNumber + ", " + ToQuoted(CritterName) + ", " + ToQuoted(Author) + ", " + ToQuoted(EventMessage) + ", " + ((Exception == null) ? ToQuoted("") : ToQuoted(Exception.StackTrace));
        }
    }

}
