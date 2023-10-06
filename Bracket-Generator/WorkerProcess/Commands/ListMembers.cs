using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerProcess.Managers;
using WorkerProcess.Models;
using WorkerProcess.TeamTracker;

namespace WorkerProcess.Commands;

public class ListMembers : ModuleBase<SocketCommandContext>
{
    [Command("bList")]
    [Summary("Creates buttons to opt into bracket generation")]
    public async Task Execute()
    {
        await Context.Message.DeleteAsync();
        var channelId = Context.Channel.Id;

        if (!TeamSource.Instance.ContainsInfo(channelId))
        {
            await ReplyAsync("You need to create a tournament before trying to list it's members");
            return;
        }

        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("The current tournament members are");

        foreach (var player in TeamSource.Instance.GetPlayers(channelId))
        {
            stringBuilder.AppendLine(player.Username);
        }

        var sentMessage = await ReplyAsync(stringBuilder.ToString());
        MessageManager.AddToList(channelId, sentMessage.Id);
    }
}