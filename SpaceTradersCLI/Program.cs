using IO.Swagger.Model;
using SpaceTradersCLI;
using Spectre.Console;

class Program
{

    static AppSettings appSettings = new();
    static SpaceTradersAPIHelper apiHelper = new(appSettings);
    static ConsoleKey key;
    static async Task Main(string[] args)
    {
        AnsiConsole.Cursor.Hide();
        Console.CursorVisible = false;
        Console.Title = "SpaceTraders Hub";
        CLIUtils.PrintBanner();
        Login();
        //MainMenu();
    }

    static void Login()
    {
        if(!apiHelper.CheckAccount())
        {
            CreateAppScreen();
        }
        var response = apiHelper.GetGlobalApi().GetStatus();
        CLIUtils.Type($"API STATUS: {response.Status}");
        CLIUtils.Type($"NEXT SERVER RESET: {response.ServerResets.Next}");
        if (!apiHelper.CheckAgent())
        {
            CreateAgentScreen();
        }
    }

    static void CreateAppScreen()
    {
        CLIUtils.Type("ERROR: SYSTEM NOT REGISTERED!!", "red");
        CLIUtils.Type("CREATE AN ACCOUNT AND ACOUNT TOKEN HERE:", "red");
        CLIUtils.Type("https://my.spacetraders.io/login", "red");
        CLIUtils.Type("ENTER YOUR APP TOKEN:", "red");
        string inputToken = AnsiConsole.Ask<string>("> ");
        appSettings.SaveAppAuth(inputToken);
        apiHelper.LoadTokens();
        CLIUtils.ClearScreen();
    }

    static void CreateAgentScreen()
    {
        CLIUtils.Type("ERROR: NO AGENT DETECTED!!", "red");
        string[] menuOptions =
        [
            "Create New Agent",
            "Add Agent Token",
            "Exit"
        ];
        Action[] menuActions =
        [
            CreateNewAgent,
            AddAgentToken,
            Exit,
        ];
        DisplayMenu(menuOptions, menuActions);
        CLIUtils.ClearScreen();
    }
    
    static async void MainMenu()
    {
        var cts = new CancellationTokenSource();
        var spinnerTask = CLIUtils.LoadingPrint("INITIALIZING!", cts.Token);
        Agent agent = apiHelper.GetAgentData();
        cts.Cancel();
        await spinnerTask;
        CLIUtils.Type($"WELCOME BACK AGENT {agent.Symbol}!");
        CLIUtils.Type($"CURRENT CREDITS: {agent.Credits}!");
        string[] menuOptions =
        [
            "View Fleet",
            "View Contracts",
            "Exit"
        ];
        Action[] menuActions = 
        [
            ViewFleet,
            ViewContracts,
            Exit,
        ];
        DisplayMenu(menuOptions, menuActions);
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
        Action[] menuActions =
        [
            MainMenu,
            ViewContracts,
            Exit,
        ];
        DisplayMenu(menuOptions, menuActions);
    }

    static void ViewContracts()
    {
        CLIUtils.Type("VIEW CONTRACTS - FEATURE COMING SOON!", "yellow");
    }

    static void CreateNewAgent()
    {
        
        CLIUtils.Type("ENTER YOUR DESIRED AGENT NAME:", "red");
        string inputName = AnsiConsole.Ask<string>("> ");
        string name = inputName;
        string[] menuOptions =
        [
            "View Faction Info",
            "Pick Faction",
            "Exit"
        ];
        Action[] menuActions = new Action[]
        {
            ViewFactions,
            PickFaction,
            Exit,
        };
        DisplayMenu(menuOptions, menuActions);
        FactionSymbol factionSymbol = FactionSymbol.ECHO; // Placeholder, should be set based on user choice
        RegisterBody registerBody = new(factionSymbol, name);


    }

    static void ViewFactions()
    {
        
    }

    static void PickFaction()
    {
        CLIUtils.Type("ENTER YOUR DESIRED FACTION:", "red");
        
    }

    static void AddAgentToken()
    {
        CLIUtils.Type("ENTER YOUR AGENT TOKEN:", "red");
        string inputToken = AnsiConsole.Ask<string>("> ");
        appSettings.SaveAppAuth(inputToken);
        apiHelper.LoadTokens();
    }

    static void DisplayMenu(string[] menuOptions, Action[] menuActions)
    {
        CLIUtils.PrintMenu(menuOptions);
        int selectedIndex = 0;
        bool menuChaged = false;
        while (!menuChaged)
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.Enter:
                    menuActions[selectedIndex]();
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

    public static void Exit()
    {
        CLIUtils.Type("GOOD BYE AGENT...");
        Thread.Sleep(1500);
        Environment.Exit(0);
    }

}
