using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SpinovKirillKT_42_22.Models
{
    public class Discipline
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Load> Loads { get; set; } = new List<Load>();

        public bool IsValidDiscipline()
        {
            //return Regex.Match(Name, @"^[А-ЯЁ](?:[а-яё0-9.,()]*(?: [а-яё0-9.,()\-]+)*)?[а-яё0-9.,()\-]$").Success;
            return Regex.Match(Name, @"^(?!.*([а-яё])\1\1)(?! )[А-ЯЁ](?:[а-яё]*(?: [а-яё]+)*)$").Success;
        }
    }
}
