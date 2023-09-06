using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubredditStats.Frontend.ConsoleClient.Utils
{
    public class WaitForExitChar
    {
        public ConsoleKey ConsoleKey { get; }
        public ConsoleModifiers ConsoleKeyModifiers { get; }

        public WaitForExitChar(ConsoleKey consoleKey, ConsoleModifiers consoleKeyModifiers)
        {
            ConsoleKey = consoleKey;
            ConsoleKeyModifiers = consoleKeyModifiers;
        }

        public void Wait()
        {
            // CTRL+m to exit
            WaitForExit(ConsoleKey, ConsoleKeyModifiers);
        }

        public static void Wait(ConsoleKey key, ConsoleModifiers modifiers)
        {
            new WaitForExitChar(key, modifiers).Wait();
        }

        private void WaitForExit(ConsoleKey key, ConsoleModifiers modifiers)
        {
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == key &&
                    keyInfo.Modifiers == modifiers)
                {
                    // key pressed, exit
                    break;
                }
            }
        }
    }
}
