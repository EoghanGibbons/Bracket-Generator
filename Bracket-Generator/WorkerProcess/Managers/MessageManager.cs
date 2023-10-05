using Discord;
using Discord.WebSocket;
using WorkerProcess.Constants;
using WorkerProcess.TeamTracker;

namespace WorkerProcess.Managers;
public static class MessageManager
{
    /// <summary>
    /// Keeps track of all messages sent in a channel, and deletes them when needed. 
    /// </summary>
    private static Dictionary<ulong, List<ulong>> SentMessagesPerChannel = new();

    public static void AddToList(ulong channelId, ulong messageId)
    {
        if (!SentMessagesPerChannel.ContainsKey(channelId))
        {
            SentMessagesPerChannel.Add(channelId, new List<ulong>());
        }

        SentMessagesPerChannel[channelId].Add(messageId);
    }

    public static Task DeleteMessagesFromChannel(ISocketMessageChannel channel)
    {
        foreach (var message in SentMessagesPerChannel[channel.Id])
        {
            channel.DeleteMessageAsync(message);
        }
        
        return Task.CompletedTask;
    }
}
