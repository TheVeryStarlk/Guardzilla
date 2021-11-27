using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Guardzilla.Models;

namespace Guardzilla.Modules;

public class ModerationModule : BaseCommandModule
{
    private readonly GuardzillaConfiguration guardzillaConfiguration;

    public ModerationModule(GuardzillaConfiguration guardzillaConfiguration)
    {
        this.guardzillaConfiguration = guardzillaConfiguration;
    }

    [Command("kick")]
    [Description("Removes a Discord member from the guild.")]
    [RequireUserPermissions(Permissions.KickMembers)]
    public async Task KickCommand(CommandContext context, DiscordMember member, string reason = "")
    {
        await member.RemoveAsync(reason);
        await context.Channel.SendMessageAsync($"{member.Id} has been kicked for: {reason}");
    }

    [Command("ban")]
    [Description("Permanently bans a Discord member from the guild.")]
    [RequireUserPermissions(Permissions.BanMembers)]
    public async Task BanCommand(CommandContext context, DiscordMember member, string reason = "", int deleteMessageDays = 30)
    {
        await member.BanAsync(deleteMessageDays, reason);
        await context.Channel.SendMessageAsync($"{member.Id} has been banned for: {reason}");
    }

    [Command("mute")]
    [Description("Temporarily mutes a Discord member.")]
    [RequireUserPermissions(Permissions.ManageRoles)]
    public async Task MuteCommand(CommandContext context, DiscordMember member, string reason = "")
    {
        await member.GrantRoleAsync(context.Guild.GetRole(guardzillaConfiguration.MuteRoleId), reason);
        await context.Channel.SendMessageAsync($"{member.Id} has been temporarily muted for: {reason} ()");
    }
}
