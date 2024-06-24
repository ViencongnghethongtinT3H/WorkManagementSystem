namespace WorkManagementSystem.Entities
{
    public class DispatchReceiveCompany : EntityBase
    {
        public Guid WorkDispatchId { get; set; }
        public Guid AccountReceiveId { get; set; }  // UserId của văn thư đơn vị nhận
    }
}
