namespace WorkManagementSystem.Entities.Enums;

public enum ContitionWorkflowEnum
{
    All = 1,    // tất cả các văn bản tương ứng với sub menu tất cả
    Proccess = 2,    // các văn bản đang xử lý
    Follow = 3,      //  Theo dõi  các văn bản mà bạn được gán việc theo dõi
    Department = 4    // Phòng ban các văn bản của phòng ban của bạn
}
