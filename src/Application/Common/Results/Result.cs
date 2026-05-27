namespace Application.Common.Results;

public sealed record Error(string Code, string Description);

public class Result<T>
{
    private Result(bool success, T? value, Error? error)
    {
        (IsSuccess, Value, Error) = (success, value, error);
    }

    public bool IsSuccess { get; }
    public T? Value { get; }
    public Error? Error { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null);
    }

    public static Result<T> Failure(string code, string description)
    {
        return new Result<T>(false, default, new Error(code, description));
    }
}