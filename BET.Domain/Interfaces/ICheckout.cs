using BET.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BET.Domain.Interfaces
{
   public interface ICheckout
    {
        ApiResponseModel SaveCheckOutInformatoin(CheckoutModel cart);
        ApiResponseModel SendEmailConfirmation(CheckoutModel cart , string orderNumber);
        ApiResponseModel AddUpdateToCart(CartActionsModel model);
        ApiResponseModel GenerateOrderNumber();

        ApiResponseModel AddUpdateQuantity(CartActionsModel model);
    }
}
