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

            Snake1 = new Snake(1, 2);
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
            Snake1.Clear();
            Snake1.CreateHead();

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void GenerateFood()
        {
            for (int i = 0; i < Snake1.MasSnake.Count; i++)
            {
                if (Food.X != Snake1[i].X && Food.Y != Snake1[i].Y)
                {
                    continue;
                }
                Food = new Point(random.Next(0, maxXPos), random.Next(0, maxYPos));
            }
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            //Check for Game Over
            if (Settings.GameOver)
            {
                //Check if Enter is pressed
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.Direction != Direction.Left)
                {
                    Settings.Direction = Direction.Right;
                }
                else if (Input.KeyPressed(Keys.Left) && Settings.Direction != Direction.Right)
                {
                    Settings.Direction = Direction.Left;
                }
                else if (Input.KeyPressed(Keys.Up) && Settings.Direction != Direction.Down)
                {
                    Settings.Direction = Direction.Up;
                }
                else if (Input.KeyPressed(Keys.Down) && Settings.Direction != Direction.Up)
                {
                    Settings.Direction = Direction.Down;
                }

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
            Graphics field = e.Graphics;

            if (!Settings.GameOver)
            {
                int r = 255;
                int g = 70;
                int b = 0;

                //Draw snake
                for (int i = 0; i < Snake1.MasSnake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                    {
                        snakeColour = Brushes.OrangeRed;     //Draw head
                    }
                    else
                    {
                        Color myColor = Color.FromArgb(r, g, b);
                        snakeColour = new SolidBrush(myColor);    //Rest of body
                    }

                    //Draw snake
                    field.FillRectangle(snakeColour, new Rectangle(Snake1[i].X * Settings.Width,
                         Snake1[i].Y * Settings.Height, Settings.Width, Settings.Height));

                    //Draw Food
                    field.FillRectangle(Brushes.Orange, new Rectangle(Food.X * Settings.Width,
                        Food.Y * Settings.Height, Settings.Width, Settings.Height));

                    if (g > 0)
                    {
                        g -= 7;
                    }
                    else
                    {
                        g += 7;
                    }

                    r -= 2;
                    b += 3;
                }
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