namespace MorningPatch.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MorningPatch.Domain;

/**
 * <summary>
 * The Entity Framework Core entity configuration for the <see cref="SteamGame"/> entity.
 * </summary>
 */
public sealed class SteamGameConfiguration : IEntityTypeConfiguration<SteamGame>
{

	public void Configure(EntityTypeBuilder<SteamGame> builder)
	{
		builder.HasKey(steamGame => steamGame.AppId);

		builder.Property(steamGame => steamGame.Name)
			   .HasColumnName("Name")
			   .IsRequired();

		builder.Property(steamGame => steamGame.ImageIconHash)
			   .HasColumnName("ImageIconHash")
			   .IsRequired();
	}
}