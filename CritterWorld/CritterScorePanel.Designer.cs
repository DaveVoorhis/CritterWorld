namespace CritterWorld
{
    partial class CritterScorePanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanelOuter = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelInnerTop = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelInnerBottom = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelOuter
            // 
            this.flowLayoutPanelOuter.Controls.Add(this.flowLayoutPanelInnerTop);
            this.flowLayoutPanelOuter.Controls.Add(this.flowLayoutPanelInnerBottom);
            this.flowLayoutPanelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelOuter.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelOuter.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelOuter.Name = "flowLayoutPanelOuter";
            this.flowLayoutPanelOuter.Size = new System.Drawing.Size(620, 265);
            this.flowLayoutPanelOuter.TabIndex = 0;
            // 
            // flowLayoutPanelInnerTop
            // 
            this.flowLayoutPanelInnerTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelInnerTop.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelInnerTop.Name = "flowLayoutPanelInnerTop";
            this.flowLayoutPanelInnerTop.Size = new System.Drawing.Size(0, 100);
            this.flowLayoutPanelInnerTop.TabIndex = 0;
            // 
            // flowLayoutPanelInnerBottom
            // 
            this.flowLayoutPanelInnerBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelInnerBottom.Location = new System.Drawing.Point(3, 109);
            this.flowLayoutPanelInnerBottom.Name = "flowLayoutPanelInnerBottom";
            this.flowLayoutPanelInnerBottom.Size = new System.Drawing.Size(0, 100);
            this.flowLayoutPanelInnerBottom.TabIndex = 1;
            // 
            // CritterScorePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanelOuter);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "CritterScorePanel";
            this.Size = new System.Drawing.Size(620, 265);
            this.flowLayoutPanelOuter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelOuter;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelInnerTop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelInnerBottom;
    }
}
