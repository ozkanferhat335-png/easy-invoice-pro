using System;

namespace FinanceApp.Shared
{
    public sealed class Result
    {
        private Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public string Error { get; }

        public static Result Success() => new Result(true, string.Empty);

        public static Result Failure(string error)
        {
            if (string.IsNullOrWhiteSpace(error)) throw new ArgumentException("Error is required.", nameof(error));
            return new Result(false, error);
        }
    }
}
