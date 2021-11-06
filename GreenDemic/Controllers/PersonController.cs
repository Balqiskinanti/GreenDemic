﻿using GreenDemic.DAL;
using GreenDemic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Controllers
{
    public class PersonController : Controller
    {

        private readonly ILogger<PersonController> _logger;

        // Initialise DAL class
        private PersonDAL personContext = new PersonDAL();

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        // GET: UserController
        public ActionResult Index()
        {
            int accID = (int)HttpContext.Session.GetInt32("AccID");
            List<Person> personList = personContext.GetAllPerson(accID);
            return View(personList);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person person)
        {
            try
            {
                person.UserID = personContext.Add(person);
                HttpContext.Session.Remove("AMR");
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int? id)
        {
            Person person = personContext.GetDetails(id.Value);
            return View(person);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    personContext.Update(person);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(person);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Person person = personContext.GetDetails(id.Value);
            return View(person);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Person person)
        {
            try
            {
                personContext.Delete(person.UserID);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(person);
            }
        }

        public IActionResult CalculateCalories()
        {
            return View();
        }

        // POST: UserController/CalculateCalories
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CalculateCalories(UserCalories userCalories)
        {
            try
            {
                double BMR = 0;
                if(userCalories.Gender == "Female")
                {
                    BMR = 655.1 + (9.563 * userCalories.Weight) + (1.850 * userCalories.Height) - (4.676 * userCalories.Age);
                }
                else
                {
                    BMR = 66.47 + (13.75 * userCalories.Weight) + (5.003 * userCalories.Height) - (6.755 * userCalories.Age);
                }
                HttpContext.Session.SetInt32("AMR", (int)CalculateAMR(BMR, userCalories.ExerciseType));
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        private double CalculateAMR(double BMR, string exerciseType)
        {
            double AMR = 0;
            if (exerciseType == "Sedentary" )
            {
                AMR = BMR * 1.2;
            }
            else if(exerciseType =="Lightly Active")
            {
                AMR = BMR * 1.375;
            }
            else if (exerciseType == "Moderately Active")
            {
                AMR = BMR * 1.55;
            }
            else if (exerciseType == "Active")
            {
                AMR = BMR * 1.725;
            }
            else if (exerciseType == "Very Active")
            {
                AMR = BMR * 1.9;
            }
            return AMR;
        }
    }
}
