using InService.Models;
using InService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace InService.Controllers
{
    [Authorize]
    [RoutePrefix("Inventory")]
    public class InventoryController : ApiController
    {
        [HttpGet]
        [Route("GetInventoryInfo")]
        [ResponseType(typeof(List<Inventory>))]
        public IHttpActionResult GetInventoryInfo(int locationId, int departmentId, int categoryId, int subCategoryId)
        {
            InventoryAgent ia = new InventoryAgent();
            return Ok<List<Inventory>>(ia.GetInInventories(locationId, departmentId, categoryId, subCategoryId));
        }

        [HttpGet]
        [Route("GetSkuInfo")]
        [ResponseType(typeof(List<Inventory>))]
        public IHttpActionResult GetSkuInfo(int skuId)
        {
            InventoryAgent ia = new InventoryAgent();
            return Ok<Inventory>(ia.GetSku(skuId));
        }
    }
}
