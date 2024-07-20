namespace WorkManagementSystem.Entities.Enums
{
    public enum UserWorkflowType
    {
        [Description("Người theo dõi")]
        Followers = 2,  // Người theo dõi   -- nằm ở menu: theo dõi  -- lãnh đạo chỉ đạo
        [Description("Người thực hiện")]
        Implementer = 1,   // Ngưới thực hiện   -- nằm ở menu: xử lý    
        [Description("Người duyệt")]
        Submit = 3,    // nguoi duyet           -- nằm ở menu: xử lý
        [Description("Người ký")]
        Signarture = 4, // nguoi ky             -- nằm ở menu: xử lý
    }
}
