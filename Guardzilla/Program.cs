using DSharpPlus;
using Guardzilla;
using Guardzilla.Models;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton(provider => new GuardzillaConfiguration()
        {
            Prefix = context.Configuration.GetValue<string>("Prefix"),
            MuteRoleId = ulong.Parse(context.Configuration.GetValue<string>("MuteRoleId"))
        });

        services.AddTransient(provider => new DiscordConfiguration()
        {
            Token = context.Configuration.GetValue<string>("DiscordBot:Token")
        });

        services.AddTransient<DiscordClient>();

        services.AddHostedService<DiscordBot>(); 
    })
    .Build();

await host.RunAsync();
