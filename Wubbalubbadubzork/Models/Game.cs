using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wubbalubbadubzork.Models
{
    public class Game
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; } 
        [Required]
        [Display(Name = "Codigo")]
        [Range(1000, 9999, ErrorMessage = "Code must be of 4 digits.")]
        public int Code { get; set; }
        [Required]
        [ForeignKey("Server")]
        public int Server_Id { get; set; }
        public virtual Server Server { get; set; }
    }
}