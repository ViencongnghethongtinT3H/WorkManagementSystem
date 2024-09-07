using System.Globalization;

namespace WorkManagementSystem.Features.Meeting.UpdateStatusUsersMeeting
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<string>> UpdateStatusMeeting(Request r)
        {
            var meetingRepo = _unitOfWork.GetRepository<Entities.Meeting>();
            var meeting = await meetingRepo.GetAsync(r.MeetingId);
            if(meeting is null)
            {
                return new ResultModel<string>(string.Empty)
                {
                    Data = string.Empty,
                    Status = 200,
                    ErrorMessage = "Không tìm thấy cuộc họp này!",
                    IsError = true,
                };
            }
            var userMeetingRepo = _unitOfWork.GetRepository<MeetingUser>();
            var userMeeting = await userMeetingRepo.GetAll().FirstOrDefaultAsync(p=>p.UserId == r.UserId && p.MeetingId == r.MeetingId);
            if (userMeeting is null)
            {
                return new ResultModel<string>(string.Empty)
                {
                    Data = string.Empty,
                    Status = 200,
                    ErrorMessage = $"Người dùng không có trong {r.TypeMeeting.GetDescription()}!",
                    IsError = true,
                };
            }
            userMeeting.StatusUserMeeting = r.StatusUserMeeting;
            userMeetingRepo.Update(userMeeting);
            return new ResultModel<string>(string.Empty)
            {
                Data = userMeeting.UserId.ToString(),
                Status = 200,
                ErrorMessage = $"Người dùng đã tham gia {r.TypeMeeting.GetDescription()}!",
                IsError = true,
            };
        }
    }
}
