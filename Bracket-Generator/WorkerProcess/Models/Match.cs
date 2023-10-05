namespace WorkerProcess.Models;
public class Match
{
    public Match(Team homeTeam, Team awayTeam, string round)
    {
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        Round = round;
    }

    public Team HomeTeam;
    public Team AwayTeam;
    public string Round;

    public List<KeyValuePair<Team, int>> FinalScore;
}
