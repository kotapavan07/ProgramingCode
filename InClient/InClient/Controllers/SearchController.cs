using InClient.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using InClient.Util;

namespace InClient.Controllers
{
    public class SearchController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            SKUViewModel model = new SKUViewModel();
            model.Locations = GetLocations();
            model.Inventories = GetSearchResults();
            return View(model);
        }

        public List<InventoryModel> GetSearchResults()//int locationId, int departmentId, int categoryId, int subCategoryId)
        {
            int locationId = Session["locationId"] != null ? (int)Session["locationId"] : 0;
            int departmentId = Session["departmentId"] != null ? (int)Session["departmentId"] : 0;
            int categoryId = Session["CategoryId"] != null ? (int)Session["CategoryId"] : 0;
            int subCategoryId = Session["subCategoryId"] != null ? (int)Session["subCategoryId"] : 0;

            string dataUrl = "Inventory/GetInventoryInfo?locationId=" + locationId +
                "&departmentId=" + departmentId + "&categoryId=" + categoryId + "&subCategoryId=" + subCategoryId;

            List<InventoryModel> items = new List<InventoryModel>();
            //items = WebService.GetServiceData(dataUrl).Content.ReadAsAsync<List<InventoryModel>>().Result;
            items = InWebService.HttpRequest<List<InventoryModel>>(dataUrl);
            return items;
        }

        //[HttpPost]
        //public ActionResult GetSearchResults(int locationId, int departmentId, int categoryId, int subCategoryId)
        //{
        //    string dataUrl = "Inventory/GetInventoryInfo?locationId=" + locationId +
        //        "&departmentId=" + departmentId + "&categoryId=" + categoryId + "&subCategoryId=" + subCategoryId;

        //    List<InventoryModel> items = new List<InventoryModel>();
        //    items = GetServiceData(dataUrl).Content.ReadAsAsync<List<InventoryModel>>().Result;

        //    return Json(items);
        //}

        [HttpPost]
        public ActionResult GetDepartments(int locationId)
        {
            Session["LocationId"] = locationId;
            List<DepartmentModel> items = new List<DepartmentModel>();
            string dataUrl = "api/Locations/"+ locationId + "/Departments";
            //items = WebService.GetServiceData(dataUrl).Content.ReadAsAsync<List<DepartmentModel>>().Result;
            items = InWebService.HttpRequest<List<DepartmentModel>>(dataUrl);
            return Json(new SelectList(items, "DepartmentId", "DepartmentName"));
        }

        [HttpPost]
        public ActionResult GetCategories(int locationId, int departmentId)
        {
            List<CategoryModel> items = new List<CategoryModel>();
            Session["DepartmentId"] = departmentId;
            string dataUrl = "api/Locations/" + locationId + "/Departments/" + departmentId + "/Categories";
            //items = WebService.GetServiceData(dataUrl).Content.ReadAsAsync<List<CategoryModel>>().Result;
            items = InWebService.HttpRequest<List<CategoryModel>>(dataUrl);
            return Json(new SelectList(items, "CategoryId", "CategoryName"));
        }

        [HttpPost]
        public ActionResult GetSubcategories(int locationId, int departmentId, int categoryId)
        {
            Session["CategoryId"] = categoryId;
            List<SubcategoryModel> items = new List<SubcategoryModel>();
            string dataUrl = "api/Locations/" + locationId + "/Departments/" + departmentId + "/Categories/"+ categoryId + "/SubCategories";
            //items = WebService.GetServiceData(dataUrl).Content.ReadAsAsync<List<SubcategoryModel>>().Result;
            items = InWebService.HttpRequest<List<SubcategoryModel>>(dataUrl);
            return Json(new SelectList(items, "SubcategoryId", "SubcategoryName"));
        }

        public List<LocationModel> GetLocations()
        {
            List<LocationModel> locations = new List<LocationModel>();
            //locations = WebService.GetServiceData("api/Locations").Content.ReadAsAsync<List<LocationModel>>().Result;
            locations = InWebService.HttpRequest<List<LocationModel>>("api/Locations");
            return locations;
        }

        [HttpGet]
        public void SetSubCategory(int subCategoryId)
        {
            Session["SubcategoryId"] = subCategoryId;
        }

        // Collection for Inventories
        public List<InventoryModel> GetAllInventories()
        {
            List<InventoryModel> items = new List<InventoryModel>();
            //items = GetServiceData("api/Inventory/GetInventoryInfo").Content.ReadAsAsync<List<InventoryModel>>().Result;
            items.Add(new InventoryModel { SkuId = 1, SkuName = "SKUFoods", CategoryName = "Bread", DepartmentName = "Food", LocationName = "HyderabadFoods" });
            items.Add(new InventoryModel { SkuId = 2, SkuName = "SKUBeverages", CategoryName = "Drink", DepartmentName = "Beverages", LocationName = "HyderabadBeverages" });
            return items;
        }

    }
}