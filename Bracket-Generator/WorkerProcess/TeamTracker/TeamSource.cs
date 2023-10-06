using Discord;
using Discord.WebSocket;
using WorkerProcess.Models;

namespace WorkerProcess.TeamTracker;
public sealed class TeamSource
{
    public void AddChannel(ulong channelId, IUser organizer)
    {
        _playersPerTeam[channelId] = 0;
        _usersPlaying[channelId] = new List<IUser>();
        _tournamentOrganizer[channelId] = organizer;
    }
    public IUser GetOrganizer(ulong channelId)
    {
        return _tournamentOrganizer[channelId];
    }

    public void ClearChannel(ulong channelId)
    {
        _playersPerTeam.Remove(channelId);
        _usersPlaying.Remove(channelId);
        _tournamentOrganizer.Remove(channelId);
    }

    public void AddUser(ulong channelId, IUser user)
    {
        _usersPlaying[channelId].Add(user);
    }

    public void RemoveUser(ulong channelId, IUser user)
    {
        _usersPlaying[channelId].Remove(user);
    }

    public bool ContainsInfo(ulong channelId)
    {
        return _playersPerTeam.ContainsKey(channelId);
    }

    public List<IUser> GetPlayers(ulong channelId)
    {
        return _usersPlaying[channelId];
    }

    public void SetPlayersPerTeam(ulong channelId, int number)
    {
        _playersPerTeam[channelId] = number;
    }

    public int PlayersPerTeam(ulong channelId)
    {
        return _playersPerTeam[channelId];
    }

    // Todo, put the team creation here.
    public List<Team> GenerateTeams(ulong channelId)
    {
        return new List<Team>();
    }
    
    public bool UserAlreadyPlaying(ulong channelId, SocketUser user)
    {
        return _usersPlaying[channelId].Contains(user);
    }

    private Dictionary<ulong, int> _playersPerTeam = new();
    private Dictionary<ulong, List<IUser>> _usersPlaying = new();
    private Dictionary<ulong, IUser> _tournamentOrganizer = new();

    // Singleton stuff
    private TeamSource() {}

    private static readonly Lazy<TeamSource> lazy = new(() => new TeamSource());

    public static TeamSource Instance => lazy.Value;

}
