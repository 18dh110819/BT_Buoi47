public class ResponseType<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T? DataResponse { get; set; }
    public DateTime Timestamp { get; set; }
}
