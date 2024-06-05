namespace WorkManagementSystem.Entities.Enums;

public enum OcrType
{
    Other = 0,   // Không OCR
    CCCD = 1,  // CMT, căn cước công dân
    VehicleRegistrations = 2,   // Đăng ký xe
    VehicleInspection = 3,      // Đăng kiểm xe
    Vehicle = 4,                // Giấy tờ xe
    VehiclePlate = 5,            // Biển số xe
    BirthCertificate = 6       // Giấy khai sinh
}
