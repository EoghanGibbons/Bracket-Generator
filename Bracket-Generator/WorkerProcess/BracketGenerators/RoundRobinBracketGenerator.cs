using Discord;
using WorkerProcess.Models;

namespace WorkerProcess.BracketGenerators;
public static class RoundRobinBracketGenerator
{
    public static Bracket CreateBracket(List<Team> teams)
    {
        var bracket = new Bracket();
        
        var halfCount = teams.Count / 2;
        var numRounds = teams.Count - 1;

        var tempTeams = new List<Team>(teams.Count);

        tempTeams.AddRange(teams.Skip(halfCount).Take(halfCount));
        tempTeams.AddRange(teams.Skip(1).Take(halfCount-1).Reverse());
        
        for (int round = 0; round < numRounds; round++)
        {
            int teamIdx = round % teams.Count;
            
            var firstTeam = tempTeams[teamIdx];
            var secondTeam = teams[0];

            bracket.Matches.Add(CreateMatchWithRandomHomeAndAway(firstTeam, secondTeam, round));

            for (int idx = 1; idx < halfCount; idx++)
            {
                firstTeam = tempTeams[(round + idx) % tempTeams.Count];
                secondTeam = tempTeams[(round + tempTeams.Count - idx) % tempTeams.Count];
                bracket.Matches.Add(CreateMatchWithRandomHomeAndAway(firstTeam, secondTeam, round));
            }
        }

        return bracket;
    }

    private static Match CreateMatchWithRandomHomeAndAway(Team firstTeam, Team secondTeam, int round)
    {
        var random = new Random();

        round = round + 1;
        int generatedNum = random.Next(0, 40);
        if (generatedNum % 2 == 0)
        {
            return new Match(firstTeam, secondTeam, round.ToString());
        }
        else
        {
            return new Match(secondTeam, firstTeam, round.ToString());
        }
    }

    private static List<Match> GetRound(List<Team> groupA, List<Team> groupB, string round)
    {
        var total = new List<Match>();

        for (int i = 0; i < groupA.Count; i++)
        {
            total.Add(new Match(groupA.ElementAt(i), groupB.ElementAt(i), round));
        }

        return total;
    }
}
