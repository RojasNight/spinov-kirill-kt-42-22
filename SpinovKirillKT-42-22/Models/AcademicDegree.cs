using System.ComponentModel.DataAnnotations;

namespace SpinovKirillKT_42_22.Models
{
    public class AcademicDegree
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
