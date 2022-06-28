using BET.Domain;
using BETEFSotware.App_Data;
using BETEFSotware.Extensions;
using BET.Domain.Interfaces;
using BET.Domain.Models;
using System;
using System.Linq;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace BETEFSotware.Managers
{
    public class CheckoutManager : ICheckout
    {
        private IBETUnitOfWork _uow { get; set; }
        public CheckoutManager(IBETUnitOfWork uow)
        {
            _uow = uow;
        }
        public ApiResponseModel SaveCheckOutInformatoin(CheckoutModel cart)
        {

            List<ProductsModel> products = new List<ProductsModel>();
            var dataRepo = _uow.GetRepo<ShoppingCartData>();
            var repo = _uow.GetRepo<Cart>();
            var orderNumber = GenerateOrderNumber().Message;

            //Get all user cart data and adds them to a list of purchase busket
            var busketData = dataRepo.GetByIQuerable(s => s.UserId == cart.User.UserId).ToList();
            foreach (var item in busketData)
            {
                var prooduct = new ProductsModel
                {
                    ProductId = item.PID,
                    ProductName = item.PName,
                    Price = item.UnitPrice,
                    Quantity = item.Quantity
                };
                products.Add(prooduct);
            }
        
            //var total = busketData.Sum(c => c.Quantity * c.UnitPrice);
            cart.TotalPrice = busketData.Sum(c => c.Quantity * c.UnitPrice).ToString("C", new CultureInfo("en-ZA"));
            cart.Products = products;

            //Deletes all data temp in db 
            foreach (var i in busketData)
            {
                dataRepo.Delete(i);
            }
            dataRepo.SaveChanges();

            
            foreach (var item in cart.Products)
            {
                var model = new CartModel
                {
                    ProductId = item.ProductId,
                    Price =Convert.ToDecimal(item.Price),
                    Quantity = item.Quantity,
                    UserId = cart.User.UserId,
                    OrderNumber = orderNumber
                };
                var saveDetails = model.ToCheckOutDetails();
                repo.Add(saveDetails);
                repo.SaveChanges();
            }

            var email = SendEmailConfirmation(cart, orderNumber);

            return email.IsSuccess
                ? new ApiResponseModel { IsSuccess = true, Message = "Cart saved successfully" }
                : new ApiResponseModel { IsSuccess = false, Message = "Cart not saved successfully" };
        }

        public ApiResponseModel SendEmailConfirmation(CheckoutModel cart, string orderNumber)
        {
            var emailBody = HelperManager.CreateEmailBody(1, cart, orderNumber);
            HelperManager.SendEmail("BET Order", emailBody, cart.User.Email);

            return new ApiResponseModel { IsSuccess = true };
        }



        public ApiResponseModel GenerateOrderNumber()
        {
            var length = 7;
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var toReturn =  new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return new ApiResponseModel { IsSuccess = true, Message = toReturn };
        }

        public ApiResponseModel AddUpdateToCart(CartActionsModel model)
        {
            // check if product is valid
            var productRepo = _uow.GetRepo<Product>();
            var dataRepo = _uow.GetRepo<ShoppingCartData>();
            Product product = productRepo.GetByIQuerable().FirstOrDefault(p => p.PID == model.ProductId);

            if (product.UnitsInStock == 0) return new ApiResponseModel { IsSuccess = false, Message = "Product is out of stock" };

            if (product != null && product.UnitsInStock > 0)
            {
                // check if product already existed
                ShoppingCartData cart = dataRepo.GetByIQuerable().FirstOrDefault(c => c.PID == model.ProductId && c.UserId == model.LoggedInUser);
                if (cart != null)
                {
                    cart.Quantity++;
                }
                else
                {
                    cart = new ShoppingCartData
                    {
                        PName = product.PName,
                        PID = product.PID,
                        UnitPrice = product.UnitPrice,
                        Quantity = 1,
                        UserId = model.LoggedInUser

                    };

                    dataRepo.Add(cart);
                }
                product.UnitsInStock--;
                productRepo.SaveChanges();
                dataRepo.SaveChanges();

            }
            return new ApiResponseModel { IsSuccess = true, Message="Product added to cart" };
        }

        public ApiResponseModel AddUpdateQuantity(CartActionsModel model)
        {
            var repo = _uow.GetRepo<ShoppingCartData>();
            var productRepo = _uow.GetRepo<Product>();
            ShoppingCartData product = repo.GetByIQuerable().FirstOrDefault(p => p.PID == model.ProductId && p.UserId == model.LoggedInUser);
            if (product == null)
            {
                return new ApiResponseModel { IsSuccess = false, ResourceId = 0 };
            }


            Product actualProduct = productRepo.GetByIQuerable().FirstOrDefault(p => p.PID == model.ProductId);
            int quantity;
            // if type 0, decrease quantity
            // if type 1, increase quanity
            switch (model.Type)
            {
                case 0:
                    product.Quantity--;
                    actualProduct.UnitsInStock++;
                    break;
                case 1:
                    product.Quantity++;
                    actualProduct.UnitsInStock--;
                    break;
                case -1:
                    actualProduct.UnitsInStock += product.Quantity;
                    product.Quantity = 0;
                    break;
                default:
                    return new ApiResponseModel { ResourceId = 0 };
            }

            if (product.Quantity == 0)
            {
                repo.Delete(product);
                quantity = 0;
            }
            else
            {
                quantity = product.Quantity;
            }

            productRepo.SaveChanges();
            return new ApiResponseModel { IsSuccess = true, ResourceId = quantity };
            
        }
    }
}