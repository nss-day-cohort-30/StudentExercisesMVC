using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models
{
    public class Cohort
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        [Display(Name="Cohort")]
        public string Name { get; set; }
    }
}
