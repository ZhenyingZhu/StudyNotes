using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureViewer
{
    public partial class PictureViewerForm : Form
    {
        public PictureViewerForm()
        {
            InitializeComponent();
            // TODO: probably should not do so.
            InitFlowLayoutPanelCheckBoxes();
        }

        /// <summary>
        /// Show the Open File dialog. If the user clicks OK, load the picture that the user chose.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showButton_Click(object sender, EventArgs e)
        {
            if (openPictureDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Load(openPictureDialog.FileName);
            }
        }

        /// <summary>
        /// Show the color dialog box. If the user clicks OK, change the PictureBox control's background
        /// to the color the user chose.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundButton_Click(object sender, EventArgs e)
        {
            if (backgroundColorDialog.ShowDialog() == DialogResult.OK)
                pictureBox.BackColor = backgroundColorDialog.Color;
        }

        /// <summary>
        /// Clear the picture.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = null;
        }

        /// <summary>
        /// Close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// If the user selects the Stretch check box, change the PictureBox's SizeMode property to "Stretch".
        /// If the user clears the check box, change it to "Normal".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stretchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (stretchCheckBox.Checked)
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            else
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        // Dynamically add some boxes.
        private void InitFlowLayoutPanelCheckBoxes()
        {
            List<string> list = new List<string>{"a", "b", "c"};

            CheckBox box;
            foreach (var name in list)
            {
                box = new CheckBox
                {
                    Tag = name,
                    Text = name
                };
                //box.Location = new Point(10, 50);
                this.flowLayoutPanelCheckBoxes.Controls.Add(box);
            }
        }
    }
}
