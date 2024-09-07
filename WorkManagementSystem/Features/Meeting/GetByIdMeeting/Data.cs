using System.Globalization;
using WorkManagementSystem.Shared.Extensions;

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
            var response = await (from b in meetingRepo.GetAll()
                                  select new Response
                                  {
                                      Id = b.Id,
                                      HourStart = b.HourStart.ToString(),
                                      HourEnd = b.HourEnd.ToString(),
                                      DayOfMeeting = b.DayOfMeeting.ToddMMyyyy(),
                                      Content = b.Content,
                                      OrganizerId = b.OrganizerId.ToString(),
                                      Title = b.Title,
                                      TypeMeetingName = b.TypeMeeting.GetDescription(),
                                      FormatMeetingName = b.FormatMeeting.GetDescription(),
                                      TypeMeeting = b.TypeMeeting,
                                      FormatMeeting = b.FormatMeeting,
                                      Link = b.Link,
                                  }).FirstOrDefaultAsync(p => p.OrganizerId == r.UserId && p.Id == r.MeetingId);
            if (response is not null)
            {
                var meetingUserRepo = _unitOfWork.GetRepository<MeetingUser>();
                var userMeetings = await meetingUserRepo.GetAll().AsNoTracking().Where(p => p.MeetingId == r.MeetingId).ToListAsync();
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

                    response.UserMeetings = lstUserMeeting;
                }
            }
            return ResultModel<Response>.Create(response);

        }
    }
}
