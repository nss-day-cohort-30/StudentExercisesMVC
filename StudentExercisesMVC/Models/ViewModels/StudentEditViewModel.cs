using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Repositories;
using System;
using System.Collections.Generic;
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

        public StudentEditViewModel() { }
        public StudentEditViewModel(int id)
        {
            Student = StudentRepository.GetStudent(id);
            BuildCohortOptions();
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
