using GreenDemic.DAL;
using GreenDemic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Controllers
{
    public class PersonController : Controller
    {
        // DAL context
        private PersonDAL personContext = new PersonDAL();

        // Logging
        private readonly ILogger<PersonController> _logger;
        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        // Return gender for dropdown list
        private List<SelectListItem> GetGender()
        {
            List<SelectListItem> genderList = new List<SelectListItem>();
            List<String> myGenderList = new List<string>() { "Female", "Male" };

            foreach (String g in myGenderList)
            {
                genderList.Add(
                    new SelectListItem
                    {
                        Value = g,
                        Text = g
                    });
            }
            return genderList;
        }

        // Return exercise type for dropdown list
        private List<SelectListItem> GetExType()
        {
            List<SelectListItem> exTypeList = new List<SelectListItem>();
            List<String> myExTypeList = new List<string>() { "Sedentary", "Lightly Active", "Moderately Active", "Active", "Very Active" };
            List<String> myExDescList = new List<String>() { "little-no exercie", "exercise 1–3 days/week", "exercise 3–5 days/week", "exercise 6–7 days/week", "hard exercise 6–7 days/week" };
            for (int i = 0; i < myExTypeList.Count; i++)
            {
                exTypeList.Add(
                    new SelectListItem
                    {
                        Value = myExTypeList[i],
                        Text = myExTypeList[i] + " (" + myExDescList[i] + ")"
                    });
            }
            return exTypeList;
        }

        // GET: PersonController
        public ActionResult Index()
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            int accID = (int)HttpContext.Session.GetInt32("AccID");
            List<Person> personList = personContext.GetAllPerson(accID);
            return View(personList);
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["IsFailed"] = false;
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person person)
        {
            ViewData["IsFailed"] = false;
            try
            {
                if (ModelState.IsValid)
                {
                    person.UserID = personContext.Add(person);
                    HttpContext.Session.Remove("AMR");
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(person);
                }
            }
            catch
            {
                ViewData["IsFailed"] = true;
                return View(person);
            }
        }

        // GET: PersonController/Edit/5
        public ActionResult Edit(int? id)
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            // No ID supplied
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["IsFailed"] = false;
            Person person = personContext.GetDetails(id.Value);
            return View(person);
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person)
        {
            ViewData["IsFailed"] = false;
            // Update person details
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
            // Shows error message
            catch
            {
                ViewData["IsFailed"] = true;
                return View(person);
            }
        }

        // GET: PersonController/Delete/5
        public ActionResult Delete(int? id)
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            // No ID supplied
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["IsFailed"] = false;
            Person person = personContext.GetDetails(id.Value);
            return View(person);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Person person)
        {
            ViewData["IsFailed"] = false;
            // Delete person 
            try
            {
                if (ModelState.IsValid)
                {
                    personContext.Delete(person.UserID);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(person);
                }
            }
            // Shows error message
            catch
            {
                ViewData["IsFailed"] = true;
                return View(person);
            }
        }

        // GET: PersonController/CalculateCalories
        public IActionResult CalculateCalories()
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["GenderList"] = GetGender();
            ViewData["ExTypeList"] = GetExType();
            ViewData["IsFailed"] = false;
            return View();
        }

        // POST: PersonController/CalculateCalories
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CalculateCalories(UserCalories userCalories)
        {
            ViewData["IsFailed"] = false;
            // calculate user calories 
            try
            {
                if (ModelState.IsValid)
                {
                    double BMR = 0;

                    // Calculate age based on birth year
                    int age = 0;
                    age = DateTime.Now.Year - userCalories.BirthDay.Year;
                    if (DateTime.Now.DayOfYear < userCalories.BirthDay.DayOfYear)
                    {
                        age--;
                    }

                    // Calculate basal metabolic rate based on
                    // Gender, weight, height, and age
                    if (userCalories.Gender == "Female")
                    {
                        BMR = 655.1 + (9.563 * userCalories.Weight) + (1.850 * userCalories.Height) - (4.676 * age);
                    }
                    else
                    {
                        BMR = 66.47 + (13.75 * userCalories.Weight) + (5.003 * userCalories.Height) - (6.755 * age);
                    }

                    // Calculate active metabolic rate based on BMR and current exercise/ activity level
                    int bmr = (int)CalculateAMR(BMR, userCalories.ExerciseType);


                    HttpContext.Session.SetInt32("AMR", bmr);
                    ViewData["GenderList"] = GetGender();
                    ViewData["ExTypeList"] = GetExType();
                    return RedirectToAction("Create");
                }
                else
                {
                    return View(userCalories);
                }
            }
            //Shows error message
            catch
            {
                ViewData["IsFailed"] = true;
                return View(userCalories);
            }
        }

        // Return AMR 
        // AMR represents the number of calories to consume to stay at your current weight
        private double CalculateAMR(double BMR, string exerciseType)
        {
            double AMR = 0;
            if (exerciseType == "Sedentary")
            {
                AMR = BMR * 1.2;
            }
            else if (exerciseType == "Lightly Active")
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