namespace WorkManagementSystem.Features.WorkDispatch.GetWorkDispatchById
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<WorkDispatchDetailResponse>> GetWorkItemById(Request r)
        {
            var workRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>().GetAll();
            var settingRepo = _unitOfWork.GetRepository<Entities.Setting>().GetAll();
            var depaRepo = _unitOfWork.GetRepository<Entities.Department>().GetAll();
            var user = _unitOfWork.GetRepository<Entities.User>().GetAll();
            var dispatchReceiveCompanyRepo = _unitOfWork.GetRepository<DispatchReceiveCompany>().GetAll();
            var fileAttachs = _unitOfWork.GetRepository<FileAttach>().GetAll();
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
                              join f in fileAttachs.AsNoTracking() on w.Id equals f.IssuesId into wf
                              from b6 in wf.DefaultIfEmpty()
                              where w.Id == r.WorkDispatchId
                              select new WorkDispatchDetailResponse
                              {
                                  FileExtension = b6.FileExtension,
                                  FileName = b6.FileName,
                                  FileUrl = b6.FileUrl,
                                  SignDay = w.SignDay.ToFormatString("dd/MM/yyyy HH:mm"),
                                  DepartmentCompile = w.DepartmentId,
                                  DocumentTypeKey = w.DocumentTypeKey,
                                  WorkflowStatus = w.WorkflowStatus,
                                  ItemId = w.ItemId,
                                  TransferType = w.TransferType.GetDescription(),
                                  UserCompile = w.UserCompile,
                                  KeyWord = w.KeyWord,
                                  UserSign = w.UserSign,
                                  WorkDispatchId = w.Id,
                                  WorkItemNumber = w.WorkItemNumber,
                                  Content = w.Content,
                                  Notation = $"{w.ItemId}/{b1.Value}",
                                  DateIssued = w.DateIssued.ToFormatString("dd/MM/yyyy HH:mm"),
                                  Priority = w.Priority,
                                  Subjective = w.Subjective,
                                  UserIdCreated = w.UserIdCreated,
                                  DepartmentId = w.DepartmentId,
                                  Dealine = w.Dealine.ToFormatString("dd/MM/yyyy HH:mm"),
                                  EvictionTime = w.EvictionTime.ToFormatString("dd/MM/yyyy HH:mm"),
                                  IndustryId = b5.Value,
                                  IndustryName = b5.Value,
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

                var historyRepo = _unitOfWork.GetRepository<Entities.History>().GetAll();
                var usersRepo = _unitOfWork.GetRepository<Entities.User>().GetAll();

                var histories = await (from h in historyRepo
                                       join u in usersRepo on h.UserId equals u.Id
                                       where h.IssueId == r.WorkDispatchId
                                       orderby h.Created descending
                                       select new HistoryListModel
                                       {
                                           ActionContent = h.actionContent,
                                           ActionTime = h.ActionTime.ToFormatString("dd/MM/yyyy HH:mm"),
                                           UserUpdated = u.Name
                                       }).ToListAsync();

                work.Histories = histories;

                var nameUser = await GetUserName(work.UserCompile.Value);

                var notes = _unitOfWork.GetRepository<UserWorkflow>().GetAll().AsNoTracking()
                    .Where(p => p.WorkflowId == r.WorkDispatchId).Select(p => new Notes
                    {
                        DateNote = p.Created.ToFormatString("dd/MM/yyyy HH:mm"),
                        UserName = nameUser,
                        DeparmentName = work.DepartmentName,
                        Note = p.Note
                    });
                work.Notes = notes.ToList();
            }

            return ResultModel<WorkDispatchDetailResponse>.Create(work);
        }
        public async Task<string> GetUserName(Guid id)
        {
            var user = await _unitOfWork.GetRepository<Entities.User>().GetAsync(id);
            if (user is not null)
            {
                return user.Name;
            }
            return string.Empty;

        }
    }
}
