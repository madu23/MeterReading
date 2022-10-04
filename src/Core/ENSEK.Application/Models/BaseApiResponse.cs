namespace ENSEK.Application.Models
{
    public class BaseResponse<T> where T : class
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T ResultData { get; set; }
        public List<ErrorModel> Errors { get; set; }
    }
}
