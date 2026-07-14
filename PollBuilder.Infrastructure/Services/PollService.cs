using PollBuilder.Application.DTOs;
using PollBuilder.Application.Features.Polls.Commands.CreatePoll;
using PollBuilder.Application.Interfaces;
using PollBuilder.Domain.Entities;
using PollBuilder.Infrastructure.DbContexts;
namespace PollBuilder.Infrastructure.Services
{
	public class PollService
	{
		private readonly PollBuilderDbContext _context;

		public PollService(PollBuilderDbContext context)
		{
			_context = context;
		}


	}
}
