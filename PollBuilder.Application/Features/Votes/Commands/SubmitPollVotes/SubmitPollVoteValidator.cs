using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace PollBuilder.Application.Features.Votes.Commands.SubmitPollVotes
{
	public class SubmitPollVoteValidator : AbstractValidator<SubmitPollVoteCommand>
	{
		public SubmitPollVoteValidator()
		{
			RuleFor(x => x.Url)
				.NotEmpty().WithMessage("Url của poll không được để trống.");

			RuleFor(x => x.UserId)
				.NotEmpty().WithMessage("Bạn cần đăng nhập để vote.");

			RuleFor(x => x.SelectedOptionIds)
				.NotEmpty().WithMessage("Bạn phải chọn ít nhất 1 lựa chọn.");

			RuleForEach(x => x.SelectedOptionIds)
				.NotEmpty().WithMessage("OptionId không hợp lệ.");
		}
	}
}
