using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public static class Settings
    {
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static bool GameOver { get; set; }
        public static Direction Direction { get; set; }

        static Settings()
        {
            SetToDefaultSettings();
        }
        public static void SetToDefaultSettings()
        {
            Width = 25;
            Height = 25;
            Speed = 10;
            Score = 0;
            Points = 1;
            GameOver = false;
            Direction = Direction.Down;
        }
    }
}
