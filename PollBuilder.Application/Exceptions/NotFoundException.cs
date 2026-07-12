namespace PollBuilder.Application.Exceptions
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string name, object key)
		: base($"{name} với khóa \"{key}\" không tồn tại.") { }
	}

	public class AlreadyVotedException : Exception
	{
		public AlreadyVotedException()
			: base("Bạn đã vote cho câu hỏi này rồi.") { }
	}
}
