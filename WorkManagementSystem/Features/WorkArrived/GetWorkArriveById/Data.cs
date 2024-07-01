﻿namespace WorkManagementSystem.Features.WorkArrived.GetWorkArriveById
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
                              join se in settingRepo.AsNoTracking() on w.Notation equals se.Key into sew
                              from b7 in sew.DefaultIfEmpty()
                              where w.Id == r.WorkDispatchId
                              select new WorkArriveDetailResponse
                              {
                                  IndustryId = b7.Value,
                                  IndustryName = b7.Value,
                                  DocumentTypeKey = w.DocumentTypeKey,
                                  DateIssued = w.DateIssued.ToFormatString("dd/MM/yyyy"),
                                  ItemId = w.ItemId,
                                  TransferType = w.TransferType.GetDescription(),    
                                  WorkArrivedStatus = w.WorkArrivedStatus.GetDescription(),
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
           if(work is not null)
            {
                var step = from st in stepRepo.AsNoTracking()
                           join w in workRepo on st.WorkflowId equals w.Id
                           select new WorkArrivedStep { Note  = st.Note, Step = st.Step, UserConfirm = st.UserConfirm};

                work.WorkArrivedStep = await step.FirstOrDefaultAsync();
            }

            return ResultModel<WorkArriveDetailResponse>.Create(work);
        }
    }
}
