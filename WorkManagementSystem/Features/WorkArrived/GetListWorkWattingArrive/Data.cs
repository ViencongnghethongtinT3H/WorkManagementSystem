namespace WorkManagementSystem.Features.WorkArrived.GetListWorkDispatchWattingArrive
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
            var workArrivedRepo = _unitOfWork.GetRepository<Entities.WorkArrived>();
            var receiveCompanyRepo = _unitOfWork.GetRepository<Entities.ReceiveCompany>();
            var dispatchReceiveCompanyRepo = _unitOfWork.GetRepository<DispatchReceiveCompany>();
            var settingRepo = _unitOfWork.GetRepository<Entities.Setting>();
            var depaRepo = _unitOfWork.GetRepository<Entities.Department>();
            var userRepo = _unitOfWork.GetRepository<Entities.User>();
            var stepRepo = _unitOfWork.GetRepository<WorkflowStep>();
            // Truy vấn để lấy danh sách DispatchIds
            var dispatchIds = from d in dispatchReceiveCompanyRepo.GetAll().AsNoTracking().Where(x => x.AccountReceiveId == request.UserId)
                              select d.WorkDispatchId;

            // Truy vấn WorkDispatchs dựa trên danh sách DispatchIds
            var query = from w in workArrivedRepo.GetAll().AsNoTracking()
                        join s in settingRepo.GetAll().AsNoTracking() on w.Notation equals s.Key
                        join de in depaRepo.GetAll().AsNoTracking() on w.DepartmentId equals de.Id
                        join u in userRepo.GetAll().AsNoTracking() on w.LeadershipDirectId equals u.Id
                        join st in stepRepo.GetAll().AsNoTracking() on w.Id equals st.WorkflowId
                        where w.WorkArrivedStatus == WorkArrivedStatus.WaittingWorkArrived && dispatchIds.Contains(w.Id)
                        select new WorkArriveResponse
                        {
                            Content = w.Content,
                            Notation = $"{w.ItemId}/{s.Value}",
                            DateIssued = w.DateIssued.ToFormatString("dd/MM/yyyy hh:mm"),
                            Dealine = w.Dealine.ToFormatString("dd/MM/yyyy hh:mm"),
                            DepartmentId = w.DepartmentId,
                            DocumentTypeKey = w.DocumentTypeKey,
                            ItemId = w.ItemId,
                            LeadershipDirectId = w.LeadershipDirectId,
                            TransferType = w.TransferType,
                            WorkItemNumber = w.WorkItemNumber,
                            WorkArrivedStatus = w.WorkArrivedStatus.GetDescription(),
                            Priority = w.Priority.GetDescription(),

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
