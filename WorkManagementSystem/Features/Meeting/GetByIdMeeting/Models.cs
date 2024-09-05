namespace WorkManagementSystem.Features.Meeting.GetByIdMeeting
{
    public class Request
    {
        public Guid MeetingId { get; set; }
        public string UserId { get; set; } = string.Empty;

    }
    public class Response 
    {
        public Guid? Id { get; set; }       
        public string? FormatMeeting { get; set; }
        public string? TypeMeeting { get; set; }
        public string? HourStart { get; set; }
        public string? HourEnd { get; set; }
        public string? OrganizerId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public List<UserMeeting> UserMeetings { get; set; } = new List<UserMeeting>();
        public string? Link {  get; set; }
    }
    public class UserMeeting 
    {
        public Guid? MeetingId { get; set; }
        public Guid UserId { get; set; } // người dùng
        public string? RoleUserMeetingName { get; set; }
        public RoleUserMeeting? RoleUserMeeting { get; set; }
        public string? StatusUserMeeting { get; set; }
    }

}
