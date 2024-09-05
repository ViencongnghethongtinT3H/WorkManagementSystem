namespace WorkManagementSystem.Features.Meeting.DeleteMeeting
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<bool>> DeleteMeeting(Request r)
        {
            var meetingRepo = _unitOfWork.GetRepository<Entities.Meeting>();
            var meetingUserRepo = _unitOfWork.GetRepository<MeetingUser>();
            var meeting = await meetingRepo.GetAll().FirstOrDefaultAsync(p=> p.Id == r.Id && p.OrganizerId == r.UserId); 
            if(meeting is null)
            {
                throw new Exception("Không tồn tại cuộc họp");
            }
            meetingRepo.HardDelete(meeting);
            var meetingUsers = meetingUserRepo.GetAll().Where(p => p.MeetingId == meeting.Id);
            if (meetingUsers.IsAny())
            {
                meetingUserRepo.HardDeletes(await meetingUsers.ToListAsync());
            }
            return new ResultModel<bool>(true) { Data = true,IsError = false,Status = 200, ErrorMessage = string.Empty };

        }
    }
}
