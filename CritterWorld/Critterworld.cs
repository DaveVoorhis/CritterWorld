using SCG.TurboSprite;
using SCG.TurboSprite.SpriteMover;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace CritterWorld
{
    public partial class Critterworld : Form
    {
        // Default name of log file.
        public string LogFileName { get; private set; } = "log.csv";

        // Level duration in seconds.
        const int levelDuration = 60 * 3;

        // Maximum number of Critters running at the same time.
        const int maxCrittersRunning = 25;

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

        // Run this level when not running a competition
        const int singleLevel = 5;

        // Log message queue.
        private static ConcurrentQueue<LogEntry> logMessageQueue = new ConcurrentQueue<LogEntry>();

        // Used to create animated activity indicator beside FPS display.
        private static string tickLine = ".....";
        private int tickCount = 0;

        // If we're not running a competition, we're running this level
        private Level level;

        // If we're running a competition, this is not null
        private Competition competition;

        // To update the FPS display
        private System.Timers.Timer fpsDisplayTimer = null;

        // To update the message log
        private System.Timers.Timer logMessageTimer = null;

        // To switch over from "GAME OVER" to splash
        private System.Timers.Timer gameOverTimer = null;

        // To terminate a level after levelDuration seconds...
        private System.Timers.Timer levelTimer = null;

        // ...by counting down a second at a time
        private int countDown;

        // Preserves window layout when going full screen
        private Size oldSize;
        private Point oldLocation;
        private FormWindowState oldState;
        private FormBorderStyle oldStyle;

        // Environment state
        private bool isFullScreen = false;
        private bool exiting = false;

        // Critter loader
        private CritterLoader critterLoader = new CritterLoader();

        // Critters waiting to run.
        private List<Critter> waitingRoom = new List<Critter>();

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

        private String TickShow()
        {
            tickCount = (tickCount + 1) % tickLine.Length;
            return " " + tickLine.Substring(tickCount) + " " + tickLine.Substring(tickLine.Length - tickCount) + ".";
        }

        private void ClearScorePanel()
        {
            if (IsHandleCreated)
            {
                Invoke(new Action(() => {
                    foreach (Control control in panelScore.Controls)
                    {
                        CritterScorePanel scorePanel = (CritterScorePanel)control;
                        scorePanel.Shutdown();
                    }
                    panelScore.Controls.Clear();
                }));
            }
        }

        private void Shutdown()
        {
            if (gameOverTimer != null)
            {
                gameOverTimer.Stop();
            }
            LevelTimerStop();
            arena.Shutdown();
            level = null;
            if (competition != null)
            {
                competition.Shutdown();
                competition = null;
            }
            ClearScorePanel();
        }

        private void AddCrittersToArena()
        {
            ClearScorePanel();
            if (waitingRoom.Count == 0)
            {
                waitingRoom = critterLoader.LoadCritters();
            }
            for (int i = 0; i < maxCrittersRunning; i++)
            {
                if (waitingRoom.Count == 0)
                {
                    break;
                }
                Critter critter = waitingRoom[0];
                waitingRoom.RemoveAt(0);
                arena.AddCritter(critter);
                Invoke(new Action(() => {
                    CritterScorePanel scorePanel = new CritterScorePanel(critter);
                    scorePanel.Location = new Point(0, scorePanel.Height * i);
                    panelScore.Controls.Add(scorePanel);
                }));
            }
            arena.Launch();
        }

        private void StartOneLevel()
        {
            Shutdown();
            LevelTimerStart();
            level = levels[singleLevel];
            level.Arena = arena;
            level.Setup();
            waitingRoom = critterLoader.LoadCritters();
            AddCrittersToArena();
        }

        private void NextLevel()
        {
            if (competition != null)
            {
                LevelTimerStart();
                competition.NextLevel();
            }
            else if (level != null)
            {
                StartOneLevel();
            }
        }

        private void ExitApplication()
        {
            exiting = true;
            DisplayGameOver();
        }

        private void MenuStart_Click(object sender, EventArgs e)
        {
            StartOneLevel();
        }

        private void MenuCompetionStart_Click(object sender, EventArgs e)
        {
            Shutdown();
            LevelTimerStart();
            competition = new Competition(arena, () => AddCrittersToArena());
            competition.Finished += (sndr, ev) => DisplayGameOver();
            competition.FinishedLevel += (sndr, ev) => LevelTimerStart();
            foreach (Level level in levels)
            {
                competition.Add(level);
            }
            competition.Launch();
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

        private void MenuStop_Click(object sender, EventArgs e)
        {
            DisplayGameOver();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void DisplayGameOver()
        {
            Shutdown();
            LevelTimerStop();
            TextSprite splashText = new TextSprite((exiting) ? "GOODBYE!" : "GAME OVER", "Arial", 1, FontStyle.Regular);
            splashText.Mover = new TextGrower(1, 100, 3);
            arena.AddSprite(splashText);
            splashText.Position = new Point(arena.Width / 2, arena.Height / 2);
            if (gameOverTimer == null)
            {
                gameOverTimer = new System.Timers.Timer
                {
                    AutoReset = false,
                    Interval = (exiting) ? 1000 : 5000
                };
                gameOverTimer.Elapsed += (sender, e) => DisplaySplash();
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

        private void DisplaySplash()
        {
            Shutdown();
            LevelTimerStop();
            if (exiting)
            {
                logMessageTimer.Stop();
                fpsDisplayTimer.Stop();
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
            countDown--;
            if (countDown <= 0)
            {
                NextLevel();
            }
            else
            {
                Invoke(new Action(() => levelTimeoutProgress.Value = countDown * 100 / levelDuration));
            }
        }

        private void LevelTimerStart()
        {
            if (levelTimer == null)
            {
                levelTimer = new System.Timers.Timer();
                levelTimer.Interval = 1000;
                levelTimer.AutoReset = true;
                levelTimer.Elapsed += (sender, e) => Tick();
            }
            levelTimer.Stop();
            levelTimer.Start();
            countDown = levelDuration;
            Invoke(new Action(() => levelTimeoutProgress.Value = 100));
        }

        private void LevelTimerStop()
        {
            if (levelTimer != null)
            {
                levelTimer.Stop();
                Invoke(new Action(() => levelTimeoutProgress.Value = 0));
            }
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
            logMessageQueue.Enqueue(logEntry);
        }

        private void RetrieveAndDisplayLogMessages()
        {
            using (StreamWriter output = File.AppendText(LogFileName))
            {
                while (logMessageQueue.TryDequeue(out LogEntry logEntry))
                {
                    output.WriteLine(logEntry.ToCSV());
                    textLog.AppendText(logEntry + "\r\n");
                    if (textLog.Text.Length > 128000)
                    {
                        String shortenedText = "..." + "\r\n" + textLog.Text.Substring(128000);
                        textLog.Text = "";
                        textLog.AppendText(shortenedText);
                    }
                }
            }
        }

        public Critterworld()
        {
            InitializeComponent();

            ForceInitialLayout();

            menuFullScreen.Checked = Fullscreen;
            menuFullScreen.ImageScaling = ToolStripItemImageScaling.None;       // fix alignment problem on HiDPI displays

            FormClosing += (sender, e) => ExitApplication();

            labelVersion.Text = Version.VersionName;

            DisplaySplash();

            fpsDisplayTimer = new System.Timers.Timer();
            fpsDisplayTimer.Interval = 250;
            fpsDisplayTimer.Elapsed += (sender, e) => Invoke(new Action(() => labelFPS.Text = arena.ActualFPS + " FPS" + TickShow()));
            fpsDisplayTimer.Start();

            logMessageTimer = new System.Timers.Timer();
            logMessageTimer.Interval = 250;
            logMessageTimer.Elapsed += (sender, e) => Invoke(new Action(() => RetrieveAndDisplayLogMessages()));
            logMessageTimer.Start();
        }
    }

    internal class LogEntry
    {
        public LogEntry(int critterNumber, string critterName, string author, string eventMessage, Exception exception)
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

        public bool Matches(LogEntry other)
        {
            string formatString = "MM/dd/yyyy HH:mm:ss.f";
            CultureInfo culture = CultureInfo.InvariantCulture;
            return Timestamp.ToString(formatString, culture) == other.Timestamp.ToString(formatString, culture) && EventMessage == other.EventMessage && Exception == null;
        }

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
