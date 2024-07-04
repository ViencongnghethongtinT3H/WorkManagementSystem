namespace WorkManagementSystem.Features.WorkDispatch.GetWorkWattingById
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<WorkWattingArriveDetailResponse>> GetWorkItemById(Request r)
        {
            var workRepo = _unitOfWork.GetRepository<Entities.WorkArriveWatting>().GetAll();
            var settingRepo = _unitOfWork.GetRepository<Entities.Setting>().GetAll();
            var depaRepo = _unitOfWork.GetRepository<Entities.Department>().GetAll();
            var user = _unitOfWork.GetRepository<Entities.User>().GetAll();
            var dispatchReceiveCompanyRepo = _unitOfWork.GetRepository<DispatchReceiveCompany>().GetAll();
            
            var work = await (from w in workRepo.AsNoTracking()
                              join s3 in settingRepo.AsNoTracking() on w.Notation equals s3.Key into sd3
                              from b1 in sd3.DefaultIfEmpty()
                              join s5 in settingRepo.AsNoTracking() on w.IndustryId equals s5.Key into sd5
                              from b5 in sd5.DefaultIfEmpty()
                              join s4 in depaRepo.AsNoTracking() on w.DepartmentId equals s4.Id into sd4
                              from b2 in sd4.DefaultIfEmpty()
                              join u in user.AsNoTracking() on w.LeadershipDirectId equals u.Id into ud
                              from b3 in ud.DefaultIfEmpty()
                              join d in dispatchReceiveCompanyRepo.AsNoTracking() on w.Id equals d.WorkDispatchId into dw
                              from b4 in dw.DefaultIfEmpty()
                              where w.Id == r.WorkWattingId
                              select new WorkWattingArriveDetailResponse
                              {
                                  SignDay = w.SignDay.ToFormatString("dd/MM/yyyy HH:mm"),
                                  DepartmentCompile = w.DepartmentId,
                                  DocumentTypeKey = w.DocumentTypeKey,
                                  WorkflowStatus = w.WorkflowStatus,
                                  ItemId = w.ItemId,
                                  TransferType = w.TransferType.GetDescription(),
                                  UserCompile = w.UserCompile,
                                  KeyWord = w.KeyWord,
                                  UserSign = w.UserSign,
                                  WorkWattingArriveId = w.Id,
                                  WorkItemNumber = w.WorkItemNumber,
                                  Content = w.Content,
                                  Notation = $"{w.ItemId}/{b1.Value}",
                                  DateIssued = w.DateIssued.ToFormatString("dd/MM/yyyy HH:mm"),
                                  Priority = w.Priority,
                                  Subjective = w.Subjective,
                                  DepartmentId = w.DepartmentId,
                                  Dealine = w.Dealine.ToFormatString("dd/MM/yyyy HH:mm"),
                                  EvictionTime = w.EvictionTime.ToFormatString("dd/MM/yyyy HH:mm"),
                                  IndustryId = w.IndustryId,
                                  IndustryName = b5.Value,
                                  LeadershipDirectId = w.LeadershipDirectId,
                                  DepartmentName = b2.Name,
                                  LeadershipDirectName = b3.Name,

                              }).FirstOrDefaultAsync();
            if (work is not null)
            {
                var fileAttachs = _unitOfWork.GetRepository<FileAttach>().GetAll();
                var folderRepo = _unitOfWork.GetRepository<FileManagement>().GetAll();

                var receiveCompanyRepo = _unitOfWork.GetRepository<Entities.ReceiveCompany>().GetAll();
                var receiveCompanys = await (from re in receiveCompanyRepo.AsNoTracking()
                                             join d in dispatchReceiveCompanyRepo on re.Id equals d.AccountReceiveId
                                             where d.WorkDispatchId == r.WorkWattingId
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

                var folderIds = folderRepo.Where(p => p.Name == work.WorkItemNumber).Select(p=>p.Id);
               

                var files = _unitOfWork.GetRepository<FileAttach>().GetAll().AsNoTracking()
                  .Where(p=> folderIds.Contains(p.RefId)).Select(p=> new Files
                  {
                      FileExtension = p.FileExtension,
                      FileId = p.Id,
                      FileName = p.FileName,
                      FileUrl = p.FileUrl,
                  });
                work.Files = files.ToList();
            }
            return ResultModel<WorkWattingArriveDetailResponse>.Create(work);
        }
    }
}
