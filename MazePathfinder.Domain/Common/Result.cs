namespace MazePathfinder.Domain.Common;
public class Result<T> where T : class
{
    private Result(T? value, bool isSuccess, string[] errors)
    {
        Value = value;
        IsSuccess = isSuccess;
        IsFailure = !isSuccess;
        Errors = errors;
    }

    public T? Value { get; }
    public bool IsSuccess { get; }
    public bool IsFailure { get; } 
    public string[] Errors { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, []);
    }

    public static Result<T> Failure(string[] errors)
    {
        return new Result<T>(null, false, errors);
    }
}
