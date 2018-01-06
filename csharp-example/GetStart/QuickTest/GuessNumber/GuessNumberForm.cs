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
        private bool started;

        private int guessCount;

        private int[] answer;

        public GuessNumberForm()
        {
            InitializeComponent();
        }

        private void GenerateAnswer()
        {
            answer = new int[4];

            Random randomizer = new Random();
            int idx = 0;
            while (idx < 4)
            {
                int num = randomizer.Next(10);
                if (!answer.Contains(num))
                {
                    answer[idx++] = num;
                }
            }
        }

        private bool IsCorrectAnswer(int[] userAnswer)
        {
            for (int i = 0; i < userAnswer.Length; i++)
            {
                if (answer[i] != userAnswer[i])
                    return false;
            }
            return true;
        }

        private string GetHintString(int[] userAnswer)
        {
            string hint = string.Join(",", userAnswer);
            hint += " ";

            int correctNumber = 0;
            foreach (int num in answer)
            {
                if (userAnswer.Contains(num))
                    correctNumber++;
            }

            int correctPlaced = 0;
            for (int i = 0; i < answer.Length; i++)
            {
                if (answer[i] == userAnswer[i])
                    correctPlaced++;
            }

            hint += correctPlaced.ToString() + "A" + (correctNumber - correctPlaced).ToString() + "B";

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

            if (IsCorrectAnswer(userAnswer))
            {
                MessageBox.Show("Bingo!", "Success");

                numberTableLayoutPanel.Controls.Clear();
                started = false;
            }
            else
            {
                Label label = new Label();
                label.Text = GetHintString(userAnswer);
                label.Font = new Font("Calibri", 18, FontStyle.Bold);
                label.AutoSize = true;
                numberTableLayoutPanel.Controls.Add(label);
            }

            numberTextBox1.Text = string.Empty;
            numberTextBox2.Text = string.Empty;
            numberTextBox3.Text = string.Empty;
            numberTextBox4.Text = string.Empty;
            numberTextBox1.Focus();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            if (!started)
            {
                GenerateAnswer();
                started = true;
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
                if (textBox.Text != string.Empty)
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
