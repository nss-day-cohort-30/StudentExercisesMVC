using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class StudentEditViewModel
    {
        // A single student
        public Student Student { get; set; }

        // All cohorts
        public List<SelectListItem> Cohorts;

        // Property to hold all training sessions for selection on edit form
        [Display(Name = "Assigned Exercises")]
        public List<SelectListItem> Exercises { get; private set; }

        public List<int> SelectedExercises { get; set; }


        public StudentEditViewModel() { }
        public StudentEditViewModel(int id)
        {
            Student = StudentRepository.GetStudent(id);
            BuildCohortOptions();
            BuildExerciseOptions();
        }

        private void BuildExerciseOptions()
        {
            //var exercises = ExerciseRepository.GetExercises();
            Exercises = ExerciseRepository.GetExercises()
                .Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString(),
                    Selected = Student.AssignedExercises.Find(ex => ex.Id == e.Id) != null
                }).ToList();
        }

        public void BuildCohortOptions()
        {
            Cohorts = CohortRepository.GetCohorts()
                .Select(li => new SelectListItem
                {
                    Text = li.Name,
                    Value = li.Id.ToString()
                }).ToList();

            Cohorts.Insert(0, new SelectListItem
            {
                Text = "Choose cohort...",
                Value = "0"
            });

        }
    }
}
