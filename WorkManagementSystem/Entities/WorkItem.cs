﻿using System.ComponentModel.DataAnnotations;
using WorkManagementSystem.Entities.Enums;

namespace WorkManagementSystem.Entities
{
    // Luồng công văn
    public class WorkItem : EntityBase
    {
        [MaxLength(100)]
        public string? ItemId { get; set; }  // số
        [MaxLength(100)]
        public string? Notation { get; set; }  // ký hiệu
        public DateTime DateIssued { get; set; }  // ngày ban hành
        public DateTime DateArrival { get; set; }  // ngày đến
        public Guid DocumentTypeId { get; set; }  //  Loại văn bản link tới bảng chung setting
        public Guid DepartmentId { get; set; }  //  cơ quan ban hành
        [MaxLength(1000)]
        [Required]
        public string Content { get; set; }   // Trích yếu         
        [MaxLength(1000)]
        public string? Subjective { get; set; }   // Chuyên đề   
        [MaxLength(1000)]
        public string? KeyWord { get; set; }   // từ khoá 
        public Guid LeadershipDirectId {get; set; }   // Lãnh đạo chỉ đạo
        public PriorityEnums Priority { get; set; }  // Độ khẩn cấp
        public ProcessingStatusEnum ProcessingStatus { get; set; }  // trạng thái của công văn
        public DateTime Dealine { get; set; }  // Thời hạn xử lý
        public DateTime EvictionTime { get; set; }  // Thời hạn thu hồi
        public Guid UserId { get; set; }    // chủ trì công văn này 
        public Guid CategoryId { get; set; }    // Lĩnh vực  link tới bảng chung setting
    }
}
