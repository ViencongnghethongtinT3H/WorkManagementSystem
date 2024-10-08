using WorkManagementSystem.Features.Meeting.GetByIdMeeting;
using WorkManagementSystem.Shared.Extensions;

namespace WorkManagementSystem.Features.Meeting.GetMeetings
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ListResultModel<Response>> GetListMeetings(Request r)
        {
            var meetingRepo = _unitOfWork.GetRepository<Entities.Meeting>();
            var meetingUserRepo = _unitOfWork.GetRepository<MeetingUser>();
            var response = new List<Response>();
            if (string.IsNullOrEmpty(r.UserId))
            {
                return ListResultModel<Response>.Create(response);
            }
            var meetUsers = meetingUserRepo.GetAll().AsQueryable().AsNoTracking().Where(p => p.UserId == Guid.Parse(r.UserId));
            if (!meetUsers.IsAny())
            {
                return ListResultModel<Response>.Create(response);
            }
            var meetings = meetingRepo.GetAll().AsQueryable().AsNoTracking();
            if (r.RoleUserMeeting == RoleUserMeeting.Organizer)
            {
                meetUsers = meetingUserRepo.GetAll().AsQueryable().AsNoTracking();
                meetings = meetings.Where(p => p.OrganizerId == Guid.Parse(r.UserId));
            }           
            response = await (from a in meetUsers
                              join b in meetings on a.MeetingId equals b.Id
                              select new Response
                              {
                                  Id = b.Id,
                                  RoleUserMeetingName = a.RoleUserMeeting.GetDescription(),
                                  RoleUserMeeting = a.RoleUserMeeting,
                                  UserId = a.UserId.ToString(),
                                  HourStart = b.HourStart.ToString(),
                                  HourEnd = b.HourEnd.ToString(),
                                  DayOfMeeting = b.DayOfMeeting.ToddMMyyyy(),
                                  Content = b.Content,
                                  OrganizerId = b.OrganizerId.ToString(),
                                  Title = b.Title,
                                  TypeMeetingName = b.TypeMeeting.GetDescription(),
                                  FormatMeetingName = b.FormatMeeting.GetDescription(),
                                  StatusUserMeetingName = a.StatusUserMeeting.GetDescription(),
                                  TypeMeeting = b.TypeMeeting,
                                  StatusUserMeeting = a.StatusUserMeeting,
                                  FormatMeeting = b.FormatMeeting,
                                  Link = b.Link,         
                              }).ToListAsync();
            foreach (var item in response)
            {
                var userMeetings = await meetingUserRepo.GetAll().AsNoTracking().Where(p => p.MeetingId == item.Id).ToListAsync();
                if (userMeetings.IsAny())
                {
                    var lstUserMeeting = userMeetings.Select(item => new UserMeeting
                    {
                        MeetingId = item.MeetingId,
                        StatusUserMeetingName = item.StatusUserMeeting.GetDescription(),
                        StatusUserMeeting = item.StatusUserMeeting,
                        RoleUserMeetingName = item.RoleUserMeeting.GetDescription(),
                        RoleUserMeeting = item.RoleUserMeeting,
                        UserId = item.UserId,
                    }).ToList();
                    item.UserMeetings = lstUserMeeting;
                }
                
            }
            return ListResultModel<Response>.Create(response, response.Count, r.Page.GetValueOrDefault(), r.PageSize.GetValueOrDefault());
        }
    }
}
