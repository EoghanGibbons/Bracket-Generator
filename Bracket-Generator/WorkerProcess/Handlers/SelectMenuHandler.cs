using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using WorkerProcess.TeamTracker;

namespace WorkerProcess.Handlers;
public static class SelectMenuHandler
{
    public static async Task SelectMenu(SocketMessageComponent args)
    {
        switch (args.Data.CustomId)
        {
            case "TeamCountSelectMenu":
                var value = int.Parse(args.Data.Values.First());

                TeamSource.Instance.SetPlayersPerTeam(args.ChannelId.Value, value);

                var menu = new SelectMenuBuilder()
                {
                    IsDisabled = true,
                    MinValues = 1,
                    MaxValues = 1,
                    Placeholder = $"{value}"
                };

                menu.AddOption("meh", "1", $"{value}");

                await args.UpdateAsync(x =>
                {
                    x.Content = $"There's going to be {value} people per team";
                    x.Components = new ComponentBuilder().WithSelectMenu(menu).Build();
                });

                break;
            default:
                break;
        }
    }
}
