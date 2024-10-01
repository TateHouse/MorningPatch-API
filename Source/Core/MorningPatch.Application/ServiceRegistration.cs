namespace MorningPatch.Application;
using Microsoft.Extensions.DependencyInjection;
using MorningPatch.Application.Abstractions.Application.SteamGameNews;
using MorningPatch.Application.Features.SteamGameNews.Services;

/**
 * <summary>
 * A class for registering services.
 * </summary>
 */
public static class ServiceRegistration
{
	/**
	 * <summary>
	 * An extension method for <see cref="IServiceCollection"/> to register the <see cref="Application"/> services.
	 * </summary>
	 */
	public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
	{
		services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		services.AddMediatR(configuration =>
		{
			configuration.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
		});

		services.AddScoped<ISteamGameNewsService, SteamGameNewsService>();

		return services;
	}
}