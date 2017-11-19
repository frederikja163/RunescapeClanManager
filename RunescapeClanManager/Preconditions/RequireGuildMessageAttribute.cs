using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace RunescapeClanManager.Preconditions
{
    public class RequireGuildMessageAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            return Task.FromResult((context.Guild == null) ? PreconditionResult.FromError("This command needs to be typed in a guild.") : PreconditionResult.FromSuccess());
        }
    }
}
