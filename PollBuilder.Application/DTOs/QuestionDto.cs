using System;
using System.Collections.Generic;
using PollBuilder.Domain.Entities;

namespace PollBuilder.Application.DTOs
{
	public class QuestionDto
	{
		public Guid Id { get; set; }
		public string QuestionText { get; set; }
		public int Position { get; set; }
		public List<OptionDto> Options { get; set; }

		//key
		public Guid PollId { get; set; }
		public virtual PollDto Poll { get; set; }
	}
}
