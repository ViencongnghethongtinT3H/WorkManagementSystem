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
            var meetings = meetingRepo.GetAll();
            response = await (from a in meetUsers
                              join b in meetings on a.MeetingId equals b.Id
                              select new Response
                              {
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
            if (r.RoleUserMeeting != null)
            {
                response = response.Where(p => p.RoleUserMeeting == r.RoleUserMeeting).ToList();
            }
            return ListResultModel<Response>.Create(response, response.Count, r.Page.GetValueOrDefault(), r.PageSize.GetValueOrDefault());
        }
    }
}
