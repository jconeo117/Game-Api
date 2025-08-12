namespace DungeonCrawlerAPI.Models
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public List<string> Errors { get; set; } = new();

        public static ServiceResult<T> Success(T result) => new() { IsSuccess = true, Data = result };
        public static ServiceResult<T> Error(string message) => new() {IsSuccess = false, ErrorMessage = message };
        public static ServiceResult<T> NotFound(string message) => new() {IsSuccess = false, ErrorMessage = message };

    }
}
