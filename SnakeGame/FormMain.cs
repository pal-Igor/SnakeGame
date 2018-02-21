using SnakeGame.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class FormMain : Form
    {
        int maxXPos;
        int maxYPos;
        Snake Snake1;
        private Point Food;
        private Random random;
        public FormMain()
        {
            InitializeComponent();

            Snake1 = new Snake();
            Food = new Point(1, 2);
            random = new Random();

            //Get maximum X and Y Pos
            maxXPos = panField.Size.Width / Settings.Width;
            maxYPos = panField.Size.Height / Settings.Height;

            //Set speed and timer
            gameTimer.Interval = 500 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            //trackBar1.Value = Settings.Speed;
            StartGame();
        }
        private void StartGame()
        {
            replay.Visible = false;
            label2.Visible = false;
            lblGameOver.Visible = false;
            trackBar1.Enabled = false;

            //Set settings to default
            Settings.SetToDefaultSettings();
            Settings.Speed = trackBar1.Value;
            gameTimer.Interval = 500 / Settings.Speed;

            //Create new player object
            Snake1 = new Snake();

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void GenerateFood()
        {
            bool poinWasGenerated = false;
            do
            {
                var newFood = new Point(random.Next(0, maxXPos), random.Next(0, maxYPos));
                if (!Snake1.SnakeContainsPoint(newFood))
                {
                    Food = newFood;
                    poinWasGenerated = true;
                }

            } while (!poinWasGenerated);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            //Check for Game Over
            if (!Snake1.IsAlive)
            {
                //Check if Enter is pressed
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                Snake1.SetDirection();

                //Detect collission with game borders and snake tail.
                if (Snake1.IsHitBorder(maxXPos, maxYPos) || Snake1.IsHitTail())
                {
                    Snake1.Die();
                }

                //Detect collision with food piece
                if (Snake1.Eat(Food))
                {
                    lblScore.Text = Settings.Score.ToString();
                    GenerateFood();
                }

                Snake1.Move();
            }

            panField.Refresh();
        }

        private void panField_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Graphics field = e.Graphics;

            if (Snake1.IsAlive)
            {
                Snake1.DrawSnake(field);

                //Draw Food
                field.FillRectangle(Brushes.Orange, new Rectangle(Food.X * Settings.Width, Food.Y * Settings.Height,
                    Settings.Width, Settings.Height));
                //Image newImage = Resources.peach;
                //field.DrawImage(newImage, Food.X * Settings.Width, Food.Y * Settings.Height,
                //    Settings.Width, Settings.Height);
            }
            else
            {
                string s1 = "Игра окончена\nВаш финальный счет: ";
                lblGameOver.Text = s1;
                label2.Text = Settings.Score.ToString();

                replay.Focus();
                trackBar1.Enabled = true;
                label2.Visible = true;
                replay.Visible = true;
                lblGameOver.Visible = true;
            }
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Control)
                    replay.PerformClick();
            }
            Input.ChangeState(e.KeyCode, false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString();
        }
    }
}