namespace WorkManagementSystem.Features.Digitization.GetListDigitization;

public class Request
{
    public OcrType OcrType { get; set; }
}
public class Response
{
    public OcrType OcrType { get; set; }
    public string Data { get; set; }
    public string Url { get; set; }
}
