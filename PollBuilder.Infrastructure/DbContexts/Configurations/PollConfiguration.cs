using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollBuilder.Common.Configs;
using PollBuilder.Domain.Entities;

namespace PollBuilder.Infrastructure.DbContexts.Configurations
{
	public class PollConfiguration : IEntityTypeConfiguration<Poll>
	{
		public void Configure(EntityTypeBuilder<Poll> builder)
		{
			builder.ToTable("Polls");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Title)
				.IsRequired()
				.HasMaxLength(MaxLenghs.PollTitle);

			builder.Property(p => p.Url)
				.IsRequired()
				.HasMaxLength(2048);

			builder.Property(p => p.Status)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(p => p.CreatedAt)
				.IsRequired()
				.HasDefaultValueSql("GETUTCDATE()");

			builder.Property(p => p.ClosedAt)
				.IsRequired(false);

			builder.HasIndex(p => p.Url).IsUnique();

			builder.HasOne(p => p.User)
				.WithMany()
				.HasForeignKey(p => p.UserId)
				.OnDelete(DeleteBehavior.SetNull)
				.IsRequired(false);
		}
	}
}
