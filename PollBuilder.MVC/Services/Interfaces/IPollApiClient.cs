using PollBuilder.MVC.Contracts.Responses;

namespace PollBuilder.MVC.Services.Interfaces
{
	public interface IPollApiClient
	{
		Task<PollResultResponseDto?> GetPollResultAsync(string url);
		Task<bool> SubmitVoteAsync(string url, List<Guid> selectedOptionIds);
	}
}
