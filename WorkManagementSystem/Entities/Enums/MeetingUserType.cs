namespace WorkManagementSystem.Entities.Enums
{
    public enum RoleUserMeeting
    {
        [Description("Người tổ chức")]
        Organizer = 1,
        [Description("Người tham gia")]
        Attendee = 2
    }
    public enum StatusUserMeeting
    {
        [Description("Đang chờ")]
        Pending = 1,
        [Description("Đồng ý")]
        Accepted = 2,
        [Description("Từ chối")]
        Declined = 2
    }

    public enum TypeMeeting
    {
        [Description("Cuộc họp")]
        Meet = 1,
        [Description("Lịch làm việc")]
        WorkSchedule = 2,
        [Description("Nhắc nhở")]
        Note = 3,
    }
    public enum FormatMeeting
    {
        [Description("Online")]
        Online = 1,
        [Description("Offline")]
        Offline = 2,
    }
}
