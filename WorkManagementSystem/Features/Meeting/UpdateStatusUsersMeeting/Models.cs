namespace WorkManagementSystem.Features.Meeting.UpdateStatusUsersMeeting
{
    public class Request
    {
        public Guid UserId { get; set; } // người đc mời
        public Guid OrganizerId { get; set; } // người tổ chức

        public StatusUserMeeting StatusUserMeeting { get; set; }
        public Guid MeetingId { get; set; }
        public TypeMeeting TypeMeeting { get; set; }
    }
    public class Response
    {
        public string message { get; set; } = string.Empty;
    }
}
