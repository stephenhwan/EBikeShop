using PollBuilder.MVC.Contracts.Requests;
using PollBuilder.MVC.Contracts.Responses;
using PollBuilder.MVC.Services.Interfaces;
namespace PollBuilder.MVC.Services.Implementations
{
	public class PollApiClient : IPollApiClient
	{
		private readonly HttpClient _httpClient;

		public PollApiClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<PollResponseDto?> GetPollAsync(string url)
		{
			var response = await _httpClient.GetAsync($"api/poll/{url}");
			if (!response.IsSuccessStatusCode) return null;
			return await response.Content.ReadFromJsonAsync<PollResponseDto>();
		}
		public async Task<List<PollSummaryResponseDto>> GetAllPollsAsync()
		{
			var response = await _httpClient.GetAsync("api/poll");
			if (!response.IsSuccessStatusCode) 
			return new List<PollSummaryResponseDto>();
			return await response.Content.ReadFromJsonAsync<List<PollSummaryResponseDto>>() ?? new();
		}

		public async Task<PollResultResponseDto?> GetPollResultAsync(string url)
		{
			var response = await _httpClient.GetAsync($"api/poll/{url}/results");

			if (!response.IsSuccessStatusCode)
				return null;

			return await response.Content.ReadFromJsonAsync<PollResultResponseDto>();
		}

		public async Task<bool> SubmitVoteAsync(string url, List<Guid> selectedOptionIds)
		{
			var response = await _httpClient.PostAsJsonAsync($"api/poll/{url}/vote", selectedOptionIds);

			return response.IsSuccessStatusCode;
		}
		public async Task<string?> CreatePollAsync(CreatePollRequestDto request)
		{
			var response = await _httpClient.PostAsJsonAsync("api/poll", request);


			// console log for debug
			if (!response.IsSuccessStatusCode)
			{
				var error = await response.Content.ReadAsStringAsync();

				throw new HttpRequestException(
					error,
					null,
					response.StatusCode);
			}
			//

			var result = await response.Content.ReadFromJsonAsync<CreatePollResponseDto>();
			return result?.Url;
		}
	}
}
