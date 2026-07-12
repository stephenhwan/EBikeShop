using MediatR;

namespace PollBuilder.Application.Features.Polls.Commands.ClosePoll
{
	// 1. Khai báo Command: Chỉ cần truyền Id của Poll cần đóng. 
	// Trả về kiểu 'bool' để báo việc đóng thành công hay thất bại.
	public record ClosePollCommand(Guid Id) : IRequest<bool>;


}
