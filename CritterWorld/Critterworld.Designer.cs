using System;

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
            this.components = new System.ComponentModel.Container();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.labelVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPadding1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.levelTimeoutProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripPadding2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelLevelInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLevelStart = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCompetitionStart = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNextHeat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNextLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFullScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.textLog = new System.Windows.Forms.TextBox();
            this.dataGridViewLeaderboard = new System.Windows.Forms.DataGridView();
            this.numberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.overallScoreDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.critterBindingSourceLeaderboard = new System.Windows.Forms.BindingSource(this.components);
            this.labelLeaderboard = new System.Windows.Forms.Label();
            this.labelWaiting = new System.Windows.Forms.Label();
            this.dataGridViewWaiting = new System.Windows.Forms.DataGridView();
            this.numberDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authorDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.critterBindingSourceWaiting = new System.Windows.Forms.BindingSource(this.components);
            this.panelScore = new System.Windows.Forms.Panel();
            this.arena = new CritterWorld.Arena();
            this.menuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeaderboard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.critterBindingSourceLeaderboard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWaiting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.critterBindingSourceWaiting)).BeginInit();
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
            this.labelLevelInfo});
            this.statusStrip.Location = new System.Drawing.Point(0, 530);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1330, 22);
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
            this.toolStripPadding1.Size = new System.Drawing.Size(162, 17);
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
            this.toolStripPadding2.Size = new System.Drawing.Size(162, 17);
            this.toolStripPadding2.Spring = true;
            // 
            // labelLevelInfo
            // 
            this.labelLevelInfo.Name = "labelLevelInfo";
            this.labelLevelInfo.Size = new System.Drawing.Size(170, 17);
            this.labelLevelInfo.Text = "Level 999 of 999 - Heat 99 of 99";
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1330, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLevelStart,
            this.menuCompetitionStart,
            this.menuNextHeat,
            this.menuNextLevel,
            this.toolStripSeparator3,
            this.menuStop,
            this.toolStripSeparator2,
            this.menuProperties,
            this.toolStripSeparator1,
            this.menuExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // menuLevelStart
            // 
            this.menuLevelStart.Name = "menuLevelStart";
            this.menuLevelStart.Size = new System.Drawing.Size(220, 22);
            this.menuLevelStart.Text = "Start running";
            this.menuLevelStart.Click += new System.EventHandler(this.MenuStart_Click);
            // 
            // menuCompetitionStart
            // 
            this.menuCompetitionStart.Name = "menuCompetitionStart";
            this.menuCompetitionStart.Size = new System.Drawing.Size(220, 22);
            this.menuCompetitionStart.Text = "Start running a competition";
            this.menuCompetitionStart.Click += new System.EventHandler(this.MenuCompetionStart_Click);
            // 
            // menuNextHeat
            // 
            this.menuNextHeat.Enabled = false;
            this.menuNextHeat.Name = "menuNextHeat";
            this.menuNextHeat.Size = new System.Drawing.Size(220, 22);
            this.menuNextHeat.Text = "Next heat";
            this.menuNextHeat.Click += new System.EventHandler(this.MenuNextHeat_Click);
            // 
            // menuNextLevel
            // 
            this.menuNextLevel.Enabled = false;
            this.menuNextLevel.Name = "menuNextLevel";
            this.menuNextLevel.Size = new System.Drawing.Size(220, 22);
            this.menuNextLevel.Text = "Next level";
            this.menuNextLevel.Click += new System.EventHandler(this.MenuNextLevel_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(217, 6);
            // 
            // menuStop
            // 
            this.menuStop.Enabled = false;
            this.menuStop.Name = "menuStop";
            this.menuStop.Size = new System.Drawing.Size(220, 22);
            this.menuStop.Text = "Stop";
            this.menuStop.Click += new System.EventHandler(this.MenuStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(217, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(220, 22);
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
            this.menuFullScreen.Size = new System.Drawing.Size(180, 22);
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
            // dataGridViewLeaderboard
            // 
            this.dataGridViewLeaderboard.AllowUserToAddRows = false;
            this.dataGridViewLeaderboard.AllowUserToDeleteRows = false;
            this.dataGridViewLeaderboard.AllowUserToOrderColumns = true;
            this.dataGridViewLeaderboard.AutoGenerateColumns = false;
            this.dataGridViewLeaderboard.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewLeaderboard.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewLeaderboard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLeaderboard.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numberDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.authorDataGridViewTextBoxColumn,
            this.overallScoreDataGridViewTextBoxColumn});
            this.dataGridViewLeaderboard.DataSource = this.critterBindingSourceLeaderboard;
            this.dataGridViewLeaderboard.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewLeaderboard.Location = new System.Drawing.Point(1110, 61);
            this.dataGridViewLeaderboard.MultiSelect = false;
            this.dataGridViewLeaderboard.Name = "dataGridViewLeaderboard";
            this.dataGridViewLeaderboard.ReadOnly = true;
            this.dataGridViewLeaderboard.RowHeadersVisible = false;
            this.dataGridViewLeaderboard.Size = new System.Drawing.Size(177, 150);
            this.dataGridViewLeaderboard.TabIndex = 5;
            // 
            // numberDataGridViewTextBoxColumn
            // 
            this.numberDataGridViewTextBoxColumn.DataPropertyName = "Number";
            this.numberDataGridViewTextBoxColumn.HeaderText = "Number";
            this.numberDataGridViewTextBoxColumn.Name = "numberDataGridViewTextBoxColumn";
            this.numberDataGridViewTextBoxColumn.ReadOnly = true;
            this.numberDataGridViewTextBoxColumn.Width = 69;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 60;
            // 
            // authorDataGridViewTextBoxColumn
            // 
            this.authorDataGridViewTextBoxColumn.DataPropertyName = "Author";
            this.authorDataGridViewTextBoxColumn.HeaderText = "Author";
            this.authorDataGridViewTextBoxColumn.Name = "authorDataGridViewTextBoxColumn";
            this.authorDataGridViewTextBoxColumn.ReadOnly = true;
            this.authorDataGridViewTextBoxColumn.Width = 63;
            // 
            // overallScoreDataGridViewTextBoxColumn
            // 
            this.overallScoreDataGridViewTextBoxColumn.DataPropertyName = "OverallScore";
            this.overallScoreDataGridViewTextBoxColumn.HeaderText = "Score";
            this.overallScoreDataGridViewTextBoxColumn.Name = "overallScoreDataGridViewTextBoxColumn";
            this.overallScoreDataGridViewTextBoxColumn.ReadOnly = true;
            this.overallScoreDataGridViewTextBoxColumn.Width = 60;
            // 
            // critterBindingSourceLeaderboard
            // 
            this.critterBindingSourceLeaderboard.DataSource = typeof(CritterWorld.Critter);
            // 
            // labelLeaderboard
            // 
            this.labelLeaderboard.AutoSize = true;
            this.labelLeaderboard.Location = new System.Drawing.Point(1088, 45);
            this.labelLeaderboard.Name = "labelLeaderboard";
            this.labelLeaderboard.Size = new System.Drawing.Size(71, 13);
            this.labelLeaderboard.TabIndex = 6;
            this.labelLeaderboard.Text = "Leader Board";
            // 
            // labelWaiting
            // 
            this.labelWaiting.AutoSize = true;
            this.labelWaiting.Location = new System.Drawing.Point(1088, 329);
            this.labelWaiting.Name = "labelWaiting";
            this.labelWaiting.Size = new System.Drawing.Size(43, 13);
            this.labelWaiting.TabIndex = 8;
            this.labelWaiting.Text = "Waiting";
            // 
            // dataGridViewWaiting
            // 
            this.dataGridViewWaiting.AllowUserToAddRows = false;
            this.dataGridViewWaiting.AllowUserToDeleteRows = false;
            this.dataGridViewWaiting.AllowUserToOrderColumns = true;
            this.dataGridViewWaiting.AutoGenerateColumns = false;
            this.dataGridViewWaiting.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewWaiting.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewWaiting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWaiting.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numberDataGridViewTextBoxColumn1,
            this.nameDataGridViewTextBoxColumn1,
            this.authorDataGridViewTextBoxColumn1});
            this.dataGridViewWaiting.DataSource = this.critterBindingSourceWaiting;
            this.dataGridViewWaiting.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewWaiting.Location = new System.Drawing.Point(1091, 345);
            this.dataGridViewWaiting.MultiSelect = false;
            this.dataGridViewWaiting.Name = "dataGridViewWaiting";
            this.dataGridViewWaiting.ReadOnly = true;
            this.dataGridViewWaiting.RowHeadersVisible = false;
            this.dataGridViewWaiting.Size = new System.Drawing.Size(177, 150);
            this.dataGridViewWaiting.TabIndex = 7;
            // 
            // numberDataGridViewTextBoxColumn1
            // 
            this.numberDataGridViewTextBoxColumn1.DataPropertyName = "Number";
            this.numberDataGridViewTextBoxColumn1.HeaderText = "Number";
            this.numberDataGridViewTextBoxColumn1.Name = "numberDataGridViewTextBoxColumn1";
            this.numberDataGridViewTextBoxColumn1.ReadOnly = true;
            this.numberDataGridViewTextBoxColumn1.Width = 69;
            // 
            // nameDataGridViewTextBoxColumn1
            // 
            this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn1.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
            this.nameDataGridViewTextBoxColumn1.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn1.Width = 60;
            // 
            // authorDataGridViewTextBoxColumn1
            // 
            this.authorDataGridViewTextBoxColumn1.DataPropertyName = "Author";
            this.authorDataGridViewTextBoxColumn1.HeaderText = "Author";
            this.authorDataGridViewTextBoxColumn1.Name = "authorDataGridViewTextBoxColumn1";
            this.authorDataGridViewTextBoxColumn1.ReadOnly = true;
            this.authorDataGridViewTextBoxColumn1.Width = 63;
            // 
            // critterBindingSourceWaiting
            // 
            this.critterBindingSourceWaiting.DataSource = typeof(CritterWorld.Critter);
            // 
            // panelScore
            // 
            this.panelScore.AutoScroll = true;
            this.panelScore.Location = new System.Drawing.Point(868, 61);
            this.panelScore.Name = "panelScore";
            this.panelScore.Size = new System.Drawing.Size(199, 99);
            this.panelScore.TabIndex = 9;
            // 
            // arena
            // 
            this.arena.Location = new System.Drawing.Point(0, 24);
            this.arena.Margin = new System.Windows.Forms.Padding(6);
            this.arena.Name = "arena";
            this.arena.Size = new System.Drawing.Size(842, 490);
            this.arena.TabIndex = 3;
            // 
            // menuProperties
            // 
            this.menuProperties.Name = "menuProperties";
            this.menuProperties.Size = new System.Drawing.Size(220, 22);
            this.menuProperties.Text = "Properties...";
            this.menuProperties.Click += new System.EventHandler(this.MenuProperties_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(217, 6);
            // 
            // Critterworld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1330, 552);
            this.Controls.Add(this.panelScore);
            this.Controls.Add(this.labelWaiting);
            this.Controls.Add(this.dataGridViewWaiting);
            this.Controls.Add(this.labelLeaderboard);
            this.Controls.Add(this.dataGridViewLeaderboard);
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeaderboard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.critterBindingSourceLeaderboard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWaiting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.critterBindingSourceWaiting)).EndInit();
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
        private System.Windows.Forms.ToolStripStatusLabel labelLevelInfo;
        private System.Windows.Forms.ToolStripMenuItem menuCompetitionStart;
        private System.Windows.Forms.ToolStripMenuItem menuNextLevel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripStatusLabel labelVersion;
        private System.Windows.Forms.ToolStripProgressBar levelTimeoutProgress;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuFullScreen;
        private System.Windows.Forms.TextBox textLog;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPadding2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPadding1;
        private System.Windows.Forms.DataGridView dataGridViewLeaderboard;
        private System.Windows.Forms.Label labelLeaderboard;
        private System.Windows.Forms.Label labelWaiting;
        private System.Windows.Forms.DataGridView dataGridViewWaiting;
        private System.Windows.Forms.Panel panelScore;
        private System.Windows.Forms.ToolStripMenuItem menuNextHeat;
        private System.Windows.Forms.BindingSource critterBindingSourceLeaderboard;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn authorDataGridViewTextBoxColumn1;
        private System.Windows.Forms.BindingSource critterBindingSourceWaiting;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn authorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn overallScoreDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuProperties;
    }
}