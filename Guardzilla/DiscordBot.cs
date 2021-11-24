using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace Guardzilla;

public class DiscordBot : BackgroundService
{
    private readonly DiscordClient discordClient;

    public DiscordBot(DiscordClient discordClient)
    {
        this.discordClient = discordClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var commandsNext = discordClient.UseCommandsNext(new CommandsNextConfiguration()
        {
            StringPrefixes = new[] { "$" },
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
