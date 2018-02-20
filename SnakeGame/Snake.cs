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
    }
}
