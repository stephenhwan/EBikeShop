using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollBuilder.Common.Configs;
using PollBuilder.Domain.Entities.PollBuilder;

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
				.HasMaxLength(255);

			builder.Property(p => p.Url)
				.IsRequired()
				.HasMaxLength(500);

			builder.Property(p => p.Status)
				.IsRequired();

			builder.Property(p => p.CreatedAt)
				.IsRequired();

			builder.Property(p => p.ClosedAt);

			builder.Property(p => p.StartAt);

			builder.Property(p => p.EndAt);

			builder.Property(p => p.UserId)
				.IsRequired();

			// ⭐ QUAN TRỌNG: chặn EF Core map IsOpen thành cột
			builder.Ignore(p => p.IsOpen);

			builder.HasMany(p => p.Questions)
				.WithOne()
				.HasForeignKey("PollId")
				.OnDelete(DeleteBehavior.Cascade);

		}
	}
}
