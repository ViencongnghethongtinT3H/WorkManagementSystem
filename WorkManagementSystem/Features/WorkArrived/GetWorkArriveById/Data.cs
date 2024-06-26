namespace WorkManagementSystem.Features.WorkArrived.GetWorkArriveById
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<WorkArriveDetailResponse>> GetWorkItemById(Request r)
        {
            var workRepo = _unitOfWork.GetRepository<Entities.WorkArrived>().GetAll();
            var depaRepo = _unitOfWork.GetRepository<Entities.Department>().GetAll();
            var user = _unitOfWork.GetRepository<Entities.User>().GetAll();
            var dispatchReceiveCompanyRepo = _unitOfWork.GetRepository<DispatchReceiveCompany>().GetAll();
            var stepRepo = _unitOfWork.GetRepository<WorkflowStep>().GetAll();
            var work = await (from w in workRepo.AsNoTracking()
                              join s4 in depaRepo.AsNoTracking() on w.DepartmentId equals s4.Id into sd4
                              from b2 in sd4.DefaultIfEmpty()
                              join u in user.AsNoTracking() on w.LeadershipDirectId equals u.Id into ud
                              from b3 in ud.DefaultIfEmpty()
                              join d in dispatchReceiveCompanyRepo.AsNoTracking() on w.Id equals d.WorkDispatchId into dw
                              from b4 in dw.DefaultIfEmpty()
                              join st in stepRepo.AsNoTracking() on w.Id equals st.WorkflowId into stw
                              from b6 in stw.DefaultIfEmpty()
                              where w.Id == r.WorkDispatchId
                              select new WorkArriveDetailResponse
                              {
                                  Step = b6.Step,
                                  Note = b6.Note,
                                  UserConfirm = b6.UserConfirm,
                                  DocumentTypeKey = w.DocumentTypeKey,
                                  DateIssued = w.DateIssued.ToFormatString("dd/MM/yyyy"),
                                  ItemId = w.ItemId,
                                  TransferType = w.TransferType,    
                                  WorkArrivedStatus = w.WorkArrivedStatus,
                                  WorkItemNumber = w.WorkItemNumber,
                                  Content = w.Content,
                                  Notation = w.Notation,
                                  Priority = w.Priority,
                                  DepartmentId = w.DepartmentId,
                                  Dealine = w.Dealine.ToFormatString("dd/MM/yyyy"),
                                  LeadershipDirectId = w.LeadershipDirectId,
                                  DepartmentName = b2.Name,
                                  LeadershipDirectName = b3.Name,

                              }).FirstOrDefaultAsync();
          

            return ResultModel<WorkArriveDetailResponse>.Create(work);
        }
    }
}
