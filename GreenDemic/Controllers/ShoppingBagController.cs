using GreenDemic.DAL;
using GreenDemic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenDemic.Controllers
{
    public class ShoppingBagController : Controller
    {
        // Initialise DAL class
        private ShoppingBagDAL shoppingBagContext = new ShoppingBagDAL();

        // GET: ShoppingBagController
        public ActionResult Index()
        {
            List<ShoppingBag> bagList = shoppingBagContext.GetAllShoppingBags();
            return View(bagList);
        }

        // GET: ShoppingBagController/Details/5
        public ActionResult Details(int? id)
        {
            ShoppingBag bag = shoppingBagContext.GetDetails(id.Value);
            return View(bag);
        }

        // GET: ShoppingBagController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShoppingBagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShoppingBag shoppingBag)
        {
            try
            {
                shoppingBag.ShoppingBagID = shoppingBagContext.Add(shoppingBag);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ShoppingBagController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            ShoppingBag bag = shoppingBagContext.GetDetails(id.Value);
            return View(bag);
        }

        // POST: ShoppingBagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShoppingBag bag)
        {
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
                return View(bag);
            }
        }

        // GET: ShoppingBagController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            ShoppingBag bag = shoppingBagContext.GetDetails(id.Value);
            return View(bag);
        }

        // POST: ShoppingBagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ShoppingBag bag)
        {
            try
            {
                shoppingBagContext.Delete(bag.ShoppingBagID);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(bag);
            }
        }
    }
}
