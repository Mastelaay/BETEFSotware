using BET.Domain;
using BET.Domain.Interfaces;
using BET.Domain.Models;
using BETWeb.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BETWeb.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private IBETUnitOfWork _uow { get; set; }
        private IUsers _accManager;
        public AccountController(IBETUnitOfWork uow)
        {
            _uow = uow;
            _accManager = new AccountManager();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UsersModel model)
        {

            if (ModelState.IsValid)
            {
                var request = _accManager.RegisterUser(model);
                return request.IsSuccess
                    ? RedirectToAction("Success", "Account")
                    : RedirectToAction("Error", new { message = request.Message, type = "Registration" });

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
          
           ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                     
            var result = _accManager.LoginUser(model.Email, model.Password);
            if (result.IsSuccess)
            {
                Session["LoginID"] = result.Item.UserId;
                Session["LoginEmail"] = result.Item.Email;
                Session["LoginName"] = result.Item.Name;
                FormsAuthentication.SetAuthCookie(result.Item.Name, false);
                return returnUrl == null ? RedirectToAction("Index", "Home") : (ActionResult)Redirect(returnUrl);
            }
            else
            {
              return RedirectToAction("Error", new { message = result.Message , type ="Login" });

            }
                        
        }

        public ActionResult Success()
        {
            return View();
        }
         
        public ActionResult Error(string message , string type)
        {
            ViewBag.ErrorMessage = message;
            ViewBag.ErrorType = type;
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult UnAuthorized()
        {
            return View();
        }


    }
}