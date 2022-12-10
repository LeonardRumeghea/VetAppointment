#nullable disable
namespace VetAppointment.Domain.Helpers
{
    public class Result
    {
        public string Error { get; private set; }
        public bool IsSuccess { get; private set; }
        public bool IsFailure { get; private set; }

        public static Result Success() => new() { IsSuccess = true };

        public static Result Failure(string error) => new() { IsFailure = true, Error = error };
    }
}
