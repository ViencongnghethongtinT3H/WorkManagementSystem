namespace WorkManagementSystem.Features.WorkDispatch.GetListWorkDispatchWattingWork
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ListResultModel<WorkDispatchResponse>> GetListWorkDispatchWattingWork(InputRequest request)
        {
            var workDispatchRepo = _unitOfWork.GetRepository<Entities.WorkDispatch>();
            var receiveCompanyRepo = _unitOfWork.GetRepository<Entities.ReceiveCompany>();
            var dispatchReceiveCompanyRepo = _unitOfWork.GetRepository<DispatchReceiveCompany>();
            var settingRepo = _unitOfWork.GetRepository<Entities.Setting>();
            var depaRepo = _unitOfWork.GetRepository<Entities.Department>();
            var userRepo = _unitOfWork.GetRepository<Entities.User>();

            // Truy vấn để lấy danh sách DispatchIds
            var dispatchIds = from d in dispatchReceiveCompanyRepo.GetAll().AsNoTracking()
                              join re in receiveCompanyRepo.GetAll().AsNoTracking() on d.AccountReceiveId equals re.AccountReceiveId
                              select d.WorkDispatchId;

            // Truy vấn WorkDispatchs dựa trên danh sách DispatchIds
            var query = from w in workDispatchRepo.GetAll().AsNoTracking()
                        join s in settingRepo.GetAll().AsNoTracking() on w.Notation equals s.Key
                        join de in depaRepo.GetAll().AsNoTracking() on w.DepartmentId equals de.Id
                        join u in userRepo.GetAll().AsNoTracking() on w.LeadershipDirectId equals u.Id
                        where w.WorkflowStatus == WorkflowStatusEnum.WaittingWorkArrived && dispatchIds.Contains(w.Id)
                        select new WorkDispatchResponse
                        {
                            WorkDispatchId = w.Id,
                            IndustryId = s.Value,
                            IndustryName = s.Value,
                            Content = w.Content,
                            SignDay = w.SignDay.ToFormatString("dd/MM/yyyy"),
                            Subjective = w.Subjective,
                            Notation = $"{w.ItemId}/{s.Value}",
                            UserSign = w.UserSign,
                            WorkflowStatus = w.WorkflowStatus,
                            DateIssued = w.DateIssued.ToFormatString("dd/MM/yyyy"),
                            Dealine = w.Dealine.ToFormatString("dd/MM/yyyy"),
                            DepartmentCompile = w.DepartmentCompile,
                            DepartmentId = w.DepartmentId,
                            DocumentTypeKey = w.DocumentTypeKey,
                            EvictionTime = w.EvictionTime.ToFormatString("dd/MM/yyyy"),
                            ItemId = w.ItemId,
                            KeyWord = w.KeyWord,
                            LeadershipDirectId = w.LeadershipDirectId,
                            Priority = w.Priority,
                            TransferType = w.TransferType,
                            UserCompile = w.UserCompile,
                            WorkItemNumber = w.WorkItemNumber,
                            DepartmentName = de.Name,
                            LeadershipDirectName = u.Name,
                        };
            var fromDate = request.Filters.GetFilterModel("FromDate");
            var toDate = request.Filters.GetFilterModel("ToDate");
            if (fromDate is not null && toDate is not null)
            {
                var fromValue = fromDate.FieldValue.ParseDateTimeNotNull(false, "dd/MM/yyyy");
                var toValue = toDate.FieldValue.ParseDateTimeNotNull(false, "dd/MM/yyyy");
                query = query.Where(x => x.Created > fromValue && x.Created <= toValue);
            }

            var data = new Response
            {
                Count = await query.CountAsync(),
                Items = await FilterActivitiesRepresentativeDetail(request, query)
            };
            return ListResultModel<WorkDispatchResponse>.Create(data.Items, data.Count, request.Page, request.PageSize);

        }
        private async Task<List<WorkDispatchResponse>> FilterActivitiesRepresentativeDetail(InputRequest r, IQueryable<WorkDispatchResponse> query)
        {
            query = EntityQueryFilterHelper.CreateSort<WorkDispatchResponse>(r.Sorts)(query);
            query = EntityQueryFilterHelper.Page<WorkDispatchResponse>(r.Page, r.PageSize)(query);
            return await query.ToListAsync();
        }
    }
}
