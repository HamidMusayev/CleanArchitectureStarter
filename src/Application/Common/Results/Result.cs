namespace Application.Common.Results;

public enum ResultStatus
{
    Ok,
    BadRequest,
    NotFound,
    Unauthorized,
    Forbidden,
    Conflict,
    Validation
}

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}

public class Result
{
    protected Result(bool isSuccess, Error error, ResultStatus status)
    {
        if (isSuccess && error != Error.None) throw new InvalidOperationException("Success result cannot have an error.");
        if (!isSuccess && error == Error.None) throw new InvalidOperationException("Failure result must have an error.");
        IsSuccess = isSuccess;
        Error = error;
        Status = status;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public ResultStatus Status { get; }

    public static Result Success() => new(true, Error.None, ResultStatus.Ok);

    public static Result Failure(string code, string description, ResultStatus status = ResultStatus.BadRequest)
        => new(false, new Error(code, description), status);

    public static Result NotFound(string code, string description)
        => new(false, new Error(code, description), ResultStatus.NotFound);

    public static Result Unauthorized(string code, string description)
        => new(false, new Error(code, description), ResultStatus.Unauthorized);

    public static Result Conflict(string code, string description)
        => new(false, new Error(code, description), ResultStatus.Conflict);
}

public sealed class Result<T> : Result
{
    private Result(T value) : base(true, Error.None, ResultStatus.Ok) => Value = value;
    private Result(Error error, ResultStatus status) : base(false, error, status) => Value = default;

    public T? Value { get; }

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Failure(string code, string description, ResultStatus status = ResultStatus.BadRequest)
        => new(new Error(code, description), status);

    public static new Result<T> NotFound(string code, string description)
        => new(new Error(code, description), ResultStatus.NotFound);

    public static new Result<T> Unauthorized(string code, string description)
        => new(new Error(code, description), ResultStatus.Unauthorized);

    public static new Result<T> Conflict(string code, string description)
        => new(new Error(code, description), ResultStatus.Conflict);
}
