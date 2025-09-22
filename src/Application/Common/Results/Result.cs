namespace Application.Common.Results;

public sealed record Error(string Code, string Description);

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public Error? Error { get; }

    private Result(bool success, T? value, Error? error) => (IsSuccess, Value, Error) = (success, value, error);

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string code, string description) => new(false, default, new Error(code, description));
}