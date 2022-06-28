using BETEFSotware.App_Data;
using BET.Domain.Models;
using System;

namespace BETEFSotware.Extensions
{
    public static class CheckoutExtesion
    {
        public static Cart ToCheckOutDetails(this CartModel model)
        {
            var cart = new Cart()
            {
                ProductId = model.ProductId,
                Price = Convert.ToString(model.Price),
                Quantity = model.Quantity,
                DatePurchase = DateTime.Now,
                UserId = model.UserId,
                OrderNumber = model.OrderNumber
            };
            return cart;
        }

    }
}