using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollBuilder.Common.Configs;
using PollBuilder.Domain.Entities;

namespace PollBuilder.Infrastructure.DbContexts.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>

	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");

			builder.HasKey(u => u.Id);

			builder.Property(u => u.UserName)
				.IsRequired()
				.HasMaxLength(MaxLenghs.UserName);

			builder.Property(u => u.Email)
				.IsRequired()
				.HasMaxLength(MaxLenghs.Email);

			builder.Property(u => u.Password)
				.IsRequired()
				.HasMaxLength(MaxLenghs.Password);

			builder.Property(u => u.CreatedAt)
				.IsRequired()
				.HasDefaultValueSql("GETUTCDATE()");

			builder.HasIndex(u => u.Email).IsUnique();
			builder.HasIndex(u => u.UserName).IsUnique();
		}
	}
}
