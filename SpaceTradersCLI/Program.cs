using IO.Swagger.Api;
using IO.Swagger.Model;
using SpaceTradersCLI;
using Spectre.Console;
using System.Threading.Tasks;

class Program
{
    static SpaceTradersAPIHelper apiHelper = new SpaceTradersAPIHelper();
    static ConsoleKey key;
    static async Task Main(string[] args)
    {
        AnsiConsole.Cursor.Hide();
        Console.CursorVisible = false;
        Console.Title = "SpaceTraders Hub";
        await CLIUtils.PrintBanner();
        await StartUp();
    }

    static async Task StartUp()
    {
        var cts = new CancellationTokenSource();
        var spinnerTask = CLIUtils.LoadingPrint("INITIALIZING!", cts.Token);
        Agent agent = apiHelper.GetAgentData();
        cts.Cancel();
        await spinnerTask;
        await CLIUtils.Type($"WELCOME BACK AGENT {agent.Symbol}!");
        await CLIUtils.Type($"CURRENT CREDITS: {agent.Credits}!");
        string[] menuOptions = new string[]
        {
            "View Fleet",
            "View Contracts",
            "Exit"
        };
        await CLIUtils.PrintMenu(menuOptions);
        int selectedIndex = 0;
        while (true)
        {
            key = Console.ReadKey(true).Key;
            switch(key)
            {
                case ConsoleKey.Enter:
                    if (selectedIndex == 0)
                    {
                        CLIUtils.ClearScreen();
                        ViewFleet();
                    }
                    else if (selectedIndex == 1)
                    {
                        //View Contracts
                    }
                    else if (selectedIndex == 2)
                    {
                        CLIUtils.ClearScreen();
                        await Exit();
                        return;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    selectedIndex--;
                    if (selectedIndex < 0)
                    {
                        selectedIndex = menuOptions.Length - 1;
                    }
                    CLIUtils.RefreshMenu(menuOptions, selectedIndex);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % menuOptions.Length;
                    CLIUtils.RefreshMenu(menuOptions, selectedIndex);
                    break;
                default:
                    break;
            }
        }
    }

    static async void ViewFleet()
    {
        var cts = new CancellationTokenSource();
        var spinnerTask = CLIUtils.LoadingPrint("FETCHING FLEET DATA...!", cts.Token);
        List<Ship> fleet = apiHelper.getFleetData();
        cts.Cancel();
        await spinnerTask;
        var table = new Table();
        table.ShowRowSeparators();
        table.Border(TableBorder.Ascii2);
        table.BorderColor(Color.Green);
        AnsiConsole.Live(table)
            .Start(ctx =>
            {
                table.AddColumn("[bold green]Ship Name[/]");
                ctx.Refresh();
                Thread.Sleep(500);

                table.AddColumn("[bold green]Ship Type[/]");
                ctx.Refresh();
                Thread.Sleep(500);

                foreach (var ship in fleet)
                {
                    table.AddEmptyRow();
                    table.UpdateCell(table.Rows.Count - 1, 0, "[bold green]" + ship.Symbol + "[/]");
                    ctx.Refresh();
                    Thread.Sleep(500);
                    table.UpdateCell(table.Rows.Count - 1, 1, "[bold green]" + ship.Frame.Name + "[/]");
                    ctx.Refresh();
                    Thread.Sleep(500);
                }
            });

        string[] menuOptions =
        [
            "Main Menu",
            "View Contracts",
            "Exit"
        ];
        await CLIUtils.PrintMenu(menuOptions);
        int selectedIndex = 0;
        while (true)
        {
            key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Enter:
                    if (selectedIndex == 0)
                    {
                        CLIUtils.ClearScreen();
                        await StartUp();
                    }
                    else if (selectedIndex == 1)
                    {
                        //View Contracts
                    }
                    else if (selectedIndex == 2)
                    {
                        CLIUtils.ClearScreen();
                        await Exit();
                        return;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    selectedIndex--;
                    if (selectedIndex < 0)
                    {
                        selectedIndex = menuOptions.Length - 1;
                    }
                    CLIUtils.RefreshMenu(menuOptions, selectedIndex);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % menuOptions.Length;
                    CLIUtils.RefreshMenu(menuOptions, selectedIndex);
                    break;
                default:
                    break;
            }
        }
    }



    static async Task Exit()
    {
        await CLIUtils.Type("GOOD BYE AGENT...");
        await Task.Delay(1500);
        Environment.Exit(0);
    }
}
