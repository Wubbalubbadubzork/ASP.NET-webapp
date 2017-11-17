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
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required]
        [ForeignKey("Scene")]
        public int Scene_id { get; set; }
        public virtual Scene Scene { get; set; }
    }
}