namespace CoursePlatform.Domain.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string Error { get; private set; } = string.Empty;
    public List<string> Errors { get; private set; } = new();

    private Result(bool isSuccess, T? value, string error, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        Errors = errors ?? new List<string>();
    }

    public static Result<T> Success(T value) => new(true, value, string.Empty);
    
    public static Result<T> Failure(string error) => new(false, default, error);
    
    public static Result<T> Failure(List<string> errors) => new(false, default, string.Empty, errors);
}

public class Result
{
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; } = string.Empty;
    public List<string> Errors { get; private set; } = new();

    private Result(bool isSuccess, string error, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        Errors = errors ?? new List<string>();
    }

    public static Result Success() => new(true, string.Empty);
    
    public static Result Failure(string error) => new(false, error);
    
    public static Result Failure(List<string> errors) => new(false, string.Empty, errors);
}
