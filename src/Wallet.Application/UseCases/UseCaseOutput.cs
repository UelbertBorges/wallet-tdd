namespace Wallet.Application.UseCases
{
    public abstract class UseCaseOutput<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }

    public class ErrorUseCaseOutput<T> : UseCaseOutput<T>
    {
        public ErrorUseCaseOutput(string message)
        {
            Success = false;
            Message = message;
        }
    }

    public class SuccessUseCaseOutput<T> : UseCaseOutput<T>
    {
        public SuccessUseCaseOutput(T data)
        {
            Success = true;
            Data = data;
        }
    }
}
