using System;
using System.Collections.Generic;
using System.Text;

namespace PollBuilder.Application.DTOs
{
	public class OptionResultDto
	{
		public Guid Id { get; set; }
		public int Position { get; set; }
		public string OptionText { get; set; }
		public int VoteCount { get; set; }
	}
}
