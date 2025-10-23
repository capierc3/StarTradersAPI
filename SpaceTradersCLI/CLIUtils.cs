using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTradersCLI
{
    internal class CLIUtils
    {
        public static async Task TypeLineAsync(string text, int delay = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                await Task.Delay(delay); // milliseconds per character
            }
            Console.WriteLine();
        }
        public static void ClearScreenLineByLine()
        {
            int lines = Console.WindowHeight;
            for (int i = 0; i < lines; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
                Thread.Sleep(20); // delay for fade effect
            }
            Console.SetCursorPosition(0, 0);
        }
        public static void SlideUpClear()
        {
            int height = Console.WindowHeight;
            for (int i = 0; i < height; i++)
            {
                Console.MoveBufferArea(0, 1, Console.WindowWidth, height - 1, 0, 0);
                Thread.Sleep(30);
            }
        }

        public static void DissolveClear()
        {
            Random rand = new Random();
            string[] buffer = new string[Console.WindowHeight];

            for (int i = 0; i < Console.WindowHeight; i++)
            {
                buffer[i] = new string(' ', Console.WindowWidth);
            }

            int remaining = Console.WindowHeight;
            while (remaining > 0)
            {
                int line = rand.Next(Console.WindowHeight);
                if (buffer[line] != null)
                {
                    Console.SetCursorPosition(0, line);
                    Console.Write(new string(' ', Console.WindowWidth));
                    buffer[line] = null;
                    remaining--;
                    Thread.Sleep(10);
                }
            }
            Console.SetCursorPosition(0, 0);
        }
    }
}
