using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentExercisesMVC.Models.ViewModels;
using StudentExercisesMVC.Repositories;

namespace StudentExercisesMVC.Controllers
{
    public class StudentsController : Controller
    {
        // GET: Students
        public ActionResult Index(string _orderBy, string _sortDirection)
        {
            string currentSort = "";

            if (_sortDirection == null && _orderBy != null)
            {
                ViewData["sortDirection"] = "desc";
                currentSort = "asc";

            } else if (_sortDirection == null && _orderBy == null) {
                _orderBy = "CohortId";
                currentSort = "asc";
                ViewData["sortDirection"] = "";

            } else if (_sortDirection == "asc") {
                ViewData["sortDirection"] = "desc";
                currentSort = "asc";

            } else if (_sortDirection == "desc") {
                ViewData["sortDirection"] = "asc";
                currentSort = "desc";
            }

            var students = StudentRepository.GetStudents(_orderBy, currentSort);
            return View(students);
        }

        // GET: Students/Details/5
        public ActionResult Details(int id)
        {
            var student = StudentRepository.GetStudent(id);
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            StudentCreateViewModel model = new StudentCreateViewModel();
            return View(model);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] StudentCreateViewModel model)
        {
            var student = StudentRepository.CreateStudent(model.Student);
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int id)
        {
            var model = new StudentEditViewModel(id);
            return View(model);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, StudentEditViewModel model)
        {
            try
            {
                model.Student.Id = id;
                StudentRepository.UpdateStudent(model.Student);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(model);
            }
        }

        // GET: Students/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            var student = StudentRepository.GetStudent(id);
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([FromForm] int id)
        {
            if (StudentRepository.DeleteStudent(id))
            {
                return RedirectToAction(nameof(Index));
            } else {
                return RedirectToAction(nameof(Details), new { id = id });
            }
        }
    }
}