using System.Data;

namespace BOL.Models
{
    public class ReturnObject<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;
        public List<T> ListData { get; set; } = null;
        public T SingleData { get; set; } = default(T);
    }
    public class ReturnObject
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;
        public object dataTable { get; set; } = null;
    }
}