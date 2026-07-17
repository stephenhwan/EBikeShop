using FluentValidation;
using PollBuilder.Common.Configs;

namespace PollBuilder.Application.Features.Polls.Commands.CreatePoll
{
	public class CreateOptionCommandValidator : AbstractValidator<CreateOptionCommand>
	{
		public CreateOptionCommandValidator()
		{
			RuleFor(x => x.OptionText)
				.NotEmpty().WithMessage("Nội dung lựa chọn không được để trống.")
				.MaximumLength(MaxLenghs.OptionText).WithMessage($"Nội dung lựa chọn tối đa {MaxLenghs.OptionText} ký tự.");

			RuleFor(x => x.Position)
				.GreaterThanOrEqualTo(0).WithMessage("Vị trí lựa chọn không hợp lệ.");
		}
	}

	public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
	{
		public CreateQuestionCommandValidator()
		{
			RuleFor(x => x.QuestionText)
				.NotEmpty().WithMessage("Nội dung câu hỏi không được để trống.")
				.MaximumLength(MaxLenghs.QuestionText).WithMessage($"Nội dung câu hỏi tối đa {MaxLenghs.QuestionText} ký tự.");

			RuleFor(x => x.Position)
				.GreaterThanOrEqualTo(0).WithMessage("Vị trí câu hỏi không hợp lệ.");

			RuleFor(x => x.Options)
				.Must(o => o != null && o.Count >= 2)
				.WithMessage("Mỗi câu hỏi phải có ít nhất 2 lựa chọn.");

			RuleForEach(x => x.Options).SetValidator(new CreateOptionCommandValidator());
		}
	}

	public class CreatePollValidator : AbstractValidator<CreatePollCommand>
	{
		public CreatePollValidator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Tiêu đề poll không được để trống.")
				.MaximumLength(MaxLenghs.PollTitle).WithMessage($"Tiêu đề poll tối đa {MaxLenghs.PollTitle} ký tự.");

			RuleFor(x => x.EndAt)
				.GreaterThan(x => x.StartAt).WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu.");

			RuleFor(x => x.Questions)
				.NotEmpty().WithMessage("Poll phải có ít nhất 1 câu hỏi.");

			RuleForEach(x => x.Questions).SetValidator(new CreateQuestionCommandValidator());

			// Lưu ý: UserId không validate ở đây vì Handler đang lấy từ
			// ICurrentUserService (JWT claims), không dùng request.UserId gửi lên.
		}
	}
}
