using Discord;
using Discord.Commands;
using WorkerProcess.Constants;
using WorkerProcess.Helpers;
using WorkerProcess.Managers;
using WorkerProcess.TeamTracker;

namespace WorkerProcess.Commands;
public class CreateBracket : ModuleBase<SocketCommandContext>
{
    [Command("bCreate")]
    [Summary("Creates buttons to opt into bracket generation")]
    public async Task Execute()
    {
        await Context.Channel.DeleteMessageAsync(Context.Message.Id);

        if (TeamSource.Instance.ContainsInfo(Context.Channel.Id))
        {
            await ReplyAsync(
                "Team building already happening for this Channel, please finalize the current generation first");
            return;
        }

        TeamSource.Instance.AddChannel(Context.Channel.Id, Context.User);

        int userCount = 0;
        var onlineUserList = Context.Channel.GetUsersAsync();
        await foreach (var userSet in onlineUserList)
        {
            userCount = userSet.Count(m => (m.Status != UserStatus.Offline || Constants.TeamConstants.IsTest) && !m.IsBot);

            if (TeamConstants.IsTest)
            {
                foreach (var user in userSet.Where(m => (m.Status != UserStatus.Offline || Constants.TeamConstants.IsTest) && !m.IsBot))
                {
                    TeamSource.Instance.AddUser(Context.Channel.Id, user);
                }
            }
        }

        if (!Constants.TeamConstants.IsTest)
        {
            if (userCount < 4)
            {
                await MessageManager.DeleteMessagesFromChannel(Context.Channel);
                await ReplyAsync("Please wait until you have at least 4 available people to start making teams");
                TeamSource.Instance.ClearChannel(Context.Channel.Id);
            }
        }

        var builder = new ComponentBuilder()
            .WithButton("I'm IN, let's fucking GOOOO", "InButton")
/*            .WithButton("Leave me outta this one, I'm no craic", "OutButton")*/;

        var teamCountSelectMenu = new SelectMenuBuilder("TeamCountSelectMenu", minValues:1, maxValues:1);
        teamCountSelectMenu.Options = new List<SelectMenuOptionBuilder>();

        // Always include the option for smaller teams, some teams will just be without players (I guess)
        for (int i = 2; i <= 5; i++)
        {
            teamCountSelectMenu.AddOption(i.ToString(), i.ToString());
        }
        for (int i = 5; i <= userCount; i++)
        {
            if (userCount % i == 0 && userCount != i && i != 1)
            {
                teamCountSelectMenu.AddOption(i.ToString(), i.ToString());
            }
        }

        var teamCountRow = new ActionRowBuilder();
        teamCountRow.WithSelectMenu(teamCountSelectMenu);
        builder.AddRow(teamCountRow);

        // Otherwise there'll be a way to tell how many people are included.
        var row = new ActionRowBuilder()
            .WithButton("Start Generation", "Start", ButtonStyle.Success)
            .WithButton("Cancel", "Cancel", ButtonStyle.Danger);
        builder.AddRow(row);

        var message =
            await ReplyAsync($"{MarkDownHelper.Header}{Context.User.Username} has started generating a tournament",
                components: builder.Build());

        MessageManager.AddToList(message.Channel.Id, message.Id);
    }
}
