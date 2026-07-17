using FluentValidation.Results;

namespace PollBuilder.Application.Exceptions;

public class ValidationException : Exception
{
	public IDictionary<string, string[]> Errors { get; }

	public ValidationException()
		: base("One or more validation failures have occurred.")
	{
		Errors = new Dictionary<string, string[]>();
	}

	public ValidationException(IEnumerable<ValidationFailure> failures)
		: this()
	{
		Errors = failures
			.GroupBy(x => x.PropertyName)
			.ToDictionary(
				g => g.Key,
				g => g.Select(x => x.ErrorMessage).ToArray());
	}

	public ValidationException(string message)
		: base(message)
	{
		Errors = new Dictionary<string, string[]>();
	}
}