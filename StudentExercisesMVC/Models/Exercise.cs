using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models
{
    public class Exercise
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        [Required]
        [StringLength(80)]
        public string Language { get; set; }
    }
}