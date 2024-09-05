using System.Globalization;

namespace WorkManagementSystem.Features.Meeting.CreateMeeting
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<string>> CreateMeeting(Request r)
        {
            var meetingRepo = _unitOfWork.GetRepository<Entities.Meeting>();
            var obj = new Entities.Meeting();
            if (string.IsNullOrEmpty(r.Id))
            {
                obj = new Entities.Meeting()
                {
                    Id = Guid.NewGuid(),
                    Content = r.Content,
                    Created = DateTime.Now, // vẫn giữ nguyên giá trị thời điểm hiện tại cho thuộc tính này
                    FormatMeeting = r.FormatMeeting,

                    // Xử lý HourStart
                    HourStart = !string.IsNullOrEmpty(r.HourStart)
        ? (DateTime.TryParseExact(r.HourStart, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime hourStart)
            ? hourStart
            : DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture))
        : DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),

                    // Xử lý HourEnd
                    HourEnd = !string.IsNullOrEmpty(r.HourEnd)
        ? (DateTime.TryParseExact(r.HourEnd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime hourEnd)
            ? hourEnd
            : DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).AddMinutes(30))
        : DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    OrganizerId = r.OrganizerId,
                    Title = r.Title,
                    TypeMeeting = r.TypeMeeting,
                    UserIdCreated = r.OrganizerId.ToString(),
                    Link = r.Link,
                };
                meetingRepo.Add(obj);
            }
            else
            {
                obj = await meetingRepo.GetAsync(Guid.Parse(r.Id));
                if (obj is null)
                {
                    return new ResultModel<string>(string.Empty)
                    {
                        Data = obj.Id.ToString(),
                        Status = 200,
                        ErrorMessage = "Không tìm thấy cuộc họp này!",
                        IsError = true,
                    };

                }
                obj.Content = r.Content;
                obj.Created = DateTime.Now;
                obj.FormatMeeting = r.FormatMeeting;
                obj.HourEnd = !string.IsNullOrEmpty(r.HourEnd) ? DateTime.ParseExact(r.HourEnd, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture) : DateTime.Now;
                obj.HourStart = !string.IsNullOrEmpty(r.HourStart) ? DateTime.ParseExact(r.HourStart, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture) : DateTime.Now.AddHours(1);
                obj.OrganizerId = r.OrganizerId;
                obj.Title = r.Title;
                obj.TypeMeeting = r.TypeMeeting;
                obj.UserIdCreated = r.OrganizerId.ToString();
                obj.Link = r.Link;
                meetingRepo.Update(obj);
            }
            // check danh sach nguoi tham gia hop
            var meetingUserRepo = _unitOfWork.GetRepository<MeetingUser>();
            var meetingUsers = meetingUserRepo.GetAll().Where(p => p.MeetingId == obj.Id);
            if (meetingUsers.IsAny())
            {
                meetingUserRepo.HardDeletes(meetingUsers.ToList());
            }
            var listUserMeeting = new List<MeetingUser>();
            listUserMeeting = r.UserMeetings.Select(item => new MeetingUser
            {
                Created = DateTime.Now,
                MeetingId = obj.Id,
                RoleUserMeeting = item.RoleUserMeeting,
                StatusUserMeeting = item.StatusUserMeeting,
                UserId = item.UserId,
                Id = Guid.NewGuid(),

            }).ToList();

            await meetingUserRepo.AddRangeAsync(listUserMeeting);
            return new ResultModel<string>(obj.Id.ToString())
            {
                Data = obj.Id.ToString(),
                Status = 200,
                ErrorMessage = "Thành công!",
                IsError = false,
            };

        }
    }
}
