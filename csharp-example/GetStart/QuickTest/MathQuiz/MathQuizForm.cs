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

        int minuend;
        int subtrahend;

        int multiplicand;
        int multiplier;

        int dividend;
        int divisor;

        int timeLeft;

        private void StartTheQuiz()
        {
            addend1 = randomizer.Next(51);
            addend2 = randomizer.Next(51);
            plusLeftLabel.Text = addend1.ToString();
            plusRightLabel.Text = addend2.ToString();
            sumNumericUpDown.Value = 0;

            minuend = randomizer.Next(1, 101);
            subtrahend = randomizer.Next(1, minuend);
            minusLeftLabel.Text = minuend.ToString();
            minusRightLabel.Text = subtrahend.ToString();
            differenceNumericUpDown.Value = 0;

            multiplicand = randomizer.Next(2, 11);
            multiplier = randomizer.Next(2, 11);
            multipleLeftLabel.Text = multiplicand.ToString();
            multipleRightLabel.Text = multiplier.ToString();
            productNumericUpDown.Value = 0;


            divisor = randomizer.Next(2, 11);
            int temporaryQuotient = randomizer.Next(2, 11);
            dividend = divisor * temporaryQuotient;
            divideLeftLabel.Text = dividend.ToString();
            divideRightLabel.Text = divisor.ToString();
            quotientNumericUpDown.Value = 0;
        }

        private void ShowTimeLeft()
        {
            timeLabel.Text = timeLeft + " seconds";
        }

        private bool CheckTheAnswer()
        {
            return sumNumericUpDown.Value == addend1 + addend2
                && differenceNumericUpDown.Value == minuend - subtrahend
                && productNumericUpDown.Value == multiplicand * multiplier
                && quotientNumericUpDown.Value == dividend / divisor;
        }

        public MathQuizForm()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartTheQuiz();
            startButton.Enabled = false;

            timeLeft = 20;
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
                differenceNumericUpDown.Value = minuend - subtrahend;
                productNumericUpDown.Value = multiplier * multiplicand;
                quotientNumericUpDown.Value = dividend / divisor;

                startButton.Enabled = true;
            }
        }

        private void answer_Enter(object sender, EventArgs e)
        {
            NumericUpDown answerBox = sender as NumericUpDown;

            if (answerBox != null)
            {
                int lenOfAns = answerBox.Value.ToString().Length;
                answerBox.Select(0, lenOfAns);
            }
        }
    }
}
