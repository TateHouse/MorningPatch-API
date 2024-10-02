namespace MorningPatch.API;
using Microsoft.OpenApi.Models;
using MorningPatch.API.Endpoints.SteamGameNews;
using MorningPatch.API.Endpoints.SteamGames;
using MorningPatch.Application;
using MorningPatch.Persistence;

/**
 * <summary>
 * A class to configure the API services and middleware.
 * </summary>
 */
public static class StartupExtensions
{
	/**
	 * <summary>
	 * An extension method for <see cref="WebApplicationBuilder"/> to configure the application's services.
	 * </summary>
	 */
	public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
	{
		builder.Configuration.Sources.Clear();
		builder.Configuration.AddJsonFile("AppSettings.json", false, true);
		builder.Configuration.AddJsonFile($"AppSettings.{builder.Environment.EnvironmentName}.json", true, true);
		builder.Configuration.AddUserSecrets<Program>();
		builder.Configuration.AddEnvironmentVariables();

		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		builder.Services.RegisterApplicationServices();
		builder.Services.RegisterPersistenceServices(builder.Configuration);

		builder.Services.AddCors(setupAction =>
		{
			setupAction.AddPolicy("Open",
								  policy =>
								  {
									  policy.AllowAnyOrigin();
									  policy.AllowAnyMethod();
									  policy.AllowAnyHeader();
								  });
		});

		builder.Services.AddHttpClient();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(setupAction =>
		{
			setupAction.SwaggerDoc("1.0.0",
								   new OpenApiInfo
								   {
									   Contact = new OpenApiContact
									   {
										   Name = "Tate House",
										   Email = "tatemonroehouse@gmail.com",
										   Url = new Uri("https://github.com/TateHouse")
									   },
									   Description = "An API for retrieving Steam news and patch notes.",
									   Title = "MorningPatch API",
									   Version = "1.0.0"
								   });

			setupAction.CustomSchemaIds(schemaIdSelector => schemaIdSelector.FullName);
		});

		return builder;
	}

	/**
	 * <summary>
	 * An extension method for <see cref="WebApplication"/> to configure the application's middleware.
	 * </summary>
	 */
	public static WebApplication ConfigureMiddleware(this WebApplication application)
	{
		application.UseCors("Open");

		if (application.Environment.IsDevelopment())
		{
			application.UseDeveloperExceptionPage();
			application.UseSwagger();
			application.UseSwaggerUI(setupAction =>
			{
				setupAction.SwaggerEndpoint("/swagger/1.0.0/swagger.json", "MorningPatch API 1.0.0");
			});
		}

		application.MapSteamGamesEndpoints();
		application.MapSteamGameNewsEndpoints();

		return application;
	}
}