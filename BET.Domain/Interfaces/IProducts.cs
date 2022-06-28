using BET.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BET.Domain.Interfaces
{
    public interface IProducts
    {
        ApiItemsResponseModel<ProductsModel> GetProductList();
        ApiItemResponseModel<ProductsModel> GetProductItemById(int pId);
        ApiItemsResponseModel<ProductsModel> GetProductListByCatName(string catName);
        ApiItemsResponseModel<SelectListItem> ProductsDList();
    }
}
