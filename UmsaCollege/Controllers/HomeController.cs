﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UmsaCollege.Models;
using UmsaCollege.Infrastructure;

namespace UmsaCollege.Controllers {
    public class HomeController : Controller {

        private ICourseRepository courserepository;
        private IStudentRepository studentrepository;

        public HomeController(ICourseRepository repocourse, IStudentRepository repostudent) {
            courserepository = repocourse;
            studentrepository = repostudent;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult DisplayPage() {
            ViewBag.Title = "Display";
            return View(new CourseListViewModel {
                Courses = courserepository.Courses
                    .OrderBy(p => p.CourseID)
            });
        }

        [HttpPost]
        public IActionResult DisplayPage(ListViewModel course) {
            ViewBag.Title = "Display";
            courserepository.Update(course.Courses);
            return View(new CourseListViewModel {
                Courses = courserepository.Courses
                    .OrderBy(p => p.CourseID)
            });
        }

        [HttpPost]
        public IActionResult DeleteCourse(int id) {
            ViewBag.Title = "Display";
            var course = courserepository.GetById(id);
            courserepository.Delete(course);
            return View("DisplayPage", new CourseListViewModel {
                Courses = courserepository.Courses
                    .OrderBy(p => p.CourseID)
            });
        }

        public IActionResult InsertPage() {
            ViewBag.Title = "Insert";
            return View();
        }

        [HttpPost]
        public IActionResult InsertPage(Course course) {
            if (ModelState.IsValid) {
                courserepository.SaveCourse(course);
                return RedirectToAction("DisplayPage");
            } else {
                return View("DisplayPage");
            }
        }

        public IActionResult DataPage() {
            ViewBag.Title = "Data";
            return View();
        }

        public IActionResult ShowCourse(Course course) {
            ViewBag.Title = "Data";
            ListViewModel listV = new ListViewModel {
                Courses = course,
                Students = studentrepository.Students
            };
            return View("DataPage",listV);
        }

        [HttpPost]
        public IActionResult DataPage(int id) {
            Course course = courserepository.GetById(id);
            return RedirectToAction("ShowCourse", course);
        }

        public IActionResult UserPage() {
            ViewBag.Title = "User";
            return View();
        }

        [HttpPost]
        public IActionResult UserPage(int id) {
            ViewBag.Title = "User";
            Student student = new Student {
                Name = "Foo",
                LastName = "Foo",
                StudentCode = "Foo",
                Gender = 'F',
                CourseID = id
            };
            return View("UserPage", student);
        }

        [HttpPost]
        public IActionResult AddStudent(Student student) {
            if (ModelState.IsValid) {
                studentrepository.SaveStudent(student);
                Course course = courserepository.GetById(student.CourseID);
                return RedirectToAction("ShowCourse", course);
            } else {
                return View("Index");
            }
        }
    }
}
