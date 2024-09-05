namespace WorkManagementSystem.Features.Meeting.CreateMeeting
{
    public class Request
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public TypeMeeting TypeMeeting { get; set; }
        public string? HourStart { get; set; }
        public string? HourEnd { get; set; }
        public Guid OrganizerId { get; set; } // id user người tổ chức
        public FormatMeeting FormatMeeting { get; set; }
        public List<MeetingUsers> UserMeetings { get; set; }  = new List<MeetingUsers>();
    }
    public class MeetingUsers
    {
        public Guid UserId { get; set; } // người đc mời
        public RoleUserMeeting RoleUserMeeting { get; set; }
        public StatusUserMeeting StatusUserMeeting { get; set; }
    }
    public class Response
    {
        public string Id { get; set; } = string.Empty;
    }
}
