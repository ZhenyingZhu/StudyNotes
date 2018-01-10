using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuessNumber
{
    public partial class GuessNumberForm : Form
    {
        private bool _started;

        private int _guessCount;

        private int[] _answer;

        public GuessNumberForm()
        {
            InitializeComponent();
        }

        private void GenerateAnswer()
        {
            _answer = new int[4];

            Random randomizer = new Random();
            int idx = 0;
            while (idx < 4)
            {
                int num = randomizer.Next(10);
                if (!_answer.Contains(num))
                {
                    _answer[idx++] = num;
                }
            }
        }

        private bool IsCorrectAnswer(int[] userAnswer)
        {
            return !userAnswer.Where((t, i) => _answer[i] != t).Any();
        }

        private string GetHintString(int[] userAnswer)
        {
            string hint = string.Join(",", userAnswer);
            hint += " ";

            int correctNumber = _answer.Count(userAnswer.Contains);

            int correctPlaced = _answer.Where((t, i) => t == userAnswer[i]).Count();

            hint += correctPlaced + "A" + (correctNumber - correctPlaced) + "B";

            return hint;
        }

        private void CheckAnswer()
        {
            int[] userAnswer = new int[4];
            try
            {
                userAnswer[0] = int.Parse(numberTextBox1.Text);
                userAnswer[1] = int.Parse(numberTextBox2.Text);
                userAnswer[2] = int.Parse(numberTextBox3.Text);
                userAnswer[3] = int.Parse(numberTextBox4.Text);
            }
            catch(Exception ex)
            {
                return;
            }

            _guessCount++;
            if (IsCorrectAnswer(userAnswer))
            {
                MessageBox.Show("Bingo!", "Success"); // should use localization string.

                numberTableLayoutPanel.Controls.Clear();
                _started = false;
            }
            else
            {
                Label guessCountLabel = new Label
                {
                    Text = _guessCount.ToString(),
                    Font = new Font("Calibri", 15),
                    AutoSize = true
                };
                numberTableLayoutPanel.Controls.Add(guessCountLabel, 0, _guessCount - 1);

                Label hintLabel = new Label
                {
                    Text = GetHintString(userAnswer),
                    Font = new Font("Calibri", 18, FontStyle.Bold),
                    AutoSize = true
                };
                numberTableLayoutPanel.Controls.Add(hintLabel, 1, _guessCount - 1);
            }

            if (_guessCount == 10) // should move it to app.config.
            {
                MessageBox.Show("You lose!", "Fail");

                numberTableLayoutPanel.Controls.Clear();
                _started = false;
            }

            numberTextBox1.Text = string.Empty;
            numberTextBox2.Text = string.Empty;
            numberTextBox3.Text = string.Empty;
            numberTextBox4.Text = string.Empty;
            numberTextBox1.Focus();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            if (!_started)
            {
                GenerateAnswer();
                _guessCount = 0;
                _started = true;
            }

            CheckAnswer();
        }

        private void numberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                SendKeys.Send("{tab}");
            }
            else if (e.KeyChar == '\b')
            {
                TextBox textBox = sender as TextBox;
                if (textBox == null || textBox.Text != string.Empty)
                {
                    return;
                }

                if (textBox == numberTextBox2)
                {
                    numberTextBox1.Focus();
                }
                else if (textBox == numberTextBox3)
                {
                    numberTextBox2.Focus();
                }
                else if (textBox == numberTextBox4)
                {
                    numberTextBox3.Focus();
                }
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
