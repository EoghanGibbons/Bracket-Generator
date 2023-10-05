using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using WorkerProcess.Helpers;

namespace WorkerProcess.Models;
public class Team
{
    public Team(string teamName, List<IUser> users)
    {
        _teamName = teamName;
        _users = users;
    }

    private string _teamName;
    private List<IUser> _users;
    public bool IsDummy => _users.Count == 0;

    public override string ToString()
    {
        return $"{_teamName}";
    }

    public string DisplayName
    {
        get
        {
            return $"**{_teamName}** ({string.Join(',', _users.Select(m => $" *{m.Username}*")).Trim()})";
        }
    }

    public string BracketDisplay => $"**{_teamName}**";
}
