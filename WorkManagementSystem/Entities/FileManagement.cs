namespace WorkManagementSystem.Entities
{
    public class FileManagement :EntityBase
    { 
        /* Mỗi 1 user khi khởi tạo sẽ luôn 4 thư mục mặc định
         * - Thư mục chia sẻ
         * - Thư mục số hoá
         * - Công văn
         * - Nhiệm vụ
        */
        public Guid UserId { get; set; }

        [MaxLength(500)]
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public FileManagementType FileManagementType { get; set; }
    }
}
