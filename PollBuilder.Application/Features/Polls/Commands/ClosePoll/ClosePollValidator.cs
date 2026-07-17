using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace PollBuilder.Application.Features.Polls.Commands.ClosePoll
{
	public class ClosePollValidator : AbstractValidator<ClosePollCommand>
	{
		public ClosePollValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("Id của poll không được để trống.");
		}
	}
}
