using InClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InClient.Controllers
{
    public class EditRecordController : Controller
    {
        // GET: EditRecord
        public ActionResult Index()
        {
            SKUViewModel model = new SKUViewModel();
            return View(model);
        }
    }
}