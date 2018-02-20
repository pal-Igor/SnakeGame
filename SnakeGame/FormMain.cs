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
        Snake Snake1;
        private Point Food;
        private Random random;
        public FormMain()
        {
            InitializeComponent();

            Snake1 = new Snake(1, 2);
            Food = new Point(1, 2);
            random = new Random();

            //Установка скорости и таймера
            gameTimer.Interval = 500 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            trackBar1.Value = Settings.Speed;

            StartGame();
        }
        private void StartGame()
        {
            button1.Visible = false;
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
            int maxXPos = panField.Size.Width / Settings.Width;
            int maxYPos = panField.Size.Height / Settings.Height;
            
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

                MovePlayer();
            }

            panField.Invalidate();
        }

        private void MovePlayer()
        {
            for (int i = Snake1.MasSnake.Count - 1; i >= 0; i--)
            {
                //Move head
                if (i == 0)
                {
                    switch (Settings.Direction)
                    {
                        case Direction.Right:
                            Snake1[i].X++;
                            break;
                        case Direction.Left:
                            Snake1[i].X--;
                            break;
                        case Direction.Up:
                            Snake1[i].Y--;
                            break;
                        case Direction.Down:
                            Snake1[i].Y++;
                            break;
                    }

                    //Get maximum X and Y Pos
                    int maxXPos = panField.Size.Width / Settings.Width;
                    int maxYPos = panField.Size.Height / Settings.Height;

                    //Detect collission with game borders.
                    if (Snake1[i].X < 0 || Snake1[i].Y < 0
                        || Snake1[i].X >= maxXPos || Snake1[i].Y >= maxYPos)
                    {
                        Snake1.Die();
                    }

                    //Detect collission with body
                    for (int j = 1; j < Snake1.MasSnake.Count; j++)
                    {
                        if (Snake1[i].X == Snake1[j].X &&
                           Snake1[i].Y == Snake1[j].Y)
                        {
                            Snake1.Die();
                        }
                    }

                    //Detect collision with food piece
                    if (Snake1[0].X == Food.X && Snake1[0].Y == Food.Y)
                    {
                        Snake1.Eat();
                        lblScore.Text = Settings.Score.ToString();

                        GenerateFood();
                    }
                }
                else
                {
                    //Move body
                    Snake1[i].X = Snake1[i -1].X;
                    Snake1[i].Y = Snake1[i -1].Y;
                }
            }
        }

        private void panField_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = panField.CreateGraphics();

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
                    canvas.FillRectangle(snakeColour, new Rectangle(Snake1[i].X * Settings.Width,
                         Snake1[i].Y * Settings.Height, Settings.Width, Settings.Height));

                    //Draw Food
                    canvas.FillRectangle(Brushes.Orange, new Rectangle(Food.X * Settings.Width,
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

                button1.Focus();
                trackBar1.Enabled = true;
                label2.Visible = true;
                button1.Visible = true;
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
                    button1.PerformClick();
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