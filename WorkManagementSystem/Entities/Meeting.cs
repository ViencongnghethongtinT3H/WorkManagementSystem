namespace WorkManagementSystem.Entities
{
    public class Meeting : EntityBase
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public TypeMeeting TypeMeeting { get; set; }
        public DateTime? HourStart { get; set; }
        public DateTime? HourEnd { get; set; }
        public Guid OrganizerId { get; set; } // id user người tổ chức
        public FormatMeeting FormatMeeting { get; set; }
        public string? Link { get; set; }

    }
}
