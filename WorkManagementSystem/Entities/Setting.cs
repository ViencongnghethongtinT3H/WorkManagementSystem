using System.ComponentModel.DataAnnotations;
using WorkManagementSystem.Entities.Enums;

namespace WorkManagementSystem.Entities
{

    public class Setting : EntityBase
    {
        [MaxLength(100)]
        [Required]
        public string Key { get; set; }
        public string Value { get; set; }
        public SettingEnum Type { get; set; }

    }

}
