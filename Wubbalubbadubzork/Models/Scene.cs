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
        public int id { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string description { get; set; }
    }
}