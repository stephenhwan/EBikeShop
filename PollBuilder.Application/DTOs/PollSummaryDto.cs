using System;
using System.Collections.Generic;
using System.Text;

namespace PollBuilder.Application.DTOs
{
	public class PollSummaryDto
	{
		
		public Guid Id { get; set; }
		public string Title { get; set; } = default!;
		public string Url { get; set; } = default!;
		public bool Status { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}

