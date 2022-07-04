using BET.Domain;
using BETWeb.Managers;
using System.Collections.Generic;
using System.Web.Mvc;
using BET.Domain.Models;
using BET.Domain.Interfaces;


namespace BETWeb.Controllers
{
    
    public class HomeController : BaseController
       {
 
        private  IBETUnitOfWork _uow { get; set; }
        private IProducts _prodManager;
        private ICart _cartManager;

       
        public HomeController(IBETUnitOfWork uow)
        {
            _uow = uow;
            _prodManager = new ProductsManager(_uow);
            _cartManager = new CartManager(_uow);
        }
       [Authorize]
        public ActionResult Index()
        {
            var products = _prodManager.GetProductList();
            ViewBag.Categories = _prodManager.ProductsDList().Items;
            ViewBag.Products = products.Items;
            return View();
        }

        public ActionResult Category(string catName)
        {
            List<ProductsModel> products = catName == "" ? _prodManager.GetProductList().Items : _prodManager.GetProductListByCatName(catName).Items;
            ViewBag.Products = products;
            return View("Index");
        }

        public ActionResult AddToCart(int id)
        {
            addToCart(id);
            return RedirectToAction("Index");
        }

        private void addToCart(int pId)
        {
            var request = _cartManager.AddToCart(pId, LoggedOnUser);
        }


      
    }
}