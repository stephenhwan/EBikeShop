using MediatR;
namespace PollBuilder.Application.Features.Polls.Commands.CreatePoll
{

	// 1. Khai báo Command (Dữ liệu đầu vào)
	// IRequest<Guid> được chọn đơn giản là để thông báo rằng kết quả của việc xử lý CreatePollCommand là một Guid
	public record CreateOptionCommand(
	string OptionText,
	int Position
	);
	public record CreateQuestionCommand(
	string QuestionText,
	int Position,
	List<CreateOptionCommand> Options
	);
	public record CreatePollCommand(
	string Title,
		DateTime StartAt,
		DateTime EndAt,
	List<CreateQuestionCommand> Questions

	) : IRequest<string>;


}
