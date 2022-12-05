#nullable disable
namespace VetAppointment.Domain.Helpers
{
    public class Result<TEntity>
    {
        public TEntity Entity { get; private set; }
        public string Error { get; private set; }
        public bool IsSuccess { get; private set; }
        public bool IsFailure { get; private set; }

        public static Result<TEntity> Success(TEntity entity) => new() { Entity = entity, IsSuccess = true };

        public static Result<TEntity> Failure(string error) => new() { IsFailure = true, Error = error };
    }
}
