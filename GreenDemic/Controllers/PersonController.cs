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
            //List<Person> pList = personContext.GetAllPerson(1);
            //logger.LogInformation(pList[2].DerivedMaxCal.ToString());
            //logger.LogInformation(pList[2].CalculateBMR().ToString());
        }

        // Return gender for dropdown list
        private List<SelectListItem> GetGender()
        {
            List<SelectListItem> genderList = new List<SelectListItem>();
            genderList.Add(
                new SelectListItem
                {
                    Value = "0",
                    Text = "Female"
                });
            genderList.Add(
                new SelectListItem
                {
                    Value = "1",
                    Text = "Male"
                });
            return genderList;
        }

        // Return exercise type for dropdown list
        private List<SelectListItem> GetExType()
        {
            List<SelectListItem> exTypeList = new List<SelectListItem>();
            List<String> myExTypeList = new List<string>() { "Sedentary", "Lightly Active", "Moderately Active", "Active", "Very Active" };
            List<String> myExDescList = new List<String>() { "little-no exercise", "exercise 1–3 days/week", "exercise 3–5 days/week", "exercise 6–7 days/week", "hard exercise 6–7 days/week" };
            for (int i = 0; i < myExTypeList.Count; i++)
            {
                exTypeList.Add(
                    new SelectListItem
                    {
                        Value = i.ToString(),
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
            List<PersonViewModel> personVMList = MapToPersonVM(personList);
            return View(personVMList);
        }

        public List<PersonViewModel> MapToPersonVM(List<Person> personList)
        {
            List<PersonViewModel> personVMList = new List<PersonViewModel>();
            foreach (Person person in personList)
            {
                PersonViewModel personVM = new PersonViewModel
                {
                    UserID = person.UserID,
                    UserName = person.UserName,
                    Age = person.GetAge(),
                    DerivedMaxCal = person.CalculateBMR(),
                    MaxCal = person.MaxCal.Value
                };
                personVMList.Add(personVM);
            }
            return personVMList;
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
            ViewData["GenderList"] = GetGender();
            ViewData["ExTypeList"] = GetExType();
            ViewData["IsFailed"] = false;
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person person)
        {
            ViewData["GenderList"] = GetGender();
            ViewData["ExTypeList"] = GetExType();
            ViewData["IsFailed"] = false;
            try
            {
                person.MaxCal = null;
                person.Gender = (int)person.Gender;
                person.ExType = (int)person.ExType;
                if (ModelState.IsValid)
                {
                    person.UserID = personContext.Add(person);
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
            ViewData["GenderList"] = GetGender();
            ViewData["ExTypeList"] = GetExType();
            return View(person);
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person)
        {
            ViewData["IsFailed"] = false;
            ViewData["GenderList"] = GetGender();
            ViewData["ExTypeList"] = GetExType();
            // Update person details
            try
            {
                if (ModelState.IsValid)
                {
                    person.MaxCal = null;
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
    }
}