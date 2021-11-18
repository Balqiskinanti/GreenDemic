using Greendemic.DAL;
using Greendemic.Models;
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
    public class ItemController : Controller
    {
        // DAL context
        private ItemDAL itemContext = new ItemDAL();
        private ShoppingBagItemDAL shoppingBagItemContext = new ShoppingBagItemDAL();
        private ShoppingBagDAL shoppingBagContext = new ShoppingBagDAL();

        // Logging 
        private readonly ILogger<ItemController> _logger;
        public ItemController(ILogger<ItemController> logger)
        {
            _logger = logger;
        }

        // Return category of food for dropdown list
        private List<SelectListItem> GetCategory()
        {
            List<SelectListItem> categoryList = new List<SelectListItem>();
            List<String> myCategoryList = new List<string>() { "Snack", "Meat", "Seafood", "Dairy", "Grains", "Fruit", "Vegetable", "Others" };

            foreach (String cat in myCategoryList)
            {
                categoryList.Add(
                    new SelectListItem
                    {
                        Value = cat,
                        Text = cat
                    });
            }
            return categoryList;
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

        //Return itemviewmodel based on a shopping bag and item ID
        public ItemViewModel MapToItemVM(int shoppingBagID, int itemID)
        {
            Item item = itemContext.GetDetails(itemID);
            ShoppingBagItem shoppingBagItem = shoppingBagItemContext.GetDetails(itemID, shoppingBagID);
            ItemViewModel itemViewModel = new ItemViewModel
            {
                ItemID = item.ItemID,
                ShoppingBagID = shoppingBagItem.ShoppingBagID,
                ItemName = item.ItemName,
                Category = item.Category,
                Cal = item.Cal,
                Qty = shoppingBagItem.Qty,
                CalSubTotal = shoppingBagItem.Qty * item.Cal
            };
            return itemViewModel;
        }

        // GET: ItemController
        public ActionResult Index(int? id)
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            // If ID is not supplied
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<ItemViewModel> itemVMList = MapToItemVM(id.Value);
            ViewData["ShoppingBagId"] = id.Value; //for create and delete item 
            return View(itemVMList);
        }

        // GET: ItemController/Create
        public ActionResult Create(int? id)
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            // If ID is not supplied
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ShoppingBagId"] = id.Value;
            ViewData["CategoryList"] = GetCategory();
            ViewData["IsFailed"] = false;
            return View();
        }

        // POST: ItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemViewModel model, int? id)
        {
            ViewData["IsFailed"] = false;
            ViewData["ShoppingBagId"] = id.Value;
            ViewData["CategoryList"] = GetCategory();

            // Adding new item in shopping bag
            try
            {
                if (ModelState.IsValid)
                {
                    Item item = new Item();
                    item.ItemID = model.ItemID;
                    item.ItemName = model.ItemName;
                    item.Category = model.Category;
                    item.Cal = model.Cal;
                    int itemID = itemContext.Add(item);

                    ShoppingBagItem shoppingBagItem = new ShoppingBagItem();
                    shoppingBagItem.ItemID = itemID;
                    shoppingBagItem.Qty = model.Qty;
                    shoppingBagItem.ShoppingBagID = model.ShoppingBagID;
                    shoppingBagItemContext.Add(shoppingBagItem);
                    return RedirectToAction("Index", "Item", new { id = ViewData["ShoppingBagId"] });
                }
                else
                {
                    return View(model);
                }

            }
            // Shows error message
            catch
            {
                ViewData["IsFailed"] = true;
                return View(model);
            }
        }

        // GET: ItemController/Edit/5
        public ActionResult Edit(int? id, int? sbID)
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            // If ID is not supplied
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // If sbID is not supplid
            if (sbID == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["IsFailed"] = false;
            ViewData["ShoppingBagId"] = sbID.Value;
            ViewData["CategoryList"] = GetCategory();
            ItemViewModel itemViewModel = MapToItemVM(sbID.Value, id.Value);
            return View(itemViewModel);
        }

        // POST: ItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemViewModel itemVM)
        {
            ViewData["ShoppingBagId"] = itemVM.ShoppingBagID;
            ViewData["CategoryList"] = GetCategory();
            ViewData["IsFailed"] = false;

            // Update item view model (shopping bag + item)
            try
            {
                if (ModelState.IsValid)
                {
                    Item item = new Item();
                    item.ItemID = itemVM.ItemID;
                    item.ItemName = itemVM.ItemName;
                    item.Cal = itemVM.Cal;
                    item.Category = itemVM.Category;

                    ShoppingBagItem shoppingBagItem = new ShoppingBagItem();
                    shoppingBagItem.ShoppingBagID = itemVM.ShoppingBagID;
                    shoppingBagItem.ItemID = itemVM.ItemID;
                    shoppingBagItem.Qty = itemVM.Qty;
                    itemContext.Update(item, shoppingBagItem);
                    return RedirectToAction("Index", "Item", new { id = ViewData["ShoppingBagId"] });
                }
                else
                {
                    return View(itemVM);
                }
            }
            // Shows error messages
            catch
            {
                ViewData["IsFailed"] = true;
                return View(itemVM);
            }
        }

        // GET: ItemController/Delete/5
        public ActionResult Delete(int? id, int? sbID)
        {
            // Authenticate user
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            // If id is not supplied
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // IF sbID is not supplied
            if (sbID == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ShoppingBagId"] = sbID.Value;
            ViewData["IsFailed"] = false;
            ItemViewModel itemVM = MapToItemVM(sbID.Value, id.Value);
            return View(itemVM);
        }

        // POST: ItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ItemViewModel itemVM)
        {
            ViewData["IsFailed"] = false;
            // Delete item from the bag
            try
            {
                if (ModelState.IsValid)
                {
                    itemContext.Delete(itemVM.ItemID, itemVM.ShoppingBagID);
                    return RedirectToAction("Index", "ShoppingBag");
                }
                else
                {
                    return View(itemVM);
                }
            }
            // Shows error message
            catch
            {
                ViewData["IsFailed"] = true;
                return View(itemVM);
            }
        }
    }
}