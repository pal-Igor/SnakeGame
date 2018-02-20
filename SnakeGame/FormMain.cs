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
        Snake Snake1 = new Snake(1,2);
        private Point Food;
        private Random random;
        public FormMain()
        {
            InitializeComponent();

            
            Food = new Point(1, 2);
            random = new Random();

            //Установка скорости и таймера
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();
        }
        private void StartGame()
        {
            label2.Visible = false;
            label3.Visible = false;
            lblGameOver.Visible = false;

            //Set settings to default
            Settings.SetToDefaultSettings();

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
                if (Food.X != Snake1.MasSnake[i].X && Food.Y != Snake1.MasSnake[i].Y)
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
                            Snake1.MasSnake[i].X++;
                            break;
                        case Direction.Left:
                            Snake1.MasSnake[i].X--;
                            break;
                        case Direction.Up:
                            Snake1.MasSnake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake1.MasSnake[i].Y++;
                            break;
                    }

                    //Get maximum X and Y Pos
                    int maxXPos = panField.Size.Width / Settings.Width;
                    int maxYPos = panField.Size.Height / Settings.Height;

                    //Detect collission with game borders.
                    if (Snake1.MasSnake[i].X < 0 || Snake1.MasSnake[i].Y < 0
                        || Snake1.MasSnake[i].X >= maxXPos || Snake1.MasSnake[i].Y >= maxYPos)
                    {
                        Snake1.Die();
                    }

                    //Detect collission with body
                    for (int j = 1; j < Snake1.MasSnake.Count; j++)
                    {
                        if (Snake1.MasSnake[i].X == Snake1.MasSnake[i].X &&
                           Snake1.MasSnake[i].Y == Snake1.MasSnake[i].Y)
                        {
                            Snake1.Die();
                        }
                    }

                    //Detect collision with food piece
                    if (Snake1.MasSnake[0].X == Food.X && Snake1.MasSnake[0].Y == Food.Y)
                    {
                        Snake1.Eat();
                        lblScore.Text = Settings.Score.ToString();

                        GenerateFood();
                    }
                }
                else
                {
                    //Move body
                    Snake1.MasSnake[i].X = Snake1.MasSnake[i -1].X;
                    Snake1.MasSnake[i].Y = Snake1.MasSnake[i -1].Y;
                }
            }
        }

        private void panField_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = panField.CreateGraphics();

            if (!Settings.GameOver)
            {
                //Draw snake
                for (int i = 0; i < Snake1.MasSnake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                    {
                        snakeColour = Brushes.OrangeRed;     //Draw head
                    }
                    else if (i % 2 == 0)
                    {
                        Color myColor = Color.FromArgb(60, 60, 60);
                        snakeColour = new SolidBrush(myColor);    //Rest of body
                    }
                    else
                    {
                        Color myColor = Color.FromArgb(75, 75, 75);
                        snakeColour = new SolidBrush(myColor);
                    }
                    //Draw snake
                    canvas.FillRectangle(snakeColour, new Rectangle(Snake1.MasSnake[i].X * Settings.Width,
                         Snake1.MasSnake[i].Y * Settings.Height, Settings.Width, Settings.Height));

                    //Draw Food
                    canvas.FillRectangle(Brushes.Orange, new Rectangle(Food.X * Settings.Width,
                        Food.Y * Settings.Height, Settings.Width, Settings.Height));
                }
            }
            else
            {
                string s1 = "Игра окончена\nВаш финальный счет: ";
                lblGameOver.Text = s1;
                label2.Text = Settings.Score.ToString();
                label3.Text = "\nНажмите <Enter>\n для начала новой игры";

                label2.Visible = true;
                label3.Visible = true;
                lblGameOver.Visible = true;
            }
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
    }
}