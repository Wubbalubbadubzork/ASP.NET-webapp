using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wubbalubbadubzork.Models
{
    public class Scene
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
    }
}