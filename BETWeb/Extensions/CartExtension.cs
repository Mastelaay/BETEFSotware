using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BET.Domain.Models;
using BETWeb.App_Data;

namespace BETWeb.Extensions
{
    public static class CartExtension
    {
        public static ShoppingCartDataModel ToReturnCardData(this ShoppingCartData entity)
        {
            if (entity == null) return null;


            return new ShoppingCartDataModel
            {
                PName = entity.PName,
                PID = entity.PID,
                UnitPrice = entity.UnitPrice,
                Quantity = entity.Quantity,
                
            };
        }
        public static ShoppingCartData ToAddCardData(Product product)
        {
            return new ShoppingCartData
            {
                PName = product.PName,
                PID = product.PID,
                UnitPrice = product.UnitPrice,
                Quantity = 1
            };
        }
    }
}