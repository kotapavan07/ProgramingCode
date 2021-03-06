﻿using InClient.Models;
using InClient.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace InClient.Controllers
{
    public class DetailsController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            try
            {
                InventoryModel data = new InventoryModel();
                if (RouteData.Values.ContainsKey("id"))
                {
                    string skuId = RouteData.Values["id"].ToString();
                    data = InWebService.HttpRequest<InventoryModel>("Inventory/GetSkuInfo?skuId=" + skuId);
                }
                return View(data);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("Index");
            }
        }
    }
}