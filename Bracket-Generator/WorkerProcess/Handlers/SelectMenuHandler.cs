using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using WorkerProcess.Models;
using WorkerProcess.TeamTracker;

namespace WorkerProcess.Handlers;
public static class SelectMenuHandler
{
    public static async Task SelectMenu(SocketMessageComponent context)
    {
        switch (context.Data.CustomId)
        {
            case "TeamCountSelectMenu":
                var value = int.Parse(context.Data.Values.First());

                TeamSource.Instance.SetPlayersPerTeam(context.ChannelId.Value, value);
                await context.RespondAsync(
                    $"Players per team set to {value}, there are currently {TeamSource.Instance.GetPlayers(context.ChannelId.Value).Count} players registered",
                    ephemeral: true);

                var playerCount = TeamSource.Instance.GetPlayers(context.ChannelId.Value).Count;

                if (playerCount % value != 0)
                {
                    await context.FollowupAsync(
                        $"There are {playerCount} players, this doesn't break into teams of {value} evenly, at least one team will be down a man",
                        ephemeral: true);
                }

                break;
            default:
                break;
        }
    }
}
