using GreenDemic.DAL;
using GreenDemic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Initialise DAL class
        private AccountDAL accountContext = new AccountDAL();
        private PersonDAL personContext = new PersonDAL();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: HomeController/Login
        [HttpPost]
        public ActionResult Login(IFormCollection formData)
        {
            // Read inputs from textboxes
            string username = formData["txtEmail"].ToString().ToLower();
            string password = formData["txtPassword"].ToString();

            bool isUser = false;

            foreach (Account a in accountContext.GetAllAccounts())
            {
                if (username == a.UserName.ToLower() && password == a.Pass_word)
                {
                    HttpContext.Session.SetString("AccName", a.AccName);
                    HttpContext.Session.SetInt32("AccID", a.AccID);
                    HttpContext.Session.SetString("Role", "User");

                    isUser = true;
                    break;
                }
            }

            if (isUser == true)
            {
                return RedirectToAction("Main");
            }

            else
            {
                TempData["Message"] = "Invalid Login Credentials!";
                return RedirectToAction("Login");
            }
        }

        public IActionResult SignUp()
        {
            return View();
        }

        // POST: HomeController/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(Account account)
        {
            if (ModelState.IsValid)
            {
                account.AccID = accountContext.Add(account);
                return RedirectToAction("Index");
            }
            else
            {
                return View(account);
            }
        }

        // GET: HomeController/Main
        public IActionResult Main()
        {
            int accID = (int)HttpContext.Session.GetInt32("AccID");
            List<Person> personList = personContext.GetAllPerson(accID);
            int totalCal = CalculateTotalCalories(personList);
            HttpContext.Session.SetInt32("TotalCal",totalCal);
            return View();
        }

        // GET: HomeController/LogOut
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        private int CalculateTotalCalories(List<Person> personList)
        {
            int totalCals = 0;
            foreach(Person p in personList)
            {
                totalCals += p.MaxCal;
            }
            return totalCals;
        }
    }
}
