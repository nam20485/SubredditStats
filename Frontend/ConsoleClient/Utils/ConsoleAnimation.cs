using System;
using System.Threading;

using SubredditStats.Shared.Model;

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
        public bool DelayStart { get; set; }

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
        private int _mostNumberOfLines;
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
            _mostNumberOfLines = 1;
            _consoleColors = new ConsoleColors();
            FrameDelayMs = DefaultDelayMs;
            HideCursor = true;
            Clear = false;
            LastFrame = "";
            DelayStart = false;

            ForegroundColor = Console.ForegroundColor;
            BackgroundColor = Console.BackgroundColor;

            if (!DelayStart)
            {
                Start();
            }
        }

        public ConsoleAnimation(GetFrameTextFunc getFrameTextFunc)
            : this(-1, -1, getFrameTextFunc)
        {
        }

        public void Start()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

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
                DrawFrame(LastFrame);
            }

            ResetPosition();

            if (HideCursor)
            {
                Console.CursorVisible = true;
            }
        }

        private (int, int) DrawFrame(string frameText)
        {
            for (int i = 0; i < _mostNumberOfLines; i++)
            {
                Console.WriteLine(LongestFrameLengthBlank);
            }
            Console.SetCursorPosition(Left, Top);
            Console.Write(frameText);
            return (frameText.Length, CountLines(frameText));
        }

        private static int CountLines(string text)
        {            
            return text.Count(c => c.Equals(Environment.NewLine));
        }

        private void DrawFrameLoop()
        {
            while (!_stop)
            {
                try
                {                    
                    (int written, int lines) = DrawFrame(GetFrameText(_frameNumber++));
                    // capture longest frame text length for erasing when Stop()'ing
                    if (written > _longestFrameText ||
                        lines > _mostNumberOfLines)
                    {
                        _longestFrameText = written;
                        _mostNumberOfLines = lines;
                    }
                    Thread.Sleep(FrameDelayMs);
                }
                catch (Exception e)
                {
                    DrawFrame(e.ToString()+Environment.NewLine);
                }
            }
        }

        // override in extended base class to implement specific animation types
        protected virtual string GetFrameText(uint frameNumber)
        {
            return _getFrameTextFunc(frameNumber);
        }

        private void EraseFrame()
        {
            for (int i = 0; i < _mostNumberOfLines; i++)
            {
                Console.WriteLine(LongestFrameLengthBlank);
            }
        }

        private string LongestFrameLengthBlank => new(' ', _longestFrameText + 1);

        public void Dispose()
        {
            Stop();
            _consoleColors.Dispose();
            GC.SuppressFinalize(this);
        }

        private void SavePosition()
        {
            _prevPosition = Console.GetCursorPosition();
        }

        public void ResetPosition()
        {
            Console.SetCursorPosition(_prevPosition.Left, _prevPosition.Top);
        }
    }
}
