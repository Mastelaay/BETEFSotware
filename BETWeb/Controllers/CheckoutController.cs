using BET.Domain;
using BETWeb.App_Data;
using BETWeb.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BET.Domain.Interfaces;
using BET.Domain.Models;

namespace BETWeb.Controllers
{
  
    public class CheckoutController : BaseController
    {
        private IBETUnitOfWork _uow { get; set; }
        private ICart _cartManager;
  
        public CheckoutController(IBETUnitOfWork uow)
        {
            _uow = uow;
            _cartManager = new CartManager(_uow);
        }

        [Authorize]
        public ActionResult Index()
        {
            var request = _cartManager.GetShoppingDataList(LoggedOnUser);
            ViewBag.Cart = request.Items;
            return View();
        }

        public JsonResult ChangeQuantity(int type, int pId)
        {
            var request = _cartManager.QuatityChange(type, pId, LoggedOnUser);
            return JsonResultBaseMethod(request);
        }

        [HttpGet]
        public JsonResult UpdateTotal()
        {
            var request = _cartManager.UpdateTotal();
            return Json(new { Total = request.Total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Clear()
        {
            _cartManager.Clear(LoggedOnUser);
            return RedirectToAction("Index", "Home", null);
        }
        public ActionResult Purchase()
        {
            var model = new UsersModel
            {
                UserId = LoggedOnUser,
                Email = LoggedOnUserEmail,
                Name = LoggedOnUserName
            };

            var request = _cartManager.PurchaseItems(model);
            if(request.IsSuccess)
                 return RedirectToAction("PurchasedSuccess");

            return RedirectToAction("Index");
        }
        public ActionResult PurchasedSuccess()
        {
            return View();
        }
     
    }
}