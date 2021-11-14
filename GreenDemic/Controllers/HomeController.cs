using Greendemic.DAL;
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
            int cal = 0;
            List<ShoppingBag> bagList = shoppingBagContext.GetThisMonthBags(accID);
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
            ViewData["ThisMonthCals"] = cal;

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
