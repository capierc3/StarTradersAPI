using IO.Swagger.Model;
using SpaceTradersCLI;
using Spectre.Console;

class Program
{

    static AppSettings appSettings = new();
    static SpaceTradersAPIHelper apiHelper = new(appSettings);
    static ConsoleKey key;
    static FactionSymbol selectedFaction;
    static async Task Main(string[] args)
    {
        AnsiConsole.Cursor.Hide();
        Console.CursorVisible = false;
        Console.Title = "SpaceTraders Hub";
        CLIUtils.PrintBanner();
        Login();
        MainMenu();
        while (true) {}
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
        CLIUtils.ClearScreen();
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
        while (!apiHelper.CheckAgent())
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
        }
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
        CLIUtils.ClearScreen();
        CLIUtils.Type("ENTER YOUR DESIRED AGENT NAME:", "red");
        string inputName = AnsiConsole.Ask<string>("> ");
        string name = inputName;
        string[] menuOptions =
        [
            "Pick Faction",
            "View Full Faction Info",
            "Exit"
        ];
        Action[] menuActions =
        [
            PickFaction,
            ViewFactions,
            Exit,
        ];
        DisplayMenu(menuOptions, menuActions);
        RegisterBody registerBody = new(selectedFaction, name);
        var response = apiHelper.CreateAgent(registerBody);
        if (response.Data.Agent.Symbol == name)
        {
            CLIUtils.Type($"AGENT {name} CREATED SUCCESSFULLY!", "red");
        }
        else
        {
            CLIUtils.Type("ERROR CREATING AGENT. PLEASE TRY AGAIN.", "red");
            Thread.Sleep(1000);
            CLIUtils.ClearScreen();
        }
    }

    static void ViewFactions()
    {
        CLIUtils.Type("COMING SOON. PLEASE TRY AGAIN LATER.", "yellow");
    }

    static void PickFaction()
    {
        CLIUtils.ClearScreen();
        CLIUtils.Type("FETECHING FACTIONS LIST..", "red");
        var factions =  apiHelper.getFactions();
        string[] menuOptions = new string[factions.Count + 1];
        Action[] menuActions = new Action[factions.Count + 1];
        for (int i = 0; i < factions.Count; i++)
        {
            int index = i;
            menuOptions[i] = factions[i].Name;
            ;
            menuActions[i] = () =>
            {
                CLIUtils.Type($"YOU HAVE SELECTED FACTION: {factions[index].Name}", "red");
                selectedFaction = factions[index].Symbol;
            };
        }
        menuOptions[factions.Count] = "View Full Faction Info";
        menuActions[factions.Count] = ViewFactions;
        DisplayMenu(menuOptions, menuActions);

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
                    menuChaged = true;
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
