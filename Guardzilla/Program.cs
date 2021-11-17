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
            .ConfigureAppConfiguration((context, config) =>
            {
                var token = config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false).Build().GetSection("token").Value;

                context.Properties.Add("token", token);
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