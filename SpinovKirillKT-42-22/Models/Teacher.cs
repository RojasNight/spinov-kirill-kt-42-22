using SpinovKirillKT_42_22.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SpinovKirillKT_42_22.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }

        [Required]

        public string FirstName { get; set; }

        [Required]

        public string LastName { get; set; }

        public int DegreeId { get; set; }

        [ForeignKey("DegreeId")]
        public virtual AcademicDegree Degree { get; set; }

        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public int? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Load> Loads { get; set; } = new List<Load>();

        public bool IsFirstNameValid()
        {

            return !string.IsNullOrEmpty(FirstName) && Regex.IsMatch(FirstName, @"^[A-ZА-Я][a-zа-яё]*$");
        }

        public bool IsLastNameValid()
        {

            return !string.IsNullOrEmpty(LastName) && Regex.IsMatch(LastName, @"^[A-ZА-Я][a-zа-яё]*$");
        }

        public bool IsValid()
        {
            return IsFirstNameValid() && IsLastNameValid();
        }
    }
}
