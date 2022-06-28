using BET.Domain;
using BET.Domain.Interfaces;
using BET.Domain.Models;
using BETWeb.App_Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;


namespace BETWeb.Managers
{
    public class AccountManager : IUsers
    {
   
        private string baseAddress => ConfigurationManager.AppSettings["ServiceProviderUrl"];
     
        public ApiItemResponseModel<UsersModel> LoginUser(string userName, string userPassword)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    HttpResponseMessage response = httpClient.GetAsync($"Users/login-user/v1/{userName}/{userPassword}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var request = response.Content.ReadAsStringAsync().Result;
                        var responseObj = JObject.Parse(request);

                        var model = new UsersModel
                        {
                            Name = responseObj["Item"]["Name"].ToString(),
                            Email = responseObj["Item"]["Email"].ToString(),
                            UserId = (int)responseObj["Item"]["UserId"],
                        };

                        return new ApiItemResponseModel<UsersModel> { IsSuccess = responseObj["IsSuccess"].Value<bool>(),Item = model, Message = responseObj["Message"].ToString() };
                    }
                }
                return new ApiItemResponseModel<UsersModel> { IsSuccess = false, Message = "Error Saving cart" };
            }

            catch (Exception e)
            {
                return new ApiItemResponseModel<UsersModel> { IsSuccess = false, Message = "Exception Error Saving cart" + e };

                throw;
            }
        }

        public ApiResponseModel RegisterUser(UsersModel model)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(baseAddress);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var data = JsonConvert.SerializeObject(model);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync("/Users/registerUses/v1", content).Result;
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

    

      
    }
}