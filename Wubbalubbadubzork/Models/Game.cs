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
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; } 
        [Required]
        [ForeignKey("Server")]
        public Guid Server_Id { get; set; }
        public virtual Server Server { get; set; }
    }
}