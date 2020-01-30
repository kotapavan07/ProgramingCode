using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InClient.Models
{
    public class SKUDetailViewModel
    {
        public SKUDetailViewModel()
        {
            this.Locations = new List<LocationModel>();
            this.Departments = new List<DepartmentModel>();
            this.Categories = new List<CategoryModel>();
            this.Subcategories = new List<SubcategoryModel>();
        }

        public int SkuId { set; get; }
        public string SkuName { set; get; }
        public int LocationId { set; get; }
        public string LocationName { set; get; }
        public int DepartmentId { set; get; }
        public string DepartmentName { set; get; }
        public int CategoryId { set; get; }
        public string CategoryName { set; get; }
        public int SubCategoryId { set; get; }
        public string SubCategoryName { set; get; }

        public List<LocationModel> Locations { get; set; }
        public List<DepartmentModel> Departments { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<SubcategoryModel> Subcategories { get; set; }
    }
}