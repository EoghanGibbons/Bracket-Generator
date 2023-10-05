using Discord;
using Discord.Commands;
using Discord.WebSocket;
using WorkerProcess;
using WorkerProcess.Handlers;

public class Program
{


    private DiscordSocketClient _client;
    private CommandHandler _handler;

    public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient(config: new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.All
        });
        
        await _client.StartAsync();
        _client.ButtonExecuted += ButtonHandler.MyButtonHandler;
        _client.SelectMenuExecuted += SelectMenuHandler.SelectMenu;

        _handler = new CommandHandler(_client, new CommandService());
        await _handler.InstallCommands();

        // Never completes, constantly listens
        await Task.Delay(-1);
    }
}