namespace WorkManagementSystem.Features.Meeting.GetByIdMeeting
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<Response>> GetById(Request r)
        {
            var meetingRepo = _unitOfWork.GetRepository<Entities.Meeting>();
            var meetingUserRepo = _unitOfWork.GetRepository<MeetingUser>();
            var response = await (from b in meetingRepo.GetAll()
                              select new Response
                              {
                                  Id = b.Id,
                                  HourStart = b.HourStart.ToFormatString("dd/MM/yyyy hh:mm"),
                                  HourEnd = b.HourEnd.ToFormatString("dd/MM/yyyy hh:mm"),
                                  Content = b.Content,
                                  OrganizerId = b.OrganizerId.ToString(),
                                  Title = b.Title,
                                  TypeMeeting = b.TypeMeeting.GetDescription(),
                                  FormatMeeting = b.FormatMeeting.GetDescription(),
                              }).FirstOrDefaultAsync(p=>p.OrganizerId == r.UserId && p.Id == r.MeetingId);
            if(response is not null)
            {
                var userMeetings = await meetingUserRepo.GetAll().AsNoTracking().Where(p => p.MeetingId == r.MeetingId).ToListAsync();
                if (userMeetings.IsAny())
                {
                    var lstUserMeeting = userMeetings.Select(item => new UserMeeting 
                    {
                        MeetingId = item.MeetingId,
                        StatusUserMeeting = item.StatusUserMeeting.GetDescription(),
                        RoleUserMeetingName = item.RoleUserMeeting.GetDescription(),
                        RoleUserMeeting = item.RoleUserMeeting,
                        UserId = item.UserId,
                    }).ToList();

                    response.UserMeetings = lstUserMeeting;
                }
            }
           

            return ResultModel<Response>.Create(response);
            
        }
    }
}
