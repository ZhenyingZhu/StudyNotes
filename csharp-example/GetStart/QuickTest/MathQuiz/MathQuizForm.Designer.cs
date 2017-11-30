namespace MathQuiz
{
    partial class MathQuizForm
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
            this.timeLabel = new System.Windows.Forms.Label();
            this.timeLeftLabel = new System.Windows.Forms.Label();
            this.plusLeftLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timeLabel
            // 
            this.timeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.timeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.timeLabel.Location = new System.Drawing.Point(268, 10);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(200, 32);
            this.timeLabel.TabIndex = 0;
            // 
            // timeLeftLabel
            // 
            this.timeLeftLabel.AutoSize = true;
            this.timeLeftLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.timeLeftLabel.Location = new System.Drawing.Point(161, 15);
            this.timeLeftLabel.Name = "timeLeftLabel";
            this.timeLeftLabel.Size = new System.Drawing.Size(101, 25);
            this.timeLeftLabel.TabIndex = 1;
            this.timeLeftLabel.Text = "Time Left";
            // 
            // plusLeftLabel
            // 
            this.plusLeftLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plusLeftLabel.Location = new System.Drawing.Point(50, 75);
            this.plusLeftLabel.Name = "plusLeftLabel";
            this.plusLeftLabel.Size = new System.Drawing.Size(60, 50);
            this.plusLeftLabel.TabIndex = 2;
            this.plusLeftLabel.Text = "?";
            this.plusLeftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(202, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 50);
            this.label1.TabIndex = 3;
            this.label1.Text = "?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(116, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 50);
            this.label2.TabIndex = 4;
            this.label2.Text = "?";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MathQuizForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 280);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.plusLeftLabel);
            this.Controls.Add(this.timeLeftLabel);
            this.Controls.Add(this.timeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "MathQuizForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Math Quiz";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label timeLeftLabel;
        private System.Windows.Forms.Label plusLeftLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

