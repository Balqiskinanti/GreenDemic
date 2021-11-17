using Greendemic.DAL;
using Greendemic.Models;
using GreenDemic.DAL;
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
    public class ShoppingBagController : Controller
    {
        // DAL context
        private ShoppingBagDAL shoppingBagContext = new ShoppingBagDAL();
        private ItemDAL itemContext = new ItemDAL();
        private ShoppingBagItemDAL shoppingBagItemContext = new ShoppingBagItemDAL();

        //Logging
        private readonly ILogger<ShoppingBagController> _logger;
        public ShoppingBagController(ILogger<ShoppingBagController> logger)
        {
            _logger = logger;
        }

        //Calculate total calories in the shopping bag
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

        //Returns list of itemviewmodel based on shopping bag ID
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

        //Returns shoppingbagviewmodel based on shopping bag ID
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

        // GET: ShoppingBagController
        public ActionResult Index()
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            int accID = (int)HttpContext.Session.GetInt32("AccID");
            List<ShoppingBag> bagList = shoppingBagContext.GetAllShoppingBags(accID);
            List<ShoppingBagViewModel> shoppingBagViewModelList = new List<ShoppingBagViewModel>();
            foreach(ShoppingBag bag in bagList)
            {
                ShoppingBagViewModel shoppingBagVM = MapToShoppingBagVM(bag);
                shoppingBagViewModelList.Add(shoppingBagVM);
            }
            return View(shoppingBagViewModelList);
        }

        // GET: ShoppingBagController/Create
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

        // POST: ShoppingBagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShoppingBag shoppingBag)
        {
            ViewData["IsFailed"] = false;
            // Add shopping bag
            try
            {
                if (ModelState.IsValid)
                {
                    shoppingBag.ShoppingBagID = shoppingBagContext.Add(shoppingBag);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(shoppingBag);
                }
                    
            }
            // Shows error messages
            catch
            {   
                ViewData["IsFailed"] = true;
                return View(shoppingBag);
            }
        }

        // GET: ShoppingBagController/Edit/5
        public ActionResult Edit(int? id)
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }
            ShoppingBag bag = shoppingBagContext.GetDetails(id.Value);
            ViewData["IsFailed"] = false;
            return View(bag);
        }

        // POST: ShoppingBagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShoppingBag bag)
        {
            ViewData["IsFailed"] = false;
            try
            {
                if (ModelState.IsValid)
                {
                    shoppingBagContext.Update(bag);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(bag);
                }
            }
            catch
            {
                ViewData["IsFailed"] = true;
                return View(bag);
            }
        }

        // GET: ShoppingBagController/Delete/5
        public ActionResult Delete(int? id)
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            ShoppingBag bag = shoppingBagContext.GetDetails(id.Value);
            ViewData["IsFailed"] = false;
            return View(bag);
        }

        // POST: ShoppingBagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ShoppingBag bag)
        {
            ViewData["IsFailed"] = false;
            try
            {
                if (ModelState.IsValid)
                {
                    shoppingBagContext.Delete(bag.ShoppingBagID);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(bag);
                }
            }
            catch
            {
                ViewData["IsFailed"] = true;
                return View(bag);
            }
        }
    }
}
