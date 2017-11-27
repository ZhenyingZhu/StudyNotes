namespace PictureViewer
{
    partial class PictureViewerForm
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
            this.pictureViewerTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.stretchCheckBox = new System.Windows.Forms.CheckBox();
            this.pictureControlFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.showButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.backgroundButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.openPictureDialog = new System.Windows.Forms.OpenFileDialog();
            this.backgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.flowLayoutPanelCheckBoxes = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureViewerTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.pictureControlFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureViewerTableLayoutPanel
            // 
            this.pictureViewerTableLayoutPanel.ColumnCount = 2;
            this.pictureViewerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.pictureViewerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.pictureViewerTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.pictureViewerTableLayoutPanel.Controls.Add(this.pictureBox, 0, 0);
            this.pictureViewerTableLayoutPanel.Controls.Add(this.stretchCheckBox, 0, 1);
            this.pictureViewerTableLayoutPanel.Controls.Add(this.pictureControlFlowLayoutPanel, 1, 1);
            this.pictureViewerTableLayoutPanel.Controls.Add(this.flowLayoutPanelCheckBoxes, 1, 2);
            this.pictureViewerTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureViewerTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.pictureViewerTableLayoutPanel.Name = "pictureViewerTableLayoutPanel";
            this.pictureViewerTableLayoutPanel.RowCount = 3;
            this.pictureViewerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.72681F));
            this.pictureViewerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.273183F));
            this.pictureViewerTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.pictureViewerTableLayoutPanel.Size = new System.Drawing.Size(558, 477);
            this.pictureViewerTableLayoutPanel.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureViewerTableLayoutPanel.SetColumnSpan(this.pictureBox, 2);
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(552, 356);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // stretchCheckBox
            // 
            this.stretchCheckBox.AutoSize = true;
            this.stretchCheckBox.Location = new System.Drawing.Point(3, 365);
            this.stretchCheckBox.Name = "stretchCheckBox";
            this.stretchCheckBox.Size = new System.Drawing.Size(60, 17);
            this.stretchCheckBox.TabIndex = 1;
            this.stretchCheckBox.Text = "Stretch";
            this.stretchCheckBox.UseVisualStyleBackColor = true;
            this.stretchCheckBox.CheckedChanged += new System.EventHandler(this.stretchCheckBox_CheckedChanged);
            // 
            // pictureControlFlowLayoutPanel
            // 
            this.pictureControlFlowLayoutPanel.Controls.Add(this.showButton);
            this.pictureControlFlowLayoutPanel.Controls.Add(this.clearButton);
            this.pictureControlFlowLayoutPanel.Controls.Add(this.backgroundButton);
            this.pictureControlFlowLayoutPanel.Controls.Add(this.closeButton);
            this.pictureControlFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureControlFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pictureControlFlowLayoutPanel.Location = new System.Drawing.Point(86, 365);
            this.pictureControlFlowLayoutPanel.Name = "pictureControlFlowLayoutPanel";
            this.pictureControlFlowLayoutPanel.Size = new System.Drawing.Size(469, 31);
            this.pictureControlFlowLayoutPanel.TabIndex = 2;
            // 
            // showButton
            // 
            this.showButton.AutoSize = true;
            this.showButton.Location = new System.Drawing.Point(367, 3);
            this.showButton.Name = "showButton";
            this.showButton.Size = new System.Drawing.Size(99, 25);
            this.showButton.TabIndex = 0;
            this.showButton.Text = "Show a picture";
            this.showButton.UseVisualStyleBackColor = true;
            this.showButton.Click += new System.EventHandler(this.showButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.AutoSize = true;
            this.clearButton.Location = new System.Drawing.Point(244, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(117, 25);
            this.clearButton.TabIndex = 1;
            this.clearButton.Text = "Clear the picture";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // backgroundButton
            // 
            this.backgroundButton.AutoSize = true;
            this.backgroundButton.Location = new System.Drawing.Point(85, 3);
            this.backgroundButton.Name = "backgroundButton";
            this.backgroundButton.Size = new System.Drawing.Size(153, 25);
            this.backgroundButton.TabIndex = 2;
            this.backgroundButton.Text = "Set the backgroud color";
            this.backgroundButton.UseVisualStyleBackColor = true;
            this.backgroundButton.Click += new System.EventHandler(this.backgroundButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.AutoSize = true;
            this.closeButton.Location = new System.Drawing.Point(4, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 25);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // openPictureDialog
            // 
            this.openPictureDialog.InitialDirectory = "E:\\Pictures\\动物";
            // 
            // flowLayoutPanelCheckBoxes
            // 
            this.flowLayoutPanelCheckBoxes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelCheckBoxes.Location = new System.Drawing.Point(86, 402);
            this.flowLayoutPanelCheckBoxes.Name = "flowLayoutPanelCheckBoxes";
            this.flowLayoutPanelCheckBoxes.Size = new System.Drawing.Size(469, 72);
            this.flowLayoutPanelCheckBoxes.TabIndex = 3;
            // 
            // PictureViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 477);
            this.Controls.Add(this.pictureViewerTableLayoutPanel);
            this.Name = "PictureViewerForm";
            this.Text = "Picture Viewer";
            this.pictureViewerTableLayoutPanel.ResumeLayout(false);
            this.pictureViewerTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.pictureControlFlowLayoutPanel.ResumeLayout(false);
            this.pictureControlFlowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel pictureViewerTableLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.CheckBox stretchCheckBox;
        private System.Windows.Forms.FlowLayoutPanel pictureControlFlowLayoutPanel;
        private System.Windows.Forms.Button showButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button backgroundButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.OpenFileDialog openPictureDialog;
        private System.Windows.Forms.ColorDialog backgroundColorDialog;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCheckBoxes;
    }
}

