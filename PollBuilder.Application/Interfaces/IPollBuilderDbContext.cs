using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PollBuilder.Domain.Entities;

namespace PollBuilder.Application.Interfaces
{
	public interface IPollBuilderDbContext
	{
		//DbSet<T>: Đại diện cho các bảng trong cơ sở dữ liệu.
		//Nhờ khai báo ở đây, các Handler (Query/Command) ở tầng Application có thể truy vấn dữ liệu (ví dụ: _context.Polls.Where(...)) mà không cần biết database thực tế là gì.

		//SaveChangesAsync: Dùng để commit các thay đổi(thêm, sửa, xóa) xuống database.
				DbSet<Poll> Polls { get; }
		DbSet<Question> Questions { get; }
		DbSet<Option> Options { get; }
		DbSet<Vote> Votes { get; }
		DbSet<User> Users { get; }
		Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
