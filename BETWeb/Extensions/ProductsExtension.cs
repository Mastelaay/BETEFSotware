using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BET.Domain.Models;
using BETWeb.App_Data;

namespace BETWeb.Extensions
{
    public static class ProductsExtension
    {
        public static ProductsModel ToReturnProducts(this Product entity)
        {
            if (entity == null) return null;


            return new ProductsModel
            {
                ProductName = entity.PName,
                ProductId = entity.PID,
                Price = entity.UnitPrice,
                Quantity = entity.UnitsInStock,
                Brand = entity.Brand,
                Category = entity.Category,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl
            };
        }
    }
}
        