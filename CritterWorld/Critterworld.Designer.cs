namespace CritterWorld
{
    partial class Critterworld
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.labelVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPadding1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.levelTimeoutProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripPadding2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelFPS = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLevelStart = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCompetitionStart = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNextLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFullScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.textLog = new System.Windows.Forms.TextBox();
            this.arena = new CritterWorld.Arena();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelVersion,
            this.toolStripPadding1,
            this.levelTimeoutProgress,
            this.toolStripPadding2,
            this.labelFPS});
            this.statusStrip.Location = new System.Drawing.Point(0, 645);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1268, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // labelVersion
            // 
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(118, 17);
            this.labelVersion.Text = "toolStripStatusLabel1";
            // 
            // toolStripPadding1
            // 
            this.toolStripPadding1.Name = "toolStripPadding1";
            this.toolStripPadding1.Size = new System.Drawing.Size(184, 17);
            this.toolStripPadding1.Spring = true;
            // 
            // levelTimeoutProgress
            // 
            this.levelTimeoutProgress.AutoSize = false;
            this.levelTimeoutProgress.Name = "levelTimeoutProgress";
            this.levelTimeoutProgress.Size = new System.Drawing.Size(700, 16);
            // 
            // toolStripPadding2
            // 
            this.toolStripPadding2.Name = "toolStripPadding2";
            this.toolStripPadding2.Size = new System.Drawing.Size(184, 17);
            this.toolStripPadding2.Spring = true;
            // 
            // labelFPS
            // 
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(64, 17);
            this.labelFPS.Text = "FPSFPSFPS";
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1268, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLevelStart,
            this.toolStripSeparator2,
            this.menuCompetitionStart,
            this.menuNextLevel,
            this.toolStripSeparator3,
            this.menuStop,
            this.toolStripSeparator1,
            this.menuExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // menuLevelStart
            // 
            this.menuLevelStart.Name = "menuLevelStart";
            this.menuLevelStart.Size = new System.Drawing.Size(168, 22);
            this.menuLevelStart.Text = "Start a Level";
            this.menuLevelStart.Click += new System.EventHandler(this.MenuStart_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(165, 6);
            // 
            // menuCompetitionStart
            // 
            this.menuCompetitionStart.Name = "menuCompetitionStart";
            this.menuCompetitionStart.Size = new System.Drawing.Size(168, 22);
            this.menuCompetitionStart.Text = "Start Competition";
            this.menuCompetitionStart.Click += new System.EventHandler(this.MenuCompetionStart_Click);
            // 
            // menuNextLevel
            // 
            this.menuNextLevel.Name = "menuNextLevel";
            this.menuNextLevel.Size = new System.Drawing.Size(168, 22);
            this.menuNextLevel.Text = "Next level";
            this.menuNextLevel.Click += new System.EventHandler(this.MenuNextLevel_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(165, 6);
            // 
            // menuStop
            // 
            this.menuStop.Name = "menuStop";
            this.menuStop.Size = new System.Drawing.Size(168, 22);
            this.menuStop.Text = "Stop";
            this.menuStop.Click += new System.EventHandler(this.MenuStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(165, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(168, 22);
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFullScreen});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // menuFullScreen
            // 
            this.menuFullScreen.Name = "menuFullScreen";
            this.menuFullScreen.Size = new System.Drawing.Size(130, 22);
            this.menuFullScreen.Text = "Full screen";
            this.menuFullScreen.Click += new System.EventHandler(this.MenuFullScreen_Click);
            // 
            // textLog
            // 
            this.textLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textLog.Location = new System.Drawing.Point(0, 523);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ReadOnly = true;
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(842, 119);
            this.textLog.TabIndex = 4;
            // 
            // arena
            // 
            this.arena.Location = new System.Drawing.Point(0, 24);
            this.arena.Margin = new System.Windows.Forms.Padding(6);
            this.arena.Name = "arena";
            this.arena.Size = new System.Drawing.Size(842, 490);
            this.arena.TabIndex = 3;
            // 
            // Critterworld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 667);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.arena);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Critterworld";
            this.Text = "Critterworld II";
            this.Resize += new System.EventHandler(this.Critterworld_Resize);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuLevelStart;
        private System.Windows.Forms.ToolStripMenuItem menuStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private Arena arena;
        private System.Windows.Forms.ToolStripStatusLabel labelFPS;
        private System.Windows.Forms.ToolStripMenuItem menuCompetitionStart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuNextLevel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripStatusLabel labelVersion;
        private System.Windows.Forms.ToolStripProgressBar levelTimeoutProgress;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuFullScreen;
        private System.Windows.Forms.TextBox textLog;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPadding2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPadding1;
    }
}