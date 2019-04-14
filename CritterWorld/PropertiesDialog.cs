using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CritterWorld
{
    public partial class PropertiesDialog : Form
    {
        static PropertiesDialog propertiesDialog = null;

        public static void Launch()
        {
            if (propertiesDialog == null)
            {
                propertiesDialog = new PropertiesDialog();
            }
            propertiesDialog.ShowDialog();
        }

        private void LoadControls()
        {
            trackBarMaxCrittersLoadedPerDLL.Value = PropertiesManager.Properties.CompetitionControllerLoadMaximum;
            textBoxPathToCritterControllerDLLs.Text = PropertiesManager.Properties.CritterControllerDLLPath;
            textBoxPathToFilesCreatedByCritterControllers.Text = PropertiesManager.Properties.CritterControllerFilesPath;
            trackBarTerrainDetailFactor.Value = PropertiesManager.Properties.TerrainDetailFactor;
            labelMaxCrittersLoadedPerDLLValue.Text = trackBarMaxCrittersLoadedPerDLL.Value.ToString();
            labelTerrainDetailFactorValue.Text = trackBarTerrainDetailFactor.Value.ToString();
        }

        public PropertiesDialog()
        {
            InitializeComponent();
            LoadControls();
        }

        private void buttonLaunchDLLPathPicker_Click(object sender, EventArgs e)
        {
            folderBrowserDialogCritterControllerDLL.SelectedPath = textBoxPathToCritterControllerDLLs.Text;
            if (folderBrowserDialogCritterControllerDLL.ShowDialog() == DialogResult.OK)
            {
                textBoxPathToCritterControllerDLLs.Text = folderBrowserDialogCritterControllerDLL.SelectedPath;
            }
        }

        private void buttonLaunchCritterFilePicker_Click(object sender, EventArgs e)
        {
            folderBrowserDialogCritterCreatedFiles.SelectedPath = textBoxPathToFilesCreatedByCritterControllers.Text;
            if (folderBrowserDialogCritterCreatedFiles.ShowDialog() == DialogResult.OK)
            {
                textBoxPathToFilesCreatedByCritterControllers.Text = folderBrowserDialogCritterCreatedFiles.SelectedPath;
            }
        }

        private void MessageInvalidInput(String prompt)
        {
            MessageBox.Show(this, "'" + prompt + "' is invalid.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            PropertiesManager.Properties.CompetitionControllerLoadMaximum = trackBarMaxCrittersLoadedPerDLL.Value;
            PropertiesManager.Properties.CritterControllerDLLPath = textBoxPathToCritterControllerDLLs.Text.Trim();
            PropertiesManager.Properties.CritterControllerFilesPath = textBoxPathToFilesCreatedByCritterControllers.Text.Trim();
            PropertiesManager.Properties.TerrainDetailFactor = trackBarTerrainDetailFactor.Value;
            Hide();
            PropertiesManager.Save();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void buttonRestoreDefaults_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you wish to lose your current settings and restore defaults?", 
                "Restore Defaults", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                PropertiesManager.RestoreDefaults();
                LoadControls();
            }
        }

        private void trackBarMaxCrittersLoadedPerDLL_ValueChange(object sender, EventArgs e)
        {
            labelMaxCrittersLoadedPerDLLValue.Text = trackBarMaxCrittersLoadedPerDLL.Value.ToString();
        }

        private void trackBarTerrainDetailFactor_ValueChange(object sender, EventArgs e)
        {
            labelTerrainDetailFactorValue.Text = trackBarTerrainDetailFactor.Value.ToString();
        }
    }
}
