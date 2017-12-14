using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class MatchingGameForm : Form
    {
        Random random = new Random();

        List<string> icons = new List<string> {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };

        Label firstClicked = null;

        Label secondClicked = null;

        private void AssignIconsToSquares()
        {
            foreach (Control control in matchingGameTableLayoutPanel.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(0, icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void CheckForWinner()
        {
            foreach (Control control in matchingGameTableLayoutPanel.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            MessageBox.Show("You matched all the icons!", "Congratulations");
            Close();
        }

        public MatchingGameForm()
        {
            InitializeComponent();

            AssignIconsToSquares();
        }

        private void label_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;

            if (clickedLabel == null ||
                clickedLabel.ForeColor == Color.Black ||
                matchingGameTimer.Enabled)
            {
                return;
            }

            clickedLabel.ForeColor = Color.Black;
            if (firstClicked == null)
            {
                firstClicked = clickedLabel;
            }
            else if (clickedLabel.Text == firstClicked.Text)
            {
                CheckForWinner();
                firstClicked = null;
            }
            else
            {
                secondClicked = clickedLabel;
                matchingGameTimer.Start();
            }

        }

        private void matchingGameTimer_Tick(object sender, EventArgs e)
        {
            matchingGameTimer.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }
    }
}
