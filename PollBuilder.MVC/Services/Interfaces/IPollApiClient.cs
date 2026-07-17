using PollBuilder.MVC.Contracts.Requests;
using PollBuilder.MVC.Contracts.Responses;

namespace PollBuilder.MVC.Services.Interfaces
{
	public interface IPollApiClient
	{
		Task<PollResponseDto?> GetPollAsync(string url);
		Task<PollResultResponseDto?> GetPollResultAsync(string url);
		Task<bool> SubmitVoteAsync(string url, List<Guid> selectedOptionIds);
		Task<List<PollSummaryResponseDto>> GetAllPollsAsync();
		Task<string?> CreatePollAsync(CreatePollRequestDto request);
	}
}
