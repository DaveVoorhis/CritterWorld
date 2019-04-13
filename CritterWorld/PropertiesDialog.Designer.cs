namespace CritterWorld
{
    partial class PropertiesDialog
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelMaxControllersPerDLL = new System.Windows.Forms.Label();
            this.textBoxMaxControllersPerDLL = new System.Windows.Forms.TextBox();
            this.labelPathToDLLs = new System.Windows.Forms.Label();
            this.textBoxPathToCritterControllerDLLs = new System.Windows.Forms.TextBox();
            this.buttonLaunchDLLPathPicker = new System.Windows.Forms.Button();
            this.buttonLaunchCritterFilePicker = new System.Windows.Forms.Button();
            this.textBoxPathToFilesCreatedByCritterControllers = new System.Windows.Forms.TextBox();
            this.labelPathToControllerFiles = new System.Windows.Forms.Label();
            this.textBoxTerrainDetailFactor = new System.Windows.Forms.TextBox();
            this.labelTerrainDetailFactor = new System.Windows.Forms.Label();
            this.folderBrowserDialogCritterControllerDLL = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialogCritterCreatedFiles = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonRestoreDefaults = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(585, 230);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(666, 230);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelMaxControllersPerDLL
            // 
            this.labelMaxControllersPerDLL.AutoSize = true;
            this.labelMaxControllersPerDLL.Location = new System.Drawing.Point(12, 9);
            this.labelMaxControllersPerDLL.Name = "labelMaxControllersPerDLL";
            this.labelMaxControllersPerDLL.Size = new System.Drawing.Size(393, 13);
            this.labelMaxControllersPerDLL.TabIndex = 2;
            this.labelMaxControllersPerDLL.Text = "Maximum number of Critter controllers loaded from a given DLL during competition:" +
    "";
            // 
            // textBoxMaxControllersPerDLL
            // 
            this.textBoxMaxControllersPerDLL.Location = new System.Drawing.Point(15, 25);
            this.textBoxMaxControllersPerDLL.Name = "textBoxMaxControllersPerDLL";
            this.textBoxMaxControllersPerDLL.Size = new System.Drawing.Size(100, 20);
            this.textBoxMaxControllersPerDLL.TabIndex = 3;
            // 
            // labelPathToDLLs
            // 
            this.labelPathToDLLs.AutoSize = true;
            this.labelPathToDLLs.Location = new System.Drawing.Point(12, 58);
            this.labelPathToDLLs.Name = "labelPathToDLLs";
            this.labelPathToDLLs.Size = new System.Drawing.Size(148, 13);
            this.labelPathToDLLs.TabIndex = 4;
            this.labelPathToDLLs.Text = "Path to Critter controller DLLs:";
            // 
            // textBoxPathToCritterControllerDLLs
            // 
            this.textBoxPathToCritterControllerDLLs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPathToCritterControllerDLLs.Location = new System.Drawing.Point(15, 74);
            this.textBoxPathToCritterControllerDLLs.Name = "textBoxPathToCritterControllerDLLs";
            this.textBoxPathToCritterControllerDLLs.Size = new System.Drawing.Size(696, 20);
            this.textBoxPathToCritterControllerDLLs.TabIndex = 5;
            // 
            // buttonLaunchDLLPathPicker
            // 
            this.buttonLaunchDLLPathPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLaunchDLLPathPicker.Location = new System.Drawing.Point(717, 74);
            this.buttonLaunchDLLPathPicker.Name = "buttonLaunchDLLPathPicker";
            this.buttonLaunchDLLPathPicker.Size = new System.Drawing.Size(25, 20);
            this.buttonLaunchDLLPathPicker.TabIndex = 6;
            this.buttonLaunchDLLPathPicker.Text = "?";
            this.buttonLaunchDLLPathPicker.UseVisualStyleBackColor = true;
            this.buttonLaunchDLLPathPicker.Click += new System.EventHandler(this.buttonLaunchDLLPathPicker_Click);
            // 
            // buttonLaunchCritterFilePicker
            // 
            this.buttonLaunchCritterFilePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLaunchCritterFilePicker.Location = new System.Drawing.Point(717, 124);
            this.buttonLaunchCritterFilePicker.Name = "buttonLaunchCritterFilePicker";
            this.buttonLaunchCritterFilePicker.Size = new System.Drawing.Size(25, 20);
            this.buttonLaunchCritterFilePicker.TabIndex = 9;
            this.buttonLaunchCritterFilePicker.Text = "?";
            this.buttonLaunchCritterFilePicker.UseVisualStyleBackColor = true;
            this.buttonLaunchCritterFilePicker.Click += new System.EventHandler(this.buttonLaunchCritterFilePicker_Click);
            // 
            // textBoxPathToFilesCreatedByCritterControllers
            // 
            this.textBoxPathToFilesCreatedByCritterControllers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPathToFilesCreatedByCritterControllers.Location = new System.Drawing.Point(15, 124);
            this.textBoxPathToFilesCreatedByCritterControllers.Name = "textBoxPathToFilesCreatedByCritterControllers";
            this.textBoxPathToFilesCreatedByCritterControllers.Size = new System.Drawing.Size(696, 20);
            this.textBoxPathToFilesCreatedByCritterControllers.TabIndex = 8;
            // 
            // labelPathToControllerFiles
            // 
            this.labelPathToControllerFiles.AutoSize = true;
            this.labelPathToControllerFiles.Location = new System.Drawing.Point(12, 108);
            this.labelPathToControllerFiles.Name = "labelPathToControllerFiles";
            this.labelPathToControllerFiles.Size = new System.Drawing.Size(199, 13);
            this.labelPathToControllerFiles.TabIndex = 7;
            this.labelPathToControllerFiles.Text = "Path to files created by Critter controllers.";
            // 
            // textBoxTerrainDetailFactor
            // 
            this.textBoxTerrainDetailFactor.Location = new System.Drawing.Point(15, 178);
            this.textBoxTerrainDetailFactor.Name = "textBoxTerrainDetailFactor";
            this.textBoxTerrainDetailFactor.Size = new System.Drawing.Size(100, 20);
            this.textBoxTerrainDetailFactor.TabIndex = 11;
            // 
            // labelTerrainDetailFactor
            // 
            this.labelTerrainDetailFactor.AutoSize = true;
            this.labelTerrainDetailFactor.Location = new System.Drawing.Point(12, 162);
            this.labelTerrainDetailFactor.Name = "labelTerrainDetailFactor";
            this.labelTerrainDetailFactor.Size = new System.Drawing.Size(101, 13);
            this.labelTerrainDetailFactor.TabIndex = 10;
            this.labelTerrainDetailFactor.Text = "Terrain detail factor:";
            // 
            // buttonRestoreDefaults
            // 
            this.buttonRestoreDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRestoreDefaults.Location = new System.Drawing.Point(388, 230);
            this.buttonRestoreDefaults.Name = "buttonRestoreDefaults";
            this.buttonRestoreDefaults.Size = new System.Drawing.Size(110, 23);
            this.buttonRestoreDefaults.TabIndex = 12;
            this.buttonRestoreDefaults.Text = "Restore Defaults";
            this.buttonRestoreDefaults.UseVisualStyleBackColor = true;
            this.buttonRestoreDefaults.Click += new System.EventHandler(this.buttonRestoreDefaults_Click);
            // 
            // PropertiesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(746, 260);
            this.Controls.Add(this.buttonRestoreDefaults);
            this.Controls.Add(this.textBoxTerrainDetailFactor);
            this.Controls.Add(this.labelTerrainDetailFactor);
            this.Controls.Add(this.buttonLaunchCritterFilePicker);
            this.Controls.Add(this.textBoxPathToFilesCreatedByCritterControllers);
            this.Controls.Add(this.labelPathToControllerFiles);
            this.Controls.Add(this.buttonLaunchDLLPathPicker);
            this.Controls.Add(this.textBoxPathToCritterControllerDLLs);
            this.Controls.Add(this.labelPathToDLLs);
            this.Controls.Add(this.textBoxMaxControllersPerDLL);
            this.Controls.Add(this.labelMaxControllersPerDLL);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PropertiesDialog";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelMaxControllersPerDLL;
        private System.Windows.Forms.TextBox textBoxMaxControllersPerDLL;
        private System.Windows.Forms.Label labelPathToDLLs;
        private System.Windows.Forms.TextBox textBoxPathToCritterControllerDLLs;
        private System.Windows.Forms.Button buttonLaunchDLLPathPicker;
        private System.Windows.Forms.Button buttonLaunchCritterFilePicker;
        private System.Windows.Forms.TextBox textBoxPathToFilesCreatedByCritterControllers;
        private System.Windows.Forms.Label labelPathToControllerFiles;
        private System.Windows.Forms.TextBox textBoxTerrainDetailFactor;
        private System.Windows.Forms.Label labelTerrainDetailFactor;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogCritterControllerDLL;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogCritterCreatedFiles;
        private System.Windows.Forms.Button buttonRestoreDefaults;
    }
}