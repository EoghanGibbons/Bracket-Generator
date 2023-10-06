using Discord.Commands;
using WorkerProcess.Record_Keeping;

namespace WorkerProcess.Commands;
public class PrintHistory : ModuleBase<SocketCommandContext>
{
    [Command("bHistory")]
    [Summary("Prints the game history")]
    public async Task Execute()
    {
        await Context.Message.DeleteAsync();

        var output = await RecordKeeper.Instance.PrintRecords(Context);

        await ReplyAsync(output);
    }
}
