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
            var settingRepo = _unitOfWork.GetRepository<Entities.Setting>().GetAll();
            var work = await (from w in workRepo.AsNoTracking()
                              join s4 in depaRepo.AsNoTracking() on w.DepartmentId equals s4.Id into sd4
                              from b2 in sd4.DefaultIfEmpty()
                              join u in user.AsNoTracking() on w.LeadershipDirectId equals u.Id into ud
                              from b3 in ud.DefaultIfEmpty()
                              join d in dispatchReceiveCompanyRepo.AsNoTracking() on w.Id equals d.WorkDispatchId into dw
                              from b4 in dw.DefaultIfEmpty()
                              join st in stepRepo.AsNoTracking() on w.Id equals st.WorkflowId into stw
                              from b6 in stw.DefaultIfEmpty()
                              join se in settingRepo.AsNoTracking() on w.Notation equals se.Key into sew
                              from b7 in sew.DefaultIfEmpty()
                              where w.Id == r.WorkDispatchId
                              select new WorkArriveDetailResponse
                              {
                                  TypeSetting = b7.Type,
                                  IndustryId = b7.Value,
                                  IndustryName = b7.Value,
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
                                  Notation = $"{w.ItemId}/{b7.Value}",
                                  Priority = w.Priority.GetDescription(),
                                  DepartmentId = w.DepartmentId,
                                  Dealine = w.Dealine.ToFormatString("dd/MM/yyyy"),
                                  LeadershipDirectId = w.LeadershipDirectId,
                                  DepartmentName = b2.Name,
                                  LeadershipDirectName = b3.Name,

                              }).FirstOrDefaultAsync();
            if (work is not null)
            {
                var receiveCompanyRepo = _unitOfWork.GetRepository<Entities.ReceiveCompany>().GetAll();
                var receiveCompanys = await (from re in receiveCompanyRepo.AsNoTracking()
                                             join d in dispatchReceiveCompanyRepo on re.Id equals d.AccountReceiveId
                                             where d.WorkDispatchId == r.WorkDispatchId
                                             orderby re.Created descending
                                             select new ReceiveCompanyModel
                                             {
                                                 AccountReceiveId = re.AccountReceiveId.Value,
                                                 Name = re.Name,
                                                 Address = re.Address,
                                                 Email = re.Email,
                                                 Fax = re.Fax
                                             }).ToListAsync();
                work.ReceiveCompanys = receiveCompanys;
            }

            return ResultModel<WorkArriveDetailResponse>.Create(work);
        }
    }
}
