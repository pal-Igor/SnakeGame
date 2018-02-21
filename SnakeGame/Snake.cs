using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public class Snake
    {
        public Snake()
        {
            CreateHead();
        }
        protected List<Point> masSnake { get; set; } = new List<Point>();
        public Direction Direction = Direction.Down;
        public bool IsAlive { get; protected set; } = true;
        public Point this[int i]
        {
            set { masSnake[i] = value; }
            get { return masSnake[i]; }
        }

        protected void CreateHead()
        {
            Point head = new Point(1, 2);
            masSnake.Add(head);
        }
        public bool Eat(Point food)
        {
            if (masSnake[0].X == food.X && masSnake[0].Y == food.Y)
            {
                masSnake.Add(food);

                //Update score
                Settings.Score += Settings.Points;
                return true;
            }
            return false;
        }
        public void Die()
        {
            IsAlive = false;
        }
        public bool IsHitTail()
        {
            for (int i = 1; i < masSnake.Count; i++)
            {
                if (masSnake[0].X == masSnake[i].X &&
                    masSnake[0].Y == masSnake[i].Y)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsHitBorder(int maxXPos, int maxYPos)
        {
            for (int i = 0; i < masSnake.Count; i++)
            {
                if (masSnake[i].X < 0 || 
                    masSnake[i].Y < 0 || 
                    masSnake[i].X >= maxXPos || 
                    masSnake[i].Y >= maxYPos)
                {
                    return true;
                }
            }
            return false;
        }
        public void Move()
        {
            for (int i = masSnake.Count - 1; i >= 0; i--)
            {
                //Move head
                if (i == 0)
                {
                    switch (Direction) // move direction from settings to snake !!!
                    {
                        case Direction.Right:
                            masSnake[i].X++;
                            break;
                        case Direction.Left:
                            masSnake[i].X--;
                            break;
                        case Direction.Up:
                            masSnake[i].Y--;
                            break;
                        case Direction.Down:
                            masSnake[i].Y++;
                            break;
                    }
                }
                else
                {
                    //Move body
                    masSnake[i].X = masSnake[i - 1].X;
                    masSnake[i].Y = masSnake[i - 1].Y;
                }
            }
        }
        public bool SnakeContainsPoint(Point point)
        {
            foreach (var snakePoint in masSnake)
            {
                if (snakePoint.IsSamePoint(point))
                {
                    return true;
                }
            }
            return false;
        }
        public void DrawSnake(Graphics field)
        {
            var localSnakeColor = new SnakeColor(255, 70, 0);
            //Draw snake
            foreach (var snakeItem in masSnake)
            {
                Brush snakeColour;
                if (snakeItem == masSnake.First())
                {
                    snakeColour = Brushes.OrangeRed;     //Draw head
                }
                else
                {
                    Color myColor = Color.FromArgb(localSnakeColor.R, localSnakeColor.G, localSnakeColor.B);
                    snakeColour = new SolidBrush(myColor);    //Rest of body
                }

                //Draw snake
                field.FillRectangle(snakeColour, new Rectangle(snakeItem.X * Settings.Width,
                     snakeItem.Y * Settings.Height, Settings.Width, Settings.Height));


                if (localSnakeColor.G > 0)
                {
                    localSnakeColor.G -= 7;
                }
                else
                {
                    localSnakeColor.G += 7;
                }

                localSnakeColor.R -= 2;
                localSnakeColor.B += 3;
            }
        }
        public void SetDirection()
        {
            if (Input.KeyPressed(Keys.Right) && Direction != Direction.Left)
            {
                Direction = Direction.Right;
            }
            else if (Input.KeyPressed(Keys.Left) && Direction != Direction.Right)
            {
                Direction = Direction.Left;
            }
            else if (Input.KeyPressed(Keys.Up) && Direction != Direction.Down)
            {
                Direction = Direction.Up;
            }
            else if (Input.KeyPressed(Keys.Down) && Direction != Direction.Up)
            {
                Direction = Direction.Down;
            }
        }
    }
}
