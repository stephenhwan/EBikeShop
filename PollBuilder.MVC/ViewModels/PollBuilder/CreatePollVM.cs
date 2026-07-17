using System.ComponentModel.DataAnnotations;

namespace PollBuilder.MVC.ViewModels.PollBuilder
{
	public class OptionVM
	{
		[Required(ErrorMessage = "Option text is required.")]
		public string OptionText { get; set; }
		public int Position { get; set; }
	}

	public class QuestionVM
	{
		[Required(ErrorMessage = "Question text is required.")]
		public string QuestionText { get; set; }
		public int Position { get; set; }

		[MinLength(2, ErrorMessage = "Each question must have at least 2 options.")]
		public List<OptionVM> Options { get; set; } = new();
	}

	public class CreatePollVM
	{
		[Required(ErrorMessage = "Poll title is required.")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Start time is required.")]
		public DateTime StartAt { get; set; }

		[Required(ErrorMessage = "End time is required.")]
		public DateTime EndAt { get; set; }

		[MinLength(1, ErrorMessage = "The poll must have at least 1 question.")]
		public List<QuestionVM> Questions { get; set; } = new();
	}
}