using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollBuilder.Common.Configs;
using PollBuilder.Domain.Entities.PollBuilder;

namespace PollBuilder.Infrastructure.DbContexts.Configurations
{
	public class OptionConfiguration : IEntityTypeConfiguration<Option>
	{
		public void Configure(EntityTypeBuilder<Option> builder)
		{
			builder.ToTable("Options");

			builder.HasKey(o => o.Id);

			builder.Property(o => o.OptionText)
				.IsRequired()
				.HasMaxLength(MaxLenghs.OptionText);

			builder.Property(o => o.Position)
				.IsRequired();

			builder.HasOne(o => o.Question)
				.WithMany(q => q.Options)
				.HasForeignKey(o => o.QuestionId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasIndex(o => new { o.QuestionId, o.Position });
		}

	}
}
