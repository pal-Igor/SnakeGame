using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public class Snake
    {
        public Snake(int x, int y)
        {
            MasSnake = new List<Point>();
            MasSnake.Add(new Point(x, y));
        }

        public Point this[int i]
        {
            set { MasSnake[i] = value; }
            get { return MasSnake[i]; }
        }
        public List<Point> MasSnake { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void CreateHead()
        {
            Point head = new Point(1, 2);
            MasSnake.Add(head);
        }
        public void Clear()
        {
            MasSnake.Clear();
        }
        public bool Eat(Point food)
        {
            if (MasSnake[0].X == food.X && MasSnake[0].Y == food.Y)
            {
                MasSnake.Add(food);
                Settings.Score += Settings.Points;
                return true;
            }
            return false;
        }
        public void Die()
        {
            Settings.GameOver = true;
        }
        public bool IsHitTail()
        {
            for (int i = 0; i < MasSnake.Count; i++)
            {
                for (int j = 1; j < MasSnake.Count; j++)
                {
                    if (MasSnake[i].X == MasSnake[j].X &&
                       MasSnake[i].Y == MasSnake[j].Y)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
        public bool IsHitBorder(int maxXPos, int maxYPos)
        {
            for (int i = 0; i < MasSnake.Count; i++)
            {
                if (MasSnake[i].X < 0 || MasSnake[i].Y < 0
                        || MasSnake[i].X >= maxXPos || MasSnake[i].Y >= maxYPos)
                {
                    return true;
                }
            }
            return false;
        }
        public void Move()
        {
            for (int i = MasSnake.Count - 1; i >= 0; i--)
            {
                //Move head
                if (i == 0)
                {
                    switch (Settings.Direction)
                    {
                        case Direction.Right:
                            MasSnake[i].X++;
                            break;
                        case Direction.Left:
                            MasSnake[i].X--;
                            break;
                        case Direction.Up:
                            MasSnake[i].Y--;
                            break;
                        case Direction.Down:
                            MasSnake[i].Y++;
                            break;
                    }
                }
                else
                {
                    //Move body
                    MasSnake[i].X = MasSnake[i - 1].X;
                    MasSnake[i].Y = MasSnake[i - 1].Y;
                }
            }
        }
    }
}
