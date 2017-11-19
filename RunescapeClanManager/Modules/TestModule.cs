using Discord.Commands;
using System.Threading.Tasks;

using RunescapeClanManager.DatabaseItems;

namespace RunescapeClanManager.Modules
{
    public class TestModule : ModuleBase
    {
        
        [Command("Test")]
        [Summary("Test a command")]
        public async Task TestAsync(string changes)
        {
            RunescapeGuild guild = RunescapeGuild.FindOrCreate(Context.Guild.Id);
            guild.Save();
        }
    }
}
