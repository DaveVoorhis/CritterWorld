namespace DemonstrationCritters
{
    partial class ChaserSettings
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.trackBarNominalFeedingSpeed = new System.Windows.Forms.TrackBar();
            this.labelNominalSpeed = new System.Windows.Forms.Label();
            this.labelNominalSpeedShown = new System.Windows.Forms.Label();
            this.labelHeadForExitSpeedShown = new System.Windows.Forms.Label();
            this.labelHeadForExitSpeed = new System.Windows.Forms.Label();
            this.trackBarHeadForExitSpeed = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNominalFeedingSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeadForExitSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(393, 212);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(474, 212);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // trackBarNominalFeedingSpeed
            // 
            this.trackBarNominalFeedingSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarNominalFeedingSpeed.Location = new System.Drawing.Point(136, 12);
            this.trackBarNominalFeedingSpeed.Name = "trackBarNominalFeedingSpeed";
            this.trackBarNominalFeedingSpeed.Size = new System.Drawing.Size(388, 45);
            this.trackBarNominalFeedingSpeed.TabIndex = 2;
            this.trackBarNominalFeedingSpeed.Value = 5;
            this.trackBarNominalFeedingSpeed.ValueChanged += new System.EventHandler(this.TrackBarNominalFeedingSpeed_ValueChanged);
            // 
            // labelNominalSpeed
            // 
            this.labelNominalSpeed.AutoSize = true;
            this.labelNominalSpeed.Location = new System.Drawing.Point(12, 12);
            this.labelNominalSpeed.Name = "labelNominalSpeed";
            this.labelNominalSpeed.Size = new System.Drawing.Size(118, 13);
            this.labelNominalSpeed.TabIndex = 3;
            this.labelNominalSpeed.Text = "Nominal feeding speed:";
            // 
            // labelNominalSpeedShown
            // 
            this.labelNominalSpeedShown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNominalSpeedShown.AutoSize = true;
            this.labelNominalSpeedShown.Location = new System.Drawing.Point(536, 12);
            this.labelNominalSpeedShown.Name = "labelNominalSpeedShown";
            this.labelNominalSpeedShown.Size = new System.Drawing.Size(13, 13);
            this.labelNominalSpeedShown.TabIndex = 4;
            this.labelNominalSpeedShown.Text = "5";
            // 
            // labelHeadForExitSpeedShown
            // 
            this.labelHeadForExitSpeedShown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHeadForExitSpeedShown.AutoSize = true;
            this.labelHeadForExitSpeedShown.Location = new System.Drawing.Point(536, 63);
            this.labelHeadForExitSpeedShown.Name = "labelHeadForExitSpeedShown";
            this.labelHeadForExitSpeedShown.Size = new System.Drawing.Size(13, 13);
            this.labelHeadForExitSpeedShown.TabIndex = 7;
            this.labelHeadForExitSpeedShown.Text = "5";
            // 
            // labelHeadForExitSpeed
            // 
            this.labelHeadForExitSpeed.AutoSize = true;
            this.labelHeadForExitSpeed.Location = new System.Drawing.Point(12, 63);
            this.labelHeadForExitSpeed.Name = "labelHeadForExitSpeed";
            this.labelHeadForExitSpeed.Size = new System.Drawing.Size(102, 13);
            this.labelHeadForExitSpeed.TabIndex = 6;
            this.labelHeadForExitSpeed.Text = "Head-for-exit speed:";
            // 
            // trackBarHeadForExitSpeed
            // 
            this.trackBarHeadForExitSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarHeadForExitSpeed.Location = new System.Drawing.Point(136, 63);
            this.trackBarHeadForExitSpeed.Name = "trackBarHeadForExitSpeed";
            this.trackBarHeadForExitSpeed.Size = new System.Drawing.Size(388, 45);
            this.trackBarHeadForExitSpeed.TabIndex = 5;
            this.trackBarHeadForExitSpeed.Value = 5;
            this.trackBarHeadForExitSpeed.Scroll += new System.EventHandler(this.TrackBarHeadForExitSpeed_Scroll);
            // 
            // ChaserSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 247);
            this.Controls.Add(this.labelHeadForExitSpeedShown);
            this.Controls.Add(this.labelHeadForExitSpeed);
            this.Controls.Add(this.trackBarHeadForExitSpeed);
            this.Controls.Add(this.labelNominalSpeedShown);
            this.Controls.Add(this.labelNominalSpeed);
            this.Controls.Add(this.trackBarNominalFeedingSpeed);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "ChaserSettings";
            this.Text = "Chaser Settings";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNominalFeedingSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeadForExitSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TrackBar trackBarNominalFeedingSpeed;
        private System.Windows.Forms.Label labelNominalSpeed;
        private System.Windows.Forms.Label labelNominalSpeedShown;
        private System.Windows.Forms.Label labelHeadForExitSpeedShown;
        private System.Windows.Forms.Label labelHeadForExitSpeed;
        private System.Windows.Forms.TrackBar trackBarHeadForExitSpeed;
    }
}