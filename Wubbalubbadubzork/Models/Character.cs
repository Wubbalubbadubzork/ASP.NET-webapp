using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wubbalubbadubzork.Models
{
    public class Character
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Rol")]
        public string Role { get; set; }
        [Required]
        [Display(Name = "HP maxima")]
        public int Max_HP { get; set; }
        [Required]
        [Display(Name = "Mana maxima")]
        public int Max_Mana { get; set; }
        [Required]
        [Display(Name = "Armadura")]
        public int Armor { get; set; }
        [Required]
        [Display(Name = "Daño")]
        public int Damage { get; set; }
        [Required]
        [Display(Name = "Poder")]
        public int Power { get; set; }
        [Required]
        [Display(Name = "Jugable")]
        public bool Playable { get; set; }
        [Required]
        [Display(Name = "HP")]
        public int Health { get; set; }
        [Required]
        [Display(Name = "Mana")]
        public int Mana { get; set; }
        [Required]
        [Display(Name = "Turn")]
        public bool Is_Turn { get; set; }
    }
}