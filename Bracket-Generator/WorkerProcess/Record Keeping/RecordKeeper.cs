using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorkerProcess.TeamTracker;
using Discord.Commands;
using WorkerProcess.Record_Keeping.Models;
using Match = WorkerProcess.Models.Match;

namespace WorkerProcess.Record_Keeping;
public sealed class RecordKeeper
{
    private string _historyFile = "records.json";

    public async Task<string> PrintRecords(SocketCommandContext context)
    {
        var stringBuilder = new StringBuilder();

        if (!File.Exists(_historyFile))
        {
            File.Create(_historyFile).Close();

            var content = new MatchHistory()
            {
                Games = new Dictionary<ulong, List<Match>> { { context.Channel.Id, new List<Match>() } }
            };

            await File.WriteAllTextAsync(_historyFile, JsonConvert.SerializeObject(content));
        }

        using StreamReader reader = new StreamReader(_historyFile);
        var jsonContent = await reader.ReadToEndAsync();
        var matchHistory = JsonConvert.DeserializeObject<MatchHistory>(jsonContent);

        var relevantGames = matchHistory.Games[context.Channel.Id];

        if (!relevantGames.Any())
        {
            return "I don't have any records for this channel";
        }

        return stringBuilder.ToString();
    }

    // Singleton stuff
    private RecordKeeper() { }

    private static readonly Lazy<RecordKeeper> lazy = new(() => new RecordKeeper());

    public static RecordKeeper Instance => lazy.Value;
}
