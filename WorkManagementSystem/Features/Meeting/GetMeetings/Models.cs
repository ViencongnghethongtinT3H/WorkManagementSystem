namespace WorkManagementSystem.Features.Meeting.GetMeetings
{
    public class Request
    {
        public string UserId { get; set; } = string.Empty;
        public RoleUserMeeting? RoleUserMeeting { get; set; }
        public int? Page { get; init; } = 1;
        public int? PageSize { get; init; } = 20;
    }
    public class Response 
    {
        public string? UserId { get; set; }
        public string? RoleUserMeetingName { get; set; }
        public RoleUserMeeting? RoleUserMeeting { get; set; }
        public string? StatusUserMeetingName { get; set; }
        public StatusUserMeeting? StatusUserMeeting { get; set; }
        public string? TypeMeetingName { get; set; }
        public TypeMeeting? TypeMeeting { get; set; }
        public FormatMeeting? FormatMeeting { get; set; }
        public string? FormatMeetingName { get; set; }
        public string? HourStart { get; set; }
        public string? HourEnd { get; set; }
        public string? DayOfMeeting { get; set; }
        public string? OrganizerId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Link {  get; set; }

    }

}
