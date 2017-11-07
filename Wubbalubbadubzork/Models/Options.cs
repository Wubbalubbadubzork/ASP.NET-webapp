using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wubbalubbadubzork.Models
{
    public class Options
    {
        [Required]
        public int id { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string description { get; set; }
        [Required]
        [Display(Name = "Numero")]
        public int number { get; set; }
        [Required]
        [Display(Name = "ID de Escena siguiente")]
        public int next_scene_id { get; set; }
        [Required]
        [ForeignKey("Scene")]
        public int scene_id { get; set; }
        public virtual Scene Scene { get; set; }
       
    }
}