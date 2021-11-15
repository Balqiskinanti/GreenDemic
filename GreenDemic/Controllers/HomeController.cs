﻿using Greendemic.DAL;
using Greendemic.Models;
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
        private ItemDAL itemContext = new ItemDAL();
        private ShoppingBagItemDAL shoppingBagItemContext = new ShoppingBagItemDAL();
        private ShoppingBagDAL shoppingBagContext = new ShoppingBagDAL();

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
        private int CalculateCalTotal(List<ItemViewModel> itemVMList)
        {
            int totalCal = 0;
            foreach (ItemViewModel i in itemVMList)
            {
                int calSubTotal = i.CalSubTotal;
                totalCal += calSubTotal;
            }
            return totalCal;
        }
        public List<ItemViewModel> MapToItemVM(int shoppingBagID)
        {
            List<ItemViewModel> itemVMList = new List<ItemViewModel>();
            List<ShoppingBagItem> shoppingBagItemList = shoppingBagItemContext.GetAllShoppingBagItem(shoppingBagID);
            foreach (ShoppingBagItem s in shoppingBagItemList)
            {
                int itemID = s.ItemID;
                Item item = itemContext.GetDetails(itemID);
                ItemViewModel itemViewModel = new ItemViewModel
                {
                    ItemID = item.ItemID,
                    ShoppingBagID = s.ShoppingBagID,
                    ItemName = item.ItemName,
                    Category = item.Category,
                    Cal = item.Cal,
                    Qty = s.Qty,
                    CalSubTotal = item.Cal * s.Qty
                };
                itemVMList.Add(itemViewModel);
            }
            return itemVMList;
        }
        public ShoppingBagViewModel MapToShoppingBagVM(ShoppingBag shoppingBag)
        {
            List<ItemViewModel> itemVMList = MapToItemVM(shoppingBag.ShoppingBagID);
            ShoppingBagViewModel shoppingBagViewModel = new ShoppingBagViewModel
            {
                ShoppingBagID = shoppingBag.ShoppingBagID,
                BagName = shoppingBag.BagName,
                CreatedAt = shoppingBag.CreatedAt,
                BagDescription = shoppingBag.BagDescription,
                totalCals = CalculateCalTotal(itemVMList)
            };

            return shoppingBagViewModel;
        }

        // GET: HomeController/Main
        public IActionResult Main()
        {
            //Account users' total calories
            int accID = (int)HttpContext.Session.GetInt32("AccID");
            List<Person> personList = personContext.GetAllPerson(accID);
            int totalCal = CalculateTotalCalories(personList);
            HttpContext.Session.SetInt32("TotalCal",totalCal);

            //This month's total calories
            List<ShoppingBag> bagList = shoppingBagContext.GetThisMonthBags(accID);
            int thisMonthCal = CalculateMonthCalories(bagList);
            ViewData["ThisMonthCals"] = thisMonthCal;

            //Get total calories of each categories this month
            List<ShoppingBagItem> sbItemList = new List<ShoppingBagItem>();
            List<int> sbIDList = new List<int>();
            List<int> itemIDList = new List<int>();
            int snackCals = 0;
            int meatCals = 0;
            int seafoodCals = 0;
            int dairyCals = 0;
            int grainsCals = 0;
            int fruitCals = 0;
            int vegeCals = 0;
            int othersCals = 0;


            foreach (ShoppingBag s in bagList)
            {
                sbIDList.Add(s.ShoppingBagID);
            }

            foreach(int sbID in sbIDList)
            {
                sbItemList = shoppingBagItemContext.GetAllShoppingBagItem(sbID);
            }

            foreach(ShoppingBagItem sbItem in sbItemList)
            {
                itemIDList.Add(sbItem.ItemID);
            }

            foreach(ShoppingBagItem s in sbItemList)
            {
                Item item = itemContext.GetDetails(s.ItemID);
                if(item.Category == "Snack")
                {
                    snackCals += (item.Cal * s.Qty);
                }
                else if(item.Category == "Meat")
                {
                    meatCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Seafood")
                {
                    seafoodCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Dairy")
                {
                    dairyCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Grains")
                {
                    grainsCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Fruit")
                {
                    fruitCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Vegetable")
                {
                    vegeCals += (item.Cal * s.Qty);
                }
                else if (item.Category == "Others")
                {
                    othersCals += (item.Cal * s.Qty);
                }
            }
            ViewData["Snack"] = snackCals;
            ViewData["Meat"] = meatCals;
            ViewData["Seafood"] = seafoodCals;
            ViewData["Dairy"] = dairyCals;
            ViewData["Grains"] = grainsCals;
            ViewData["Fruit"] = fruitCals;
            ViewData["Vegetable"] = vegeCals;
            ViewData["Others"] = othersCals;

            // Get total calories of items across the months for the year
            List<ShoppingBag> bagListJan = shoppingBagContext.GetBagsOfMonth(accID, 1);
            int janCal = CalculateMonthCalories(bagListJan);

            List<ShoppingBag> bagListFeb = shoppingBagContext.GetBagsOfMonth(accID, 2);
            int febCal = CalculateMonthCalories(bagListFeb);

            List<ShoppingBag> bagListMar = shoppingBagContext.GetBagsOfMonth(accID, 3);
            int marCal = CalculateMonthCalories(bagListMar);

            List<ShoppingBag> bagListApr = shoppingBagContext.GetBagsOfMonth(accID, 4);
            int aprCal = CalculateMonthCalories(bagListApr);

            List<ShoppingBag> bagListMay = shoppingBagContext.GetBagsOfMonth(accID, 5);
            int mayCal = CalculateMonthCalories(bagListMay);

            List<ShoppingBag> bagListJun = shoppingBagContext.GetBagsOfMonth(accID, 6);
            int junCal = CalculateMonthCalories(bagListJun);

            List<ShoppingBag> bagListJul = shoppingBagContext.GetBagsOfMonth(accID, 7);
            int julCal = CalculateMonthCalories(bagListJul);

            List<ShoppingBag> bagListAug = shoppingBagContext.GetBagsOfMonth(accID, 8);
            int augCal = CalculateMonthCalories(bagListAug);

            List<ShoppingBag> bagListSept = shoppingBagContext.GetBagsOfMonth(accID, 9);
            int septCal = CalculateMonthCalories(bagListSept);

            List<ShoppingBag> bagListOct = shoppingBagContext.GetBagsOfMonth(accID, 10);
            int octCal = CalculateMonthCalories(bagListOct);

            List<ShoppingBag> bagListNov = shoppingBagContext.GetBagsOfMonth(accID, 11);
            int novCal = CalculateMonthCalories(bagListNov);

            List<ShoppingBag> bagListDec = shoppingBagContext.GetBagsOfMonth(accID, 12);
            int decCal = CalculateMonthCalories(bagListDec);

            List<int> calForTheYearList = new List<int>() { janCal, febCal, marCal, aprCal, mayCal, junCal, julCal, augCal, septCal, octCal, novCal, decCal };
            ViewData["ThisYearCals"] = calForTheYearList;
             
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

            DateTime today = new DateTime();
            int totalDaysThisMonth = DateTime.DaysInMonth(today.Year, today.Month);
            return totalCals * totalDaysThisMonth;
        }

        private int CalculateMonthCalories(List<ShoppingBag> bagList)
        {
            int cal = 0;
            List<ShoppingBagViewModel> shoppingBagViewModelList = new List<ShoppingBagViewModel>();
            foreach (ShoppingBag bag in bagList)
            {
                ShoppingBagViewModel shoppingBagVM = MapToShoppingBagVM(bag);
                shoppingBagViewModelList.Add(shoppingBagVM);
            }
            foreach (ShoppingBagViewModel bag in shoppingBagViewModelList)
            {
                cal += bag.totalCals;
            }
            return cal;
        }
    }
}
