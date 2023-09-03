using System;

namespace SubredditStats.Frontend.ConsoleClient.Utils
{
    public class ConsoleColors : IDisposable
    {
        private ConsoleColor _previousForegroundColor;
        private ConsoleColor _previousBackgroundColor;

        public ConsoleColors()
        {
        }

        public ConsoleColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public ConsoleColors(ConsoleColor foregroundColor)
        {
            ForegroundColor = foregroundColor;
        }

        public ConsoleColor ForegroundColor
        {
            set
            {
                _previousForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = value;
            }
        }

        public ConsoleColor BackgroundColor
        {
            set
            {
                _previousBackgroundColor = Console.BackgroundColor;
                Console.BackgroundColor = value;
            }
        }

        public void ResetForegroundColor()
        {
            Console.ForegroundColor = _previousForegroundColor;
        }

        public void ResetBackgroundColor()
        {
            Console.BackgroundColor = _previousBackgroundColor;
        }

        public void ResetColors()
        {
            ResetForegroundColor();
            ResetBackgroundColor();
        }

        public void Dispose()
        {
            ResetColors();
        }
    }
}
