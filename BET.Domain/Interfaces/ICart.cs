using BET.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BET.Domain.Interfaces
{
    public interface ICart
    {
        ApiItemsResponseModel<ShoppingCartDataModel> GetShoppingDataList(int loggedInUser);
        decimal GetCartTotalPrice(int loggedInUserId);
        ApiResponseModel QuatityChange(int type, int pId, int loggedInUser);
        ApiResponseModel UpdateTotal();
        ApiResponseModel Clear(int loggedInUser);
        ApiResponseModel AddToCart(int pId, int loggedInUser);
        ApiResponseModel PurchaseItems(UsersModel model);

    }
}
