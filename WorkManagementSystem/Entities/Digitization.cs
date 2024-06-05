namespace WorkManagementSystem.Entities
{
    // bảng lưu thông tin số hoá dữ liệu
    public class Digitization : EntityBase
    {
        public OcrType OcrType { get; set; }
        public string Data { get; set; }
        public string Url { get; set; }
    }
}
