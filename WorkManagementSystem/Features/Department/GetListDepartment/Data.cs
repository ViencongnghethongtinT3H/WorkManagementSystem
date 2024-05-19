using WorkManagementSystem.Features.Setting;

namespace WorkManagementSystem.Features.Department.GetListDepartment
{
    public class Data
    {
        private readonly IUnitOfWork _unitOfWork;
        public Data(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Reponse>> GetDepartment(OrganizationLevelEnum level, Guid? parentId = null)
        {
            var setting = _unitOfWork.GetRepository<Entities.Department>().GetAll();
            var departments = await setting
               .Where(d => d.ParentId == parentId).Select(x => new DepartmentModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   OrganizationLevel = x.OrganizationLevel,
                   ParentId = x.ParentId,
               }).ToListAsync();
            var result = new List<Reponse>();

            foreach (var department in departments)
            {
                var departmentDto = new Reponse { Department = department };
                departmentDto.Children = await GetDepartment(OrganizationLevelEnum.All, department.Id);
                result.Add(departmentDto);
            }
            return result;
        }
    }
}
