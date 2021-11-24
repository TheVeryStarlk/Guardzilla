using DSharpPlus;
using Guardzilla;

var host = Host.CreateDefaultBuilder().ConfigureDefaults(args)
    .ConfigureServices((context, services) =>
    {
        services.AddTransient(provider => new DiscordConfiguration()
        {
            Token = (string)context.Configuration["DiscordBot:Token"],
            TokenType = TokenType.Bot
        });

        services.AddTransient(provider => new DiscordClient(provider.GetRequiredService<DiscordConfiguration>()));

        services.AddHostedService<DiscordBot>();
    })
    .Build();

await host.RunAsync();
