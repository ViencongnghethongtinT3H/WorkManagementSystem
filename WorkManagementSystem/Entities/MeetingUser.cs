namespace WorkManagementSystem.Entities
{
    public class MeetingUser : EntityBase
    {
        public Guid? MeetingId { get; set; }
        public Guid UserId { get; set; } // người dùng
        public RoleUserMeeting RoleUserMeeting { get; set; }
        public StatusUserMeeting StatusUserMeeting { get; set; }

    }

}
