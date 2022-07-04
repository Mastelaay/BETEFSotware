using BET.Domain;
using BET.Domain.Interfaces;
using BET.Domain.Models;
using BETWeb.App_Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using BETWeb.Extensions;
using System.Net.Http;
using System.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BETWeb.Managers
{
    public class CartManager : ICart
    {
        private IBETUnitOfWork _uow { get; set; }
        private string baseAddress => ConfigurationManager.AppSettings["ServiceProviderUrl"];
        public CartManager(IBETUnitOfWork uow)
        {
            _uow = uow;
        }

        public decimal GetCartTotalPrice(int loggedInUserId)
        {
            var total = GetShoppingDataList(loggedInUserId).Items;
            return total.Sum(c => c.Quantity * c.UnitPrice);
        }

        public ApiItemsResponseModel<ShoppingCartDataModel> GetShoppingDataList(int loggedInUserId)
        {
            var repo = _uow.GetRepo<ShoppingCartData>();
            var data = repo.GetByIQuerable(i=>i.UserId == loggedInUserId).ToList().Select(s => s.ToReturnCardData()).ToList();
           
            return new ApiItemsResponseModel<ShoppingCartDataModel>
            {
                Items = data
            };
        }

        public ApiResponseModel QuatityChange(int type, int pId,int loggedInUser)
        {
            var model = new CartActionsModel
            {
                ProductId = pId,
                LoggedInUser = loggedInUser,
                Type = type

            };
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var data = JsonConvert.SerializeObject(model);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync("Checkout/quantity-change/v1", content).Result;
                    if (response.IsSuccessStatusCode)
                    {

                        var request = response.Content.ReadAsStringAsync().Result;
                        var responseObj = JObject.Parse(request);
                        return new ApiResponseModel { IsSuccess = responseObj["IsSuccess"].Value<bool>(), Message = responseObj["Message"].ToString() ,ResourceId = responseObj["ResourceId"].Value<int>() };
                    }
                }
                return new ApiResponseModel { IsSuccess = false, Message = "Error Saving cart" };
            }

            catch (Exception e)
            {
                return new ApiResponseModel { IsSuccess = false, Message = "Exception Error Saving cart" + e };

                throw;
            }
        }

        public ApiResponseModel UpdateTotal()
        {
            var repo = _uow.GetRepo<ShoppingCartData>();
            decimal total;
            try
            {
                total = repo.GetByIQuerable().Select(p => p.UnitPrice * p.Quantity).Sum();
            }
            catch (Exception) { total = 0; }
            return new ApiResponseModel { Total = total.ToString("C", new CultureInfo("en-ZA")) };
        }

        public ApiResponseModel Clear(int loggedInUser)
        {
             try
            {
                BETDataEntities _context = new BETDataEntities();
                List<ShoppingCartData> carts = _context.ShoppingCartDatas.Where(s => s.UserId == loggedInUser).ToList();
                carts.ForEach(a => {
                    Product product = _context.Products.FirstOrDefault(p => p.PID == a.PID);
                    product.UnitsInStock += a.Quantity;
                });
                _context.ShoppingCartDatas.RemoveRange(carts);
                _context.SaveChanges();
            }
            catch (Exception) { }
            return new ApiResponseModel { IsSuccess = true };
        }

        public ApiResponseModel AddToCart(int pId,int loggedInUser)
        {
             var model = new CartActionsModel
            {
                ProductId = pId,
                LoggedInUser = loggedInUser

            };
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var data = JsonConvert.SerializeObject(model);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync("Checkout/addToCart-basket/v1", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                       
                        var request = response.Content.ReadAsStringAsync().Result;
                        var responseObj = JObject.Parse(request);
                        return new ApiResponseModel { IsSuccess = responseObj["IsSuccess"].Value<bool>(), Message = responseObj["Message"].ToString() };
                    }
                }
                return new ApiResponseModel { IsSuccess = false, Message = "Error Saving cart" };
            }

            catch (Exception e)
            {
                return new ApiResponseModel { IsSuccess = false, Message = "Exception Error Saving cart" + e };

                throw;
            }
        }

        public ApiResponseModel PurchaseItems(UsersModel purchaseModel)
        {

            var model = new CheckoutModel
            {
              User = purchaseModel,
            };

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var data = JsonConvert.SerializeObject(model);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync("Checkout/checkout-basket/v1", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var request = response.Content.ReadAsStringAsync().Result;
                        var responseObj = JObject.Parse(request);
                        return new ApiResponseModel { IsSuccess = responseObj["IsSuccess"].Value<bool>(), Message = responseObj["Message"].ToString() };
                    }
                }
                return new ApiResponseModel { IsSuccess = false, Message = "Error Saving cart" };
            }

            catch (Exception e)
            {
                return new ApiResponseModel { IsSuccess = false, Message ="Exception Error Saving cart"+ e};

                throw;
            }
        }
    }
}