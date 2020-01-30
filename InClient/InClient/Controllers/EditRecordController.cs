using InClient.Models;
using InClient.Util;
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
            SKUDetailViewModel data = new SKUDetailViewModel();
            if (RouteData.Values.ContainsKey("id"))
            {
                string skuId = RouteData.Values["id"].ToString();
                data = InWebService.HttpRequest<SKUDetailViewModel>("Inventory/GetSkuInfo?skuId=" + skuId);
                data.Locations = GetLocations();
                data.Departments = GetDepartments(data.LocationId);
                data.Categories = GetCategories(data.LocationId, data.DepartmentId);
                data.Subcategories = GetSubcategories(data.LocationId, data.DepartmentId, data.CategoryId);
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult SaveRecord(int skuId, string skuName, int subCategoryId)
        {
            return Json("");
        }

        public List<DepartmentModel> GetDepartments(int locationId)
        {
            Session["LocationId"] = locationId;
            List<DepartmentModel> items = new List<DepartmentModel>();
            string dataUrl = "api/Locations/" + locationId + "/Departments";
            items = InWebService.HttpRequest<List<DepartmentModel>>(dataUrl);
            return items != null ? items : new List<DepartmentModel>();
        }

        public List<CategoryModel> GetCategories(int locationId, int departmentId)
        {
            List<CategoryModel> items = new List<CategoryModel>();
            Session["DepartmentId"] = departmentId;
            string dataUrl = "api/Locations/" + locationId + "/Departments/" + departmentId + "/Categories";
            items = InWebService.HttpRequest<List<CategoryModel>>(dataUrl);
            return items != null ? items : new List<CategoryModel>();
        }

        public List<SubcategoryModel> GetSubcategories(int locationId, int departmentId, int categoryId)
        {
            Session["CategoryId"] = categoryId;
            List<SubcategoryModel> items = new List<SubcategoryModel>();
            string dataUrl = "api/Locations/" + locationId + "/Departments/" + departmentId + "/Categories/" + categoryId + "/SubCategories";
            items = InWebService.HttpRequest<List<SubcategoryModel>>(dataUrl);
            return items != null ? items : new List<SubcategoryModel>();
        }

        public List<LocationModel> GetLocations()
        {
            List<LocationModel> locations = new List<LocationModel>();
            locations = InWebService.HttpRequest<List<LocationModel>>("api/Locations");
            return locations != null ? locations : new List<LocationModel>();
        }
    }
}