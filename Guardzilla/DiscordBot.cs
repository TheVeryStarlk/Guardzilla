using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Hosting;

namespace Guardzilla;

internal class DiscordBot : IHostedService
{
    private readonly DiscordClient discordClient;

    public DiscordBot(DiscordClient discordClient)
    {
        this.discordClient = discordClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var commandsNext = discordClient.UseCommandsNext(new CommandsNextConfiguration()
        {
            StringPrefixes = new[] { "#" },
        });
        commandsNext.RegisterCommands(Assembly.GetExecutingAssembly());

        await discordClient.ConnectAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await discordClient.DisconnectAsync();
    }
}
