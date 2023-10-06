using Discord;

namespace WorkerProcess.Models;
public class Team
{
    public Team(string teamName, List<IUser> players)
    {
        _teamName = teamName;
        _players = players;
    }

    private string _teamName;
    private List<IUser> _players;
    public bool IsDummy => _players.Count == 0;

    public override string ToString()
    {
        return $"{_teamName}";
    }

    public string DisplayName => $"**{_teamName}** ({PlayerDisplay})";

    public string BracketDisplay => $"**{_teamName}**";
    public string PlayerDisplay => $"{string.Join(',', _players.Select(m => $" *{m.Username}*")).Trim()}";
}
