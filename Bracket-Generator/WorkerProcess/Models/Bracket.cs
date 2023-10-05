using System.Text;
using WorkerProcess.Helpers;

namespace WorkerProcess.Models;
public class Bracket
{
    public Bracket()
    {
        Matches = new List<Match>();
    }

    public List<Match> Matches;
    
    public string GetDiscordDisplay
    {
        get
        {
            var stringBuilder = new StringBuilder();
            string prevRound = string.Empty;

            foreach (var match in Matches)
            {
                if (match.HomeTeam.IsDummy || match.AwayTeam.IsDummy)
                {
                    continue;
                }

                if (!prevRound.Equals(match.Round))
                {
                    stringBuilder.AppendLine($"{MarkDownHelper.SubHeader}Round {match.Round}: ");
                    prevRound = match.Round;
                }

                // Eventually, this will also include the possibility to have text inputs for score, so, probably we'll be returning a message component instead
                stringBuilder.AppendLine($"{match.HomeTeam.BracketDisplay} VS {match.AwayTeam.BracketDisplay}");
            }

            return stringBuilder.ToString();
        }
    }
}
