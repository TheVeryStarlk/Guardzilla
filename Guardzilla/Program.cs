using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Guardzilla;

internal class Program
{
    private static async Task Main()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("settings.json", false);

                var configRoot = configBuilder.Build();
                context.Properties.Add("token", configRoot.GetSection("token").Value);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddTransient(provider => new DiscordConfiguration()
                {
                    Token = (string)context.Properties["token"],
                    TokenType = TokenType.Bot
                });

                services.AddTransient(provider => new DiscordClient(provider.GetRequiredService<DiscordConfiguration>()));

                services.AddHostedService<DiscordBot>();
            })
            .UseConsoleLifetime();

        await hostBuilder.RunConsoleAsync();
    }
}