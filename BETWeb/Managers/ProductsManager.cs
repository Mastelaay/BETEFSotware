using BET.Domain;
using BET.Domain.Interfaces;
using BET.Domain.Models;
using BETWeb.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BETWeb.Extensions;
using System.Web.Mvc;

namespace BETWeb.Managers
{
    public class ProductsManager : IProducts

    {
        private IBETUnitOfWork _uow { get; set; }

        public ProductsManager(IBETUnitOfWork uow)
        {
            _uow = uow;
        }

        public ApiItemsResponseModel<ProductsModel> GetProductList()
        {
            var repo = _uow.GetRepo<Product>();
            var data = repo.GetByIQuerable().ToList().Select(s => s.ToReturnProducts()).ToList();
            return new ApiItemsResponseModel<ProductsModel>
            {
                Items = data,
                IsSuccess = true
            };
        }

        public ApiItemResponseModel<ProductsModel> GetProductItemById(int pId)
        {
            var repo = _uow.GetRepo<Product>();
            var data = repo.GetByIQuerable(p => p.PID == pId).Select(s => s.ToReturnProducts()).FirstOrDefault();
            return new ApiItemResponseModel<ProductsModel>
            {
                Item = data,
                IsSuccess = true
            };
        }

        public ApiItemsResponseModel<ProductsModel> GetProductListByCatName(string catName)
        {
            var repo = _uow.GetRepo<Product>();
            var data = repo.GetByIQuerable(p => p.Category == catName).ToList().Select(s=>s.ToReturnProducts()).ToList();
            return new ApiItemsResponseModel<ProductsModel>
            {
                Items = data,
                IsSuccess = true
            };
        }

        public ApiItemsResponseModel<SelectListItem> ProductsDList()
        {
            var products = GetProductList();
            var list = new List<SelectListItem>();
            foreach (var item in products.Items)
            {
                var selecListItem = new SelectListItem { Value = item.Category, Text = item.Category };
                list.Add(selecListItem);
            }
            return new ApiItemsResponseModel<SelectListItem> { Items = list };
        }
    }
}