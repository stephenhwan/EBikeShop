using System;
using System.Collections.Generic;
using System.Text;

namespace PollBuilder.Application.Interfaces
{
	public interface ICurrentUserService
	{
		string UserId { get; }
	}
}
