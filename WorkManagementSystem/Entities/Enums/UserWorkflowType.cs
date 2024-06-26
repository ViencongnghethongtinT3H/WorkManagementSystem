namespace WorkManagementSystem.Entities.Enums
{
    public enum UserWorkflowType
    {
        Implementer = 1,   // Ngưới thực hiện
        Forward = 2,   // chuyển tiếp cho người khác
        Followers = 3,  // Người theo dõi
        Combination = 4,   // người phối hợp
        Submit = 5,    // nguoi duyet
        Signarture = 6, // nguoi ky
        Presented = 7 // nguoi trinh
    }
}
