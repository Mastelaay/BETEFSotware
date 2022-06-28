using BET.Domain;
using BET.Domain.Interfaces;
using BETEFSotware.Managers;
using BET.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BETEFSotware.Controllers
{
    [RoutePrefix("Checkout")]
    public class CheckOutController : ApiController
    {
        private IBETUnitOfWork _uow;
        private ICheckout _checkoutManager;

        public CheckOutController(IBETUnitOfWork uow)
        {
            _uow = uow;
            _checkoutManager = new CheckoutManager(_uow);
        }

        [Route("checkout-basket/v1")]
        public ApiResponseModel RegisterUser(CheckoutModel model)
        {
            var request = _checkoutManager.SaveCheckOutInformatoin(model);
            return new ApiResponseModel { IsSuccess = request.IsSuccess, Message = request.Message };
        }

        [Route("addToCart-basket/v1")]
        public ApiResponseModel AddToCart(CartActionsModel model)
        {
            var request = _checkoutManager.AddUpdateToCart(model);
            return new ApiResponseModel { IsSuccess = request.IsSuccess, Message = request.Message };
        }

        [Route("quantity-change/v1")]
        public ApiResponseModel UpdateQuantity(CartActionsModel model)
        {
            var request = _checkoutManager.AddUpdateQuantity(model);
            return new ApiResponseModel { IsSuccess = request.IsSuccess, Message = request.Message };
        }

    }
}
