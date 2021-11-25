using DSharpPlus;
using Guardzilla;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddTransient(provider => new DiscordConfiguration()
        {
            Token = context.Configuration.GetValue<string>("DiscordBot:Token")
        });

        services.AddTransient<DiscordClient>();

        services.AddHostedService<DiscordBot>();
    })
    .Build();

await host.RunAsync();
