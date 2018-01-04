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
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MathQuiz.MathQuizForm", typeof(MathQuizForm).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        public static System.IO.UnmanagedMemoryStream beep
        {
            get
            {
                return ResourceManager.GetStream("beep", resourceCulture);
            }
        }

        public static System.Drawing.Point mathQuizTimer_TrayLocation
        {
            get
            {
                object obj = ResourceManager.GetObject("mathQuizTimer.TrayLocation", resourceCulture);
                return ((System.Drawing.Point)(obj));
            }
        }

        public static string TestResx
        {
            get
            {
                return ResourceManager.GetString("TestResx", resourceCulture);
            }
        }

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
            if (timeLeft <= 5 && timeLeft >= 0)
            {
                timeLabel.BackColor = Color.Red;
            }
            else
            {
                timeLabel.BackColor = Color.White;
            }
            timeLabel.Text = timeLeft + " seconds";
        }

        private bool IsSumAnswerCorrect()
        {
            return sumNumericUpDown.Value == addend1 + addend2;
        }

        private bool IsDifferenceAnswerCorrect()
        {
            return differenceNumericUpDown.Value == minuend - subtrahend;
        }

        private bool IsProductAnswerCorrect()
        {
            return productNumericUpDown.Value == multiplicand * multiplier;
        }

        private bool IsQuotientAnswerCorrect()
        {
            return quotientNumericUpDown.Value == dividend / divisor;
        }

        private bool CheckTheAnswer()
        {
            return IsSumAnswerCorrect()
                && IsDifferenceAnswerCorrect()
                && IsProductAnswerCorrect()
                && IsQuotientAnswerCorrect();
        }

        private void TestComplete()
        {
            mathQuizTimer.Stop();
            timeLabel.BackColor = Color.White;
            startButton.Enabled = true;
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
                // Timer stop must place here. Otherwise it will keep running.
                TestComplete();

                MessageBox.Show("You are bang bang!", "Success");
            }
            else if (timeLeft > 0)
            {
                timeLeft--;
                ShowTimeLeft();
            }
            else
            {
                TestComplete();

                timeLabel.Text = "Time is up";
                MessageBox.Show("You are too slow!", "Fail");
                
                sumNumericUpDown.Value = addend1 + addend2;
                differenceNumericUpDown.Value = minuend - subtrahend;
                productNumericUpDown.Value = multiplier * multiplicand;
                quotientNumericUpDown.Value = dividend / divisor;
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

        private void play_Sound_If_Correct(object sender, EventArgs e)
        {
            NumericUpDown answerBox = sender as NumericUpDown;

            if (answerBox == null)
                return;

            if ((answerBox == sumNumericUpDown && IsSumAnswerCorrect()) ||
                (answerBox == differenceNumericUpDown && IsDifferenceAnswerCorrect()) ||
                (answerBox == productNumericUpDown && IsProductAnswerCorrect()) ||
                (answerBox == quotientNumericUpDown && IsQuotientAnswerCorrect()))
            {
                PlaySound();
            }
        }

        private void PlaySound()
        {
            /*
            string rootLocation = typeof(Program).Assembly.Location;
            string fullPathToSound = System.IO.Path.Combine(rootLocation, @"Data\Sounds\beep.wav");
            */

            //string fullPathToSound = @"beep.wav";
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(beep);
            player.Play();
        }
    }
}
