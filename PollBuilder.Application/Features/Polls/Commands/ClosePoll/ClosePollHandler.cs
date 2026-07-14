using MediatR;
using Microsoft.EntityFrameworkCore;
using PollBuilder.Application.Exceptions;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities.PollBuilder;

namespace PollBuilder.Application.Features.Polls.Commands.ClosePoll
{
	public class ClosePollCommandHandler : IRequestHandler<ClosePollCommand, bool>
	{
		private readonly IPollBuilderDbContext _context;
		public ClosePollCommandHandler(IPollBuilderDbContext context)
		{
			_context = context;
		}
		public async Task<bool> Handle(ClosePollCommand request, CancellationToken cancellationToken)
		{
			// Bước A: Tìm Poll trong Database dựa vào Id được gửi
			var poll = await _context.Polls
				.FirstOrDefaultAsync(p => p.Id == request.Id,cancellationToken)
				?? throw new NotFoundException(nameof(Poll), request.Id.ToString());


			// Bước C: Cập nhật trạng thái đóng Poll
			// Cách 1 (Cơ bản): Thay đổi trực tiếp thuộc tính
			// poll.IsClosed = true; 

			// Cách 2 (Chuẩn Clean Architecture/DDD): Gọi hàm nghiệp vụ được định nghĩa trong Entity
			poll.Close();
			// Bước D: Lưu thay đổi vào Database
			await _context.SaveChangesAsync(cancellationToken);

			// Trả về true báo hiệu đã đóng Poll thành công

			return true;
		}
	}



}
