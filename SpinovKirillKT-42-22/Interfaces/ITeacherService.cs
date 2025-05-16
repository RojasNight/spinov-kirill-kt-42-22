using SpinovKirillKT_42_22.Filters.TeacherFilter;
using SpinovKirillKT_42_22.Models;
using SpinovKirillKT_42_22.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpinovKirillKT_42_22.Interfaces
{
    public interface ITeacherService
    {
        Task<List<TeacherFilter>> GetTeachersAsync(string departmentName = null, string degreeName = null, string positionName = null);
        Task<TeacherDto> GetTeacherByIdAsync(int id);
        Task<TeacherResponseDto> AddTeacherAsync(string firstName, string lastName, int postId, int degreeId, int? departmentId);
        Task<TeacherResponseDto> UpdateTeacherAsync(int id, string firstName, string lastName, int positionId, int degreeId, int? departmentId);
        Task<bool> DeleteTeacherAsync(int id);
    }
}
