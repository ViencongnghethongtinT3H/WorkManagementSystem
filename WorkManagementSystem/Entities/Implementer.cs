using System.ComponentModel.DataAnnotations;
using WorkManagementSystem.Entities.Enums;

namespace WorkManagementSystem.Entities;

// Luồng xử lý của văn bản
public class Implementer : EntityBase
{
    public Guid IssuesId { get; set; }   // Id của văn bản hoặc của nhiệm vụ
    public Guid UserId { get; set; }  // Id của nhân viên thực hiện
    public bool IsTaskItem { get; set; }  // True thì sẽ là task, ngc lại false sẽ là công văn
    [MaxLength(100)]
    public string? Note { get; set; }  // ý kiến xử lý
    public ProgressValueEnum ProgressValue { get; set; } = ProgressValueEnum.Progress0;  // tiến độ xử lý
}
