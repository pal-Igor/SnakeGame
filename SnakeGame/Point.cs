using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
            Width = Settings.Width;
            Height = Settings.Height;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsSamePoint(Point point)
        {
            return point.X == this.X && point.Y == this.Y;
        }
    }
}
