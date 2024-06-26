﻿namespace WorkManagementSystem.Features.File.UploadFile
{
    public class Request
    {
        public List<IFormFile> Files { get; set; }
        public OcrType OcrType { get; set; } = OcrType.Other;
    }

    public class FileAttachModel
    {
        public Guid? IssuesId { get; set; }   // Link tới công văn hay nhiệm vụ
        [MaxLength(100)]
        public string FileName { get; set; }  // tên phòng ban
        [MaxLength(100)]
        public string FileUrl { get; set; }  // tên phòng ban
        [MaxLength(50)]
        public string? FileExtension { get; set; }  // tên phòng ban
    }
    public class FileInfo
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }

    public class Response
    {
        public string Data { get; set; }
        public string FileUrl { get; set; }
        public List<Guid> ids { get; set; }
    }
}
