using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wubbalubbadubzork.Models
{
    public class Server
    {
        [Required]
        public int id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string name { get; set; }
        [Required]
        [ForeignKey("Scene")]
        public int scene_id { get; set; }
        public virtual Scene Scene { get; set; }
    }
}