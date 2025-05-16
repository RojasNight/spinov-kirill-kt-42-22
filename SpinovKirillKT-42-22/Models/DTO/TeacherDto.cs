using System.ComponentModel.DataAnnotations;

namespace SpinovKirillKT_42_22.Models.DTO
{
    public class TeacherDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PostId { get; set; }
        public int DegreeId { get; set; }
        public int? DepartmentId { get; set; }
    }
}
