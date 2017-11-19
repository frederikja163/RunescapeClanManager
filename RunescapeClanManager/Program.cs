using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

using RunescapeClanManager.Services;

namespace RunescapeClanManager
{
    public class Program
    {
        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        IServiceProvider _services;

        private async Task StartAsync()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config.json")
                .Build();

            _services = new ServiceCollection()
                .AddSingleton<CommandService>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<LoggingService>()
                .AddSingleton<StartupService>()
                .AddSingleton(config)
                .BuildServiceProvider();

            _services.GetRequiredService<LoggingService>();
            await _services.GetRequiredService<StartupService>().StartAsync();
            _services.GetRequiredService<CommandHandler>();

            await Task.Delay(-1);
        }
    }
}
