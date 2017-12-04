using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathQuiz
{
    public partial class MathQuizForm : Form
    {
        Random randomizer = new Random();

        int addend1;
        int addend2;

        int timeLeft;

        private void StartTheQuiz()
        {
            addend1 = randomizer.Next(51);
            addend2 = randomizer.Next(51);

            plusLeftLabel.Text = addend1.ToString();
            plusRightLabel.Text = addend2.ToString();

            sumNumericUpDown.Value = 0;
        }

        private void ShowTimeLeft()
        {
            timeLabel.Text = timeLeft + " seconds";
        }

        private bool CheckTheAnswer()
        {
            return sumNumericUpDown.Value == addend1 + addend2;
        }

        public MathQuizForm()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartTheQuiz();
            startButton.Enabled = false;

            timeLeft = 5;
            ShowTimeLeft();
            mathQuizTimer.Start();
        }

        private void mathQuizTimer_Tick(object sender, EventArgs e)
        {
            if (CheckTheAnswer())
            {
                mathQuizTimer.Stop();
                startButton.Enabled = true;
                MessageBox.Show("You are bang bang!", "Success");
            }
            else if (timeLeft > 0)
            {
                timeLeft--;
                ShowTimeLeft();
            }
            else
            {
                mathQuizTimer.Stop();
                timeLabel.Text = "Time is up";
                MessageBox.Show("You are too slow!", "Fail");
                sumNumericUpDown.Value = addend1 + addend2;
                startButton.Enabled = true;
            }
        }
    }
}
