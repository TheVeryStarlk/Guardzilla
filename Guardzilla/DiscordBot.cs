using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Guardzilla.Models;

namespace Guardzilla;

public class DiscordBot : BackgroundService
{
    private readonly DiscordClient discordClient;
    private readonly GuardzillaConfiguration guardzillaConfiguration;

    public DiscordBot(DiscordClient discordClient, GuardzillaConfiguration guardzillaConfiguration)
    {
        this.discordClient = discordClient;
        this.guardzillaConfiguration = guardzillaConfiguration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var services = new ServiceCollection()
            .AddSingleton(guardzillaConfiguration)
            .BuildServiceProvider();

        var commandsNext = discordClient.UseCommandsNext(new CommandsNextConfiguration()
        {
            StringPrefixes = new[] { guardzillaConfiguration.Prefix },
            Services = services
        });
        commandsNext.RegisterCommands(Assembly.GetExecutingAssembly());

        commandsNext.CommandErrored += (_, eventArgs) => eventArgs.Context.Message.CreateReactionAsync(DiscordEmoji.FromUnicode("⚠️"));
        commandsNext.CommandExecuted += (_, eventArgs) => eventArgs.Context.Message.CreateReactionAsync(DiscordEmoji.FromUnicode("✅"));

        await discordClient.ConnectAsync();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await discordClient.DisconnectAsync();
    }
}
