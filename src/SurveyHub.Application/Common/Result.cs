namespace SurveyHub.Application.Common;

public class Result
{
    private readonly Error? _error;
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null) throw new ArgumentException("A successful result cannot have an error.", nameof(error));
        if (!isSuccess && error is null) throw new ArgumentException("A failed result must have an error.", nameof(error));

        IsSuccess = isSuccess;
        _error = error;
    }

    public Error Error => _error ?? throw new InvalidOperationException("A successful result has no error.");

    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
}

public sealed class Result<T> : Result
{
    private readonly T? _value;

    private Result(T value) : base(true, null) => _value = value;
    private Result(Error error) : base(false, error) { }

    public T Value => IsSuccess ? _value! : throw new InvalidOperationException("A failed result has no value.");

    public static Result<T> Success(T value) => new(value);
    public static new Result<T> Failure(Error error) => new(error);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
}