using IO.Swagger.Api;
using IO.Swagger.Model;
using SpaceTradersCLI;
using Spectre.Console;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        SpaceTradersAPIHelper apiHelper = new SpaceTradersAPIHelper();

        AnsiConsole.Cursor.Hide();
        Console.Title = "SpaceTraders Hub";
        await CLIUtils.printBanner();
        var cts = new CancellationTokenSource();
        var spinnerTask = CLIUtils.LoadingPrint("INITIALIZING!", cts.Token);
        Agent agent = apiHelper.GetAgentData();
        List<Ship> fleet = apiHelper.getFleetData();
        cts.Cancel();
        await spinnerTask;
        AnsiConsole.Clear();
        await CLIUtils.Type($"WELCOME BACK AGENT {agent.Symbol}!");
        await CLIUtils.Type($"CURRENT CREDITS: {agent.Credits}!");
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


        while (true)
        {

        }
    }
}
