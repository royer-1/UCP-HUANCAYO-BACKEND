namespace UCP_HUANCAYO.Dtos.Common
{
    public class ApiErrorResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "Ocurrió un error";
        public Dictionary<string, string[]>? Errors { get; set; }

        public ApiErrorResponse(string message, Dictionary<string, string[]>? errors = null)
        {
            Message = message;
            Errors = errors;
        }
    }
}
