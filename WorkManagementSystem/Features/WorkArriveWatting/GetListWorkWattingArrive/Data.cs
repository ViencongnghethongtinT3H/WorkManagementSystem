namespace WorkManagementSystem.Features.WorkArriveWatting.GetListWorkWattingArrive
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ListResultModel<WorkArriveResponse>> GetListWorkDispatchWattingWork(InputRequest request)
        {
            var workArrivedRepo = _unitOfWork.GetRepository<Entities.WorkArriveWatting>();
            var receiveCompanyRepo = _unitOfWork.GetRepository<Entities.ReceiveCompany>();
            var dispatchReceiveCompanyRepo = _unitOfWork.GetRepository<DispatchReceiveCompany>();
            var settingRepo = _unitOfWork.GetRepository<Entities.Setting>();
            var depaRepo = _unitOfWork.GetRepository<Entities.Department>();
            var userRepo = _unitOfWork.GetRepository<Entities.User>();
            // Truy vấn để lấy danh sách DispatchIds


            var receiveCompanyIds = from d in receiveCompanyRepo.GetAll().AsNoTracking().Where(x => x.AccountReceiveId == request.UserId)
                              select d.Id;
            var dispatchIds = dispatchReceiveCompanyRepo.GetAll().AsNoTracking().Where(x => receiveCompanyIds.Contains(x.AccountReceiveId)).Select(x=>x.WorkDispatchId);
            // Truy vấn WorkDispatchs dựa trên danh sách DispatchIds
            var query = from w in workArrivedRepo.GetAll().AsNoTracking()
                        join s in settingRepo.GetAll().AsNoTracking() on w.Notation equals s.Key
                        join de in depaRepo.GetAll().AsNoTracking() on w.DepartmentId equals de.Id
                        join u in userRepo.GetAll().AsNoTracking() on w.LeadershipDirectId equals u.Id
                        where w.WorkflowStatus == WorkflowStatusEnum.WaittingWorkArrived && dispatchIds.Contains(w.Id)
                        select new WorkArriveResponse
                        {
                            WorkArriveWattingId = w.Id,
                            IndustryName = s.Value,
                            Content = w.Content,
                            SignDay = w.SignDay.ToFormatString("dd/MM/yyyy"),
                            Subjective = w.Subjective,
                            Notation = $"{w.ItemId}/{s.Value}",
                            UserSign = w.UserSign,
                            WorkflowStatus = w.WorkflowStatus.GetDescription(),
                            DateIssued = w.DateIssued.ToFormatString("dd/MM/yyyy"),
                            Dealine = w.Dealine.ToFormatString("dd/MM/yyyy"),
                            DepartmentCompile = w.DepartmentCompile,
                            DocumentTypeKey = w.DocumentTypeKey,
                            EvictionTime = w.EvictionTime.ToFormatString("dd/MM/yyyy"),
                            KeyWord = w.KeyWord,
                            Priority = w.Priority,
                            TransferType = w.TransferType.GetDescription(),
                            UserCompile = w.UserCompile,
                            WorkItemNumber = w.WorkItemNumber,
                            DepartmentName = de.Name,
                            LeadershipDirectName = u.Name,
                            Created = w.Created,

                        };
            //var fromDate = request.Filters.GetFilterModel("FromDate");
            //var toDate = request.Filters.GetFilterModel("ToDate");
            //if (fromDate is not null && toDate is not null)
            //{
            //    var fromValue = fromDate.FieldValue.ParseDateTimeNotNull(false, "dd/MM/yyyy");
            //    var toValue = toDate.FieldValue.ParseDateTimeNotNull(false, "dd/MM/yyyy");
            //    query = query.Where(x => x.Created > fromValue && x.Created <= toValue);
            //}

            var data = new Response
            {
                Count = await query.CountAsync(),
                Items = await FilterActivitiesRepresentativeDetail(request, query)
            };
            return ListResultModel<WorkArriveResponse>.Create(data.Items, data.Count, request.Page, request.PageSize);

        }
        private async Task<List<WorkArriveResponse>> FilterActivitiesRepresentativeDetail(InputRequest r, IQueryable<WorkArriveResponse> query)
        {
            query = EntityQueryFilterHelper.CreateSort<WorkArriveResponse>(r.Sorts)(query);
            query = EntityQueryFilterHelper.Page<WorkArriveResponse>(r.Page, r.PageSize)(query);
            return await query.ToListAsync();
        }
    }
}
