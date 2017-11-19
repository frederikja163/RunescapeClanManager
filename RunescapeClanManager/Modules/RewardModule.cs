using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using RunescapeClanManager.Preconditions;
using RunescapeClanManager.DatabaseItems;
using RunescapeClanManager.DatabaseItems.Utility;

namespace RunescapeClanManager.Modules
{
    [Name("Reward")]
    [Group("Reward")]
    public class RewardModule : ModuleBase
    {
        [Command("Create")]
        [Alias("cr")]
        [Summary("Create a new reward")]
        [RequireGuildMessage]
        [RequireUserPermission(ChannelPermission.ManagePermissions)]
        public async Task CreateAsync(string name, [Remainder] string variables = "")
        {
            RunescapeGuild guild = RunescapeGuild.FindOrCreate(Context.Channel.Id);

            guild.rewards.Add(new Reward());
            List<Change.ChangeResult> results = Change.PerformChange(guild.rewards.Last(), $"name:{name} {variables}");

            EmbedBuilder builder = new EmbedBuilder()
            {
                Color = new Color(255, 255, 255),
                Description = "Reward created with following properties"
            };
            for (int i = 0; i < results.Count(); i++)
            {
                builder.AddField(x =>
                {
                    x.Name = (results[i].isSucces) ? "Succesfully set" : "Error could not set";
                    x.Value = $"{results[i].property.Name} to \"{results[i].value}\"";
                });
            }

            await ReplyAsync("", false, builder.Build());
            guild.Save();
        }

        [Command("List")]
        [Alias("li")]
        [Summary("Lists all rewards")]
        [RequireGuildMessage]
        [RequireUserPermission(ChannelPermission.ManagePermissions)]
        public async Task ListAsync()
        {
            RunescapeGuild guild = RunescapeGuild.FindOrCreate(Context.Channel.Id);
            List<Reward> rewards = guild.rewards;
            EmbedBuilder builder = new EmbedBuilder()
            {
                Color = new Color(255, 255, 255),
                Description = "This is the rewards existing on this guild"
            };
            for (int i = 0; i < rewards.Count(); i++)
            {
                builder.AddField(x =>
                {
                    x.Name = rewards[i].name;
                    x.Value = $"Is giving {rewards[i].points} points when handed out";
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}
