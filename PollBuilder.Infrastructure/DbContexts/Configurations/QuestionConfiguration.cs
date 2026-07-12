using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollBuilder.Common.Configs;
using PollBuilder.Domain.Entities;

namespace PollBuilder.Infrastructure.DbContexts.Configurations
{
	public class QuestionConfiguration : IEntityTypeConfiguration<Question>
	{
		public void Configure(EntityTypeBuilder<Question> builder)
		{
			builder.ToTable("Questions");

			builder.HasKey(q => q.Id);

			builder.Property(q => q.QuestionText)
				.IsRequired()
				.HasMaxLength(MaxLenghs.QuestionText);

			builder.Property(q => q.Position)
				.IsRequired();

			builder.HasOne(q => q.Poll)
				.WithMany(p => p.Questions)
				.HasForeignKey(q => q.PollId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasIndex(q => new { q.PollId, q.Position });
		}

	}
}
