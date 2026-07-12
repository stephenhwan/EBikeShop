using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PollBuilder.Common.Configs;
using PollBuilder.Domain.Entities;

namespace PollBuilder.Infrastructure.DbContexts.Configurations
{
	public class VoteConfiguration : IEntityTypeConfiguration<Vote>

	{
		public void Configure(EntityTypeBuilder<Vote> builder) 
		{
			builder.ToTable("Votes");

			builder.HasKey(v => v.Id);

			builder.Property(v => v.Opinion)
				.HasMaxLength(MaxLenghs.Opinion)
				.IsRequired(false);

			builder.Property(v => v.RequiredText)
				.IsRequired()
				.HasDefaultValue(false);

			builder.Property(v => v.CreatedAt)
				.IsRequired()
				.HasDefaultValueSql("GETUTCDATE()");

			// 3 FK bắt buộc: chỉ cascade 1 nhánh (Option) để tránh
			// lỗi "multiple cascade paths" của SQL Server
			builder.HasOne(v => v.User)
				.WithMany()
				.HasForeignKey(v => v.UserId)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			builder.HasOne(v => v.Question)
				.WithMany()
				.HasForeignKey(v => v.QuestionId)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			builder.HasOne(v => v.Option)
				.WithMany()
				.HasForeignKey(v => v.OptionId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			builder.HasIndex(v => new { v.UserId, v.QuestionId })
				   .HasFilter("[IsCurrent] = 1") 
				   .IsUnique();
		}
	}
}
