using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Guardzilla.Modules;

public class ModerationModule : BaseCommandModule
{
    [Command("kick")]
    [Description("Removes a Discord member from the guild.")]
    [RequireUserPermissions(Permissions.KickMembers)]
    public async Task KickCommand(CommandContext context, DiscordMember member, string reason = "")
    {
        await member.RemoveAsync(reason);
    }

    [Command("ban")]
    [Description("Permanently bans a Discord member from the guild.")]
    [RequireUserPermissions(Permissions.BanMembers)]
    public async Task BanCommand(CommandContext context, DiscordMember member, string reason = "", int deleteMessageDays = 30)
    {
        await member.BanAsync(deleteMessageDays, reason);
    }
}
