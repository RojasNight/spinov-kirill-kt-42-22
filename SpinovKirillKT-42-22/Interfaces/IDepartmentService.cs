using SpinovKirillKT_42_22.Filters.DepartmentFilters;
using SpinovKirillKT_42_22.Models;

namespace SpinovKirillKT_42_22.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentFilter>> GetDepartmentsAsync(DateTime? foundedAfter = null, int? minTeacherCount = null);
        Task AddDepartmentAsync(Department department);
        Task<Department> UpdateDepartmentAsync(Department department);
        Task<bool> DeleteDepartmentAsync(int departmentId);
    }
}
