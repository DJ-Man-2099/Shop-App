namespace Shop.Models.Contracts;

public class OpResult<T>
{
	public const string NotFoundCode = "NotFound";
	public const string BadRequestCode = "BadRequest";
	public const string ServerErrorCode = "ServerError";
	public const string UnAuthenticatedErrorCode = "UnAuthenticatedError";
	public bool Succeeded { get; set; } = true;
	public T? Value { get; set; }
	public Dictionary<string, string>? Errors { get; set; }

	public static OpResult<T> NotFound(string msg) => ErrorOpResult(NotFoundCode, msg);
	public static OpResult<T> BadRequest(string msg) => ErrorOpResult(BadRequestCode, msg);
	public static OpResult<T> ServerError(string msg) => ErrorOpResult(ServerErrorCode, msg);
	public static OpResult<T> UnAuthenticatedError(string msg) => ErrorOpResult(UnAuthenticatedErrorCode, msg);

	protected static OpResult<T> ErrorOpResult(string opCode, string msg)
	{
		return new OpResult<T>
		{
			Succeeded = false,
			Errors = new Dictionary<string, string>{
				{opCode, msg}
			}
		};
	}
}

public class OpResult : OpResult<object>
{
	public static new OpResult NotFound(string msg) => ErrorOpResult(NotFoundCode, msg);
	public static new OpResult BadRequest(string msg) => ErrorOpResult(BadRequestCode, msg);
	public static new OpResult ServerError(string msg) => ErrorOpResult(ServerErrorCode, msg);
	public static new OpResult UnAuthenticatedError(string msg) => ErrorOpResult(UnAuthenticatedErrorCode, msg);


	protected static new OpResult ErrorOpResult(string opCode, string msg)
	{
		return new OpResult
		{
			Succeeded = false,
			Errors = new Dictionary<string, string>{
				{opCode, msg}
			}
		};
	}
}
