using System;
using System.Threading;

namespace SubredditStats.Frontend.ConsoleClient.Utils
{
    public class ConsoleAnimation : IDisposable
    {
        public delegate string GetFrameTextFunc(uint frameNumber);

        private const int DefaultDelayMs = 0;

        public int Top { get; private set; }
        public int Left { get; private set; }

        public int FrameDelayMs { get; set; }
        public bool HideCursor { get; set; }
        public bool Clear { get; set; }
        public string LastFrame { get; set; }

        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => _consoleColors.ForegroundColor = value;
        }
        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => _consoleColors.BackgroundColor = value;
        }

        private readonly GetFrameTextFunc _getFrameTextFunc;
        private readonly Thread _thread;

        private readonly ConsoleColors _consoleColors;

        private volatile bool _stop = false;
        private uint _frameNumber;
        private int _longestFrameText;
        private (int Left, int Top) _prevPosition;

        public ConsoleAnimation(int left, int top, GetFrameTextFunc getFrameTextFunc)
        {
            Top = top;
            Left = left;

            _getFrameTextFunc = getFrameTextFunc;
            _thread = new Thread(DrawFrameLoop);

            _prevPosition = Console.GetCursorPosition();
            _frameNumber = 0;
            _longestFrameText = 0;
            _consoleColors = new ConsoleColors();
            FrameDelayMs = DefaultDelayMs;
            HideCursor = true;
            Clear = false;
            LastFrame = "";

            ForegroundColor = Console.ForegroundColor;
            BackgroundColor = Console.BackgroundColor;
        }

        public ConsoleAnimation(GetFrameTextFunc getFrameTextFunc)
            : this(-1, -1, getFrameTextFunc)
        {
        }

        private string LongestFrameLengthBlank() => new(' ', _longestFrameText + 1);

        public void Dispose()
        {
            Stop();
            _consoleColors.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            if (HideCursor)
            {
                Console.CursorVisible = false;
            }

            // if no Left, Top provided, then use the current position (i.e. just after the last Console.Write)
            if (Left == -1 &&
                Top == -1)
            {
                (Left, Top) = Console.GetCursorPosition();
            }

            _thread.Start();
        }

        private void SavePosition()
        {
            _prevPosition = Console.GetCursorPosition();
        }

        public void ResetPosition()
        {
            Console.SetCursorPosition(_prevPosition.Left, _prevPosition.Top);
        }

        public void Stop()
        {
            _stop = true;
            if (_thread.IsAlive)
            {
                _thread.Join();
            }

            SavePosition();

            if (Clear)
            {
                EraseFrame();
            }

            if (!string.IsNullOrWhiteSpace(LastFrame))
            {
                EraseFrame();
                DrawFrame(LastFrame);
            }

            ResetPosition();

            if (HideCursor)
            {
                Console.CursorVisible = true;
            }
        }

        private void EraseFrame()
        {
            DrawFrame(LongestFrameLengthBlank());
        }

        private void DrawFrame(string frameText)
        {
            Console.Write(LongestFrameLengthBlank());
            Console.SetCursorPosition(Left, Top);
            Console.Write(frameText);
        }

        private void DrawFrameLoop()
        {
            while (!_stop)
            {
                var ft = GetFrameText(_frameNumber++);
                DrawFrame(ft);
                // capture longest frame text length for erasing when Stop()'ing
                if (ft.Length > _longestFrameText)
                {
                    _longestFrameText = ft.Length;
                }
                Thread.Sleep(FrameDelayMs);
            }
        }

        // override in extended base class to implement specific animation types
        protected virtual string GetFrameText(uint frameNumber)
        {
            return _getFrameTextFunc(frameNumber);
        }
    }
}
