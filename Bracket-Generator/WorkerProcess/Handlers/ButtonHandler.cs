using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Discord;
using Discord.WebSocket;
using WorkerProcess.BracketGenerators;
using WorkerProcess.Constants;
using WorkerProcess.Helpers;
using WorkerProcess.Managers;
using WorkerProcess.Models;
using WorkerProcess.TeamTracker;

namespace WorkerProcess.Handlers;
public static class ButtonHandler
{
    public static async Task MyButtonHandler(SocketMessageComponent component)
    {
        switch (component.Data.CustomId)
        {
            case "InButton":
                var inMessage = await component.Channel.SendMessageAsync($"{component.User.Mention} is IN! Let's go!");
                MessageManager.AddToList(inMessage.Channel.Id, inMessage.Id);
                TeamSource.Instance.AddUser(component.ChannelId.Value, component.User);
                break;
            case "OutButton":
                var outMessage = await component.Channel.SendMessageAsync($"{component.User.Mention} is out! BOOOOOO!");
                MessageManager.AddToList(outMessage.Channel.Id, outMessage.Id);
                TeamSource.Instance.RemoveUser(component.ChannelId.Value, component.User);
                break;
            case "Start":
                await MessageManager.DeleteMessagesFromChannel(component.Channel);
                await CreateTeamsAndBracket(component.ChannelId.Value, component);
                TeamSource.Instance.ClearChannel(component.ChannelId.Value);
                break;
            case "Cancel":
                await MessageManager.DeleteMessagesFromChannel(component.Channel);
                TeamSource.Instance.ClearChannel(component.ChannelId.Value);
                break;
            default:
                break;
        }
    }

    private static async Task CreateTeamsAndBracket(ulong channelId, SocketMessageComponent component)
    {
        var stringBuilder = new StringBuilder();

        var random = new Random();

        var teamMemberCollection = TeamSource.Instance.GetPlayers(channelId);

        var usedTeamMembers = new List<IUser>();
        var availableTeamNames = TeamConstants.TeamNames;

        var teams = new List<Team>();

        stringBuilder.AppendLine($"{MarkDownHelper.Header}Starting a new Tournament");
        stringBuilder.AppendLine($"{MarkDownHelper.SubHeader}The teams are");

        for (int i = 0; i <= teamMemberCollection.Count/TeamSource.Instance.PlayersPerTeam(channelId); i++)
        {
            var teamMemembers = new List<IUser>();
            var teamName = availableTeamNames[random.Next(availableTeamNames.Count)];

            for (int j = 0; j < TeamSource.Instance.PlayersPerTeam(channelId); j++)
            {
                var availableTeamMembers = teamMemberCollection.Except(usedTeamMembers).ToArray();

                if (availableTeamMembers.Length == 0)
                {
                    continue;
                }

                var userToAdd = availableTeamMembers[random.Next(availableTeamMembers.Length)];

                teamMemembers.Add(userToAdd);
                usedTeamMembers.Add(userToAdd);
            }

            if (teamMemembers.Count == 0)
            {
                continue;
            }

            var finalizedTeam = new Team(teamName, teamMemembers);

            availableTeamNames.Remove(teamName);

            stringBuilder.AppendLine(finalizedTeam.DisplayName);

            teams.Add(finalizedTeam);
        }
        
        stringBuilder.AppendLine($"{MarkDownHelper.Header}And the draw is!");

        if (teams.Count % 2 != 0)
        {
            teams.Add(new Team("Bye round", new List<IUser>()));
        }

        Bracket bracket = null;
        // This is just for now, later we'll do a switch on the selected item, to know what type of tournament we're making
        if (true)
        {
            bracket = RoundRobinBracketGenerator.CreateBracket(teams);
        }
        else
        {
            throw new Exception("Tournament type not implemented yet");
        }

        stringBuilder.AppendLine(bracket.GetDiscordDisplay);

        try
        {
            await component.Channel.SendMessageAsync(stringBuilder.ToString());
        }
        catch (Exception ex)
        {
            var x = 1;
            await component.Channel.SendMessageAsync($"the message was too long to send, but everything else worked");
        }
        // TODO, next is generating the bracket itself
    }
}
