using PollBuilder.Application.DTOs;
using PollBuilder.Domain.Entities;

namespace PollBuilder.Application.Interfaces
{
	public interface IPollHubService
	{
		Task BroadcastResultsAsync(string pollCode, PollResultDto results);
	}
}
