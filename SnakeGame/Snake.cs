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
        public void Eat()
        {
            Point food = new Point(MasSnake[MasSnake.Count - 1].X, MasSnake[MasSnake.Count - 1].Y);
            MasSnake.Add(food);

            Settings.Score += Settings.Points;
        }
        public void Die()
        {
            Settings.GameOver = true;
        }
        public bool IsHeatTail()
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
    }
}
