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
        private readonly ILogger<ItemController> _logger;
        private ItemDAL itemContext = new ItemDAL();
        private ShoppingBagItemDAL shoppingBagItemContext = new ShoppingBagItemDAL();
        private ShoppingBagDAL shoppingBagContext = new ShoppingBagDAL();
        public ItemController(ILogger<ItemController> logger)
        {
            _logger = logger;
        }

        // Return area interest list for dropdown
        private List<SelectListItem> GetCategory()
        {
            List<SelectListItem> categoryList = new List<SelectListItem>();
            List<String> myCategoryList = new List<string>() { "Snack","Meat","Seafood","Dairy","Grains","Fruit","Vegetable","Others" };

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
            List<ItemViewModel> itemVMList = MapToItemVM(id.Value);
            ViewData["ShoppingBagId"] = id.Value;
            return View(itemVMList);
        }

        // GET: ItemController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ItemController/Create
        public ActionResult Create(int? id)
        {
            ViewData["ShoppingBagId"] = id.Value;
            ViewData["CategoryList"] = GetCategory();
            return View();
        }

        // POST: ItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemViewModel model, int? id)
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

            ViewData["ShoppingBagId"] = id.Value;
            ViewData["CategoryList"] = GetCategory();
            return RedirectToAction("Index","Item", new { id = ViewData["ShoppingBagId"] });
        }

        // GET: ItemController/Edit/5
        public ActionResult Edit(int? id, int? sbID)
        {
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

            ViewData["ShoppingBagId"] = itemVM.ShoppingBagID;
            ViewData["CategoryList"] = GetCategory();
            return RedirectToAction("Index", "Item", new { id = ViewData["ShoppingBagId"] });
        }

        // GET: ItemController/Delete/5
        public ActionResult Delete(int? id, int? sbID)
        {
            ViewData["ShoppingBagId"] = sbID.Value;
            ItemViewModel itemVM = MapToItemVM(sbID.Value, id.Value);
            return View(itemVM);
        }

        // POST: ItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ItemViewModel itemVM)
        {
            itemContext.Delete(itemVM.ItemID, itemVM.ShoppingBagID);
            return RedirectToAction("Index", "ShoppingBag");
        }
    }
}
