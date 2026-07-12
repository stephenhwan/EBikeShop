using System;
using System.Collections.Generic;
using System.Text;

namespace PollBuilder.Domain.Entities
{
	public class Option
	{
		public Guid Id { get; set; }
		public string OptionText { get; set; }
		public int Position { get; set; }

		//key
		public Guid QuestionId { get; set; }
		public virtual Question Question { get; set; }
	}
}
