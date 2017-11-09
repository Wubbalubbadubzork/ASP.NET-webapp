﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wubbalubbadubzork.Models
{
    public class Options
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Numero")]
        public int Number { get; set; }
        [Required]
        [Display(Name = "ID de Escena siguiente")]
        public int Next_scene_id { get; set; }
        [Required]
        [ForeignKey("Scene")]
        public int Scene_id { get; set; }
        public virtual Scene Scene { get; set; }

    }
}