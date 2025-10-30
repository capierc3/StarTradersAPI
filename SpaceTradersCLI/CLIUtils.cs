using Spectre.Console;

namespace SpaceTradersCLI
{
    internal class CLIUtils
    {
        private const int Delay = 25;
        private const string FontColor = "bold green";
        private const int TopLine = 14;

        public static void Type(string text, string fontColor = FontColor)
        {
            foreach (char c in text)
            {
                AnsiConsole.Markup($"[{fontColor}]{c}[/]");
                Thread.Sleep(Delay);
            }
            Console.WriteLine();
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
            Console.SetCursorPosition(0, Console.CursorTop);
            AnsiConsole.Markup(new string(' ', 20));
            Console.WriteLine();
        }

        public static void PrintBanner()
        {
            string[] bannerLines = new string[]
            {
                "+==========================================================+",
                "|   _____                                                  |",
                "|  / ___/ ____   ____ _ _____ ___                          |",
                "|  \\__ \\ / __ \\ / __ `// ___// _ \\                         |",
                "| ___/ // /_/ // /_/ // /__ /  __/                         |",
                "|/____// .___/ \\__,_/ \\___/ \\___/                          |",
                "|     /_/                                                  |",
                "|               ______                  __                 |",
                "|              /_  __/_____ ____ _ ____/ /___   _____ _____|",
                "|               / /  / ___// __ `// __  // _ \\ / ___// ___/|",
                "|              / /  / /   / /_/ // /_/ //  __// /   (__  ) |",
                "|             /_/  /_/    \\__,_/ \\__,_/ \\___//_/   /____/  |",
                "+==========================================================+",
                "                                                          ",
            };
            foreach (var line in bannerLines)
            {
                AnsiConsole.MarkupLine($"[{FontColor}]{line}[/]");
                Thread.Sleep(100);
            }
        }

        public static void PrintMenu(String[] menuItems)
        {
            AnsiConsole.Cursor.Hide();
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == 0)
                {
                    Type($"(*) {menuItems[i]}");
                }
                else
                    Type($"( ) {menuItems[i]}");
            }
        }
        
        public static void RefreshMenu(String[] menuItems, int selectedIndex)
        {
            AnsiConsole.Cursor.SetPosition(0, Console.CursorTop - (menuItems.Length - 1));
            AnsiConsole.Cursor.Hide();
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                {
                    AnsiConsole.Markup($"[{FontColor}](*) {menuItems[i]}[/]");
                    Console.WriteLine();
                }
                else
                {
                    AnsiConsole.Markup($"[{FontColor}]( ) {menuItems[i]}[/]");
                    Console.WriteLine();
                }
                    
            }
        }

        public static void ClearScreen()
        {
            while (Console.CursorTop > TopLine)
            {
                AnsiConsole.Cursor.MoveUp(1);
                Console.Write(new string(' ', Console.BufferWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
                Thread.Sleep(50);
            }
        }
    }
}
