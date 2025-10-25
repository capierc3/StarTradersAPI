using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTradersCLI
{
    internal class CLIUtils
    {
        private const int Delay = 25;
        private const string FontColor = "bold green";

        public static async Task Type(string text)
        {
            foreach (char c in text)
            {
                AnsiConsole.Markup($"[{FontColor}]{c}[/]");
                await Task.Delay(Delay);
            }
            Console.WriteLine();
        }

        public static async Task DisplayLoadingAsync(CancellationToken token)
        {
            var sequence = new[] { ".", "..", "...", "..", "." };
            int counter = 0;

            while (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"\r{sequence[counter++ % sequence.Length]}");
                await Task.Delay(300);
            }

            Console.Write("\r "); // clear spinner
        }

        public static async Task LoadingPrint(string text, CancellationToken token)
        {
            int maxDots = 5;
            int delay = 500;
            int dotCount = 0;

            while (!token.IsCancellationRequested)
            {
                dotCount = (dotCount % maxDots) + 1;
                string dots = new string('.', dotCount);
                AnsiConsole.Markup(new string(' ', 20));
                AnsiConsole.Markup($"\r[{FontColor}]{text}{dots}[/]");
                await Task.Delay(delay);
            }
            Console.WriteLine();
        }

        public static async Task printBanner()
        {
            string[] bannerLines = new string[]
            {
                "+=============================================+",
                "|   _____                                     |",
                "|  / ___/ ____   ____ _ _____ ___             |",
                "|  \\__ \\ / __ \\ / __ `// ___// _ \\            |",
                "| ___/ // /_/ // /_/ // /__ /  __/            |",
                "|/____// .___/ \\__,_/ \\___/ \\___/             |",
                "|     /_/                                     |",
                "|  ______                  __                 |",
                "| /_  __/_____ ____ _ ____/ /___   _____ _____|",
                "|  / /  / ___// __ `// __  // _ \\ / ___// ___/|",
                "| / /  / /   / /_/ // /_/ //  __// /   (__  ) |",
                "|/_/  /_/    \\__,_/ \\__,_/ \\___//_/   /____/  |",
                "+=============================================+",


            };
            foreach (var line in bannerLines)
            {
                AnsiConsole.MarkupLine($"[{FontColor}]{line}[/]");
                await Task.Delay(100);
            }
        }
    }
}
