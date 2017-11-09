using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wubbalubbadubzork.Models
{
    public class Skill
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Base Power")]
        public int Base_Power { get; set; }
        [Required]
        [ForeignKey("Character")]
        public int Character_Id { get; set; }
        public virtual Character Character { get; set; }
    }
}