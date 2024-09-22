namespace MorningPatch.Application.Features.SteamGames.Commands.Update;
using MediatR;
using MorningPatch.Application.Features.SteamGames.Commands.Update.DTOs;
using MorningPatch.Domain.Entities;

/**
 * <summary>
 * A command for updating the <see cref="SteamGame"/> entities in the database. In this case, the
 * update operation only adds new <see cref="SteamGame"/> entities not currently stored in the
 * database. It is not common for Steam users to remove games from their account.
 * </summary>
 */
public sealed class UpdateSteamGamesCommand : IRequest<UpdateSteamGamesCommandResponse>
{

}