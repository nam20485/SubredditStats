using System;

namespace SubredditStats.Shared.Utils
{
    public class ConsoleColors : IDisposable
    {
        private ConsoleColor _originalForegroundColor;
        private ConsoleColor _originalBackgroundColor;

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
                _originalForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = value;
            }
        }

        public ConsoleColor BackgroundColor
        {
            set
            {
                _originalBackgroundColor = Console.BackgroundColor;
                Console.BackgroundColor = value;
            }
        }

        public void ResetForegroundColor()
        {
            Console.ForegroundColor = _originalForegroundColor;
        }

        public void ResetBackgroundColor()
        {
            Console.BackgroundColor = _originalBackgroundColor;
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
