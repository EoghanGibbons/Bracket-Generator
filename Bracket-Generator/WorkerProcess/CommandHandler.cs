using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace WorkerProcess;
public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;

    public CommandHandler(DiscordSocketClient client, CommandService commands)
    {
        _commands = commands;
        _client = client;
    }

    public async Task InstallCommands()
    {
        _client.MessageReceived += HandleCommand;

        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
    }

    private async Task HandleCommand(SocketMessage messageParam)
    {
        var message = messageParam as SocketUserMessage;

        if (message == null)
        {
            return;
        }

        int argPos = 0;

        if (!message.HasStringPrefix("!", ref argPos) || message.Author.IsBot)
        {
            return;
        }

        var context = new SocketCommandContext(_client, message);

        await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);
    }
}
