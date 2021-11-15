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

        private readonly ILogger<PersonController> _logger;

        // Initialise DAL class
        private PersonDAL personContext = new PersonDAL();

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        // Return area interest list for dropdown
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

        // Return area interest list for dropdown
        private List<SelectListItem> GetExType()
        {
            List<SelectListItem> exTypeList = new List<SelectListItem>();
            List<String> myExTypeList = new List<string>() { "Sedentary", "Lightly Active", "Moderately Active", "Active", "Very Active" };
            List<String> myExDescList = new List<String>() { "little-no exercie", "exercise 1–3 days/week", "exercise 3–5 days/week", "exercise 6–7 days/week", "hard exercise 6–7 days/week" };
            for(int i=0; i<myExTypeList.Count; i++)
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
            ViewData["GenderList"] = GetGender();
            ViewData["ExTypeList"] = GetExType();
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
                int age = 0;
                age = DateTime.Now.Year - userCalories.BirthDay.Year;
                if (DateTime.Now.DayOfYear < userCalories.BirthDay.DayOfYear)
                {
                    age--;
                }

                if (userCalories.Gender == "Female")
                {
                    BMR = 655.1 + (9.563 * userCalories.Weight) + (1.850 * userCalories.Height) - (4.676 * age);
                }
                else
                {
                    BMR = 66.47 + (13.75 * userCalories.Weight) + (5.003 * userCalories.Height) - (6.755 * age);
                }
                HttpContext.Session.SetInt32("AMR", (int)CalculateAMR(BMR, userCalories.ExerciseType));

                ViewData["GenderList"] = GetGender();
                ViewData["ExTypeList"] = GetExType();
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
