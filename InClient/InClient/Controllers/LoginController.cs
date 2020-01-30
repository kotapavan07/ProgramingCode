using InClient.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InClient.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Login()
        {
            try
            {
                var client = new HttpClient();

                var authorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(Request.Form["UserName"] + ":secretKey"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basicwe", authorizationHeader);

                var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", Request.Form["UserName"]},
                   {"password", Request.Form["Password"]},
               };

                var tokenResponse = client.PostAsync(ConfigurationManager.AppSettings["WebServiceUrl"] + "/token", new FormUrlEncodedContent(form)).Result;
                InWebService.token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                if (InWebService.token != null && InWebService.token.AccessToken == null)
                {
                    ViewBag.Message = "Invalid login details";
                    return View("Index");
                }
                Console.WriteLine("Token issued is: {0}", InWebService.token.AccessToken);
                return RedirectToAction("Index", "Search");                
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("Index");
            }
        }
    }
}