using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class StudentCreateViewModel
    {
        // A single student
        public Student Student { get; set; } = new Student();

        // All cohorts
        public List<SelectListItem> Cohorts;

        public StudentCreateViewModel()
        {
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
