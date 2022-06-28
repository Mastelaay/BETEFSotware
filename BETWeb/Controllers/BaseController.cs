using BET.Domain;
using BET.Domain.Models;
using BETWeb.App_Data;
using BETWeb.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BETWeb.Controllers
{
    public class BaseController : Controller
    {
       
        private readonly IBETUnitOfWork _uow;
        private CartManager _cartManager;
        private int _loggedOnUser;
        private string _loggedOnUserName;
        private string _loggedOnUserEmail;
        public BaseController()
        {
           _uow = new BETUnitOfWork (new BETDataEntities());
            _cartManager = new CartManager(_uow);
       
           
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (LoggedOnUser == 0) RedirectToAction("Login", "Account");
            _loggedOnUser = (int)System.Web.HttpContext.Current.Session["LoginID"];
            _loggedOnUserName = System.Web.HttpContext.Current.Session["LoginName"].ToString();
            _loggedOnUserEmail = System.Web.HttpContext.Current.Session["LoginEmail"].ToString();

            ViewBag.User = _loggedOnUser > 0; ;
            ViewBag.CartTotalPrice = CartTotalPrice.ToString("C", new CultureInfo("en-ZA"));
            ViewBag.Cart = Cart;
            ViewBag.CartUnits = Cart.Count;
            ViewBag.CV = new CultureInfo("en-ZA");
        }


        private List<ShoppingCartDataModel> Cart => _cartManager.GetShoppingDataList(_loggedOnUser).Items;

        private decimal CartTotalPrice => _cartManager.GetCartTotalPrice(_loggedOnUser);

        protected JsonResult JsonResultBaseMethod(ApiResponseModel response)
        {
            if (!response.IsSuccess)
            {
                return Json(new JsonResponseModel { Success = false, ResultMessage = response.Message , ResourceId = response.ResourceId, Total = response.Total },
                    "application/json");
            }

            return Json(new JsonResponseModel { Success = true, ResultMessage = response.Message });
        }
        protected int LoggedOnUser =>_loggedOnUser;
        protected string LoggedOnUserName => _loggedOnUserName;
        protected string LoggedOnUserEmail => _loggedOnUserEmail;

    }
}