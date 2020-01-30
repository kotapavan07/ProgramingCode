using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InClient.Models
{
    public class SKUSearchViewModel
    {
        public SKUSearchViewModel()
        {
            this.Locations = new List<LocationModel>();
            this.Departments = new List<DepartmentModel>();
            this.Categories = new List<CategoryModel>();
            this.Subcategories = new List<SubcategoryModel>();
            this.Inventories = new List<InventoryModel>();
        }

        public List<LocationModel> Locations { get; set; }
        public List<DepartmentModel> Departments { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<SubcategoryModel> Subcategories { get; set; }
        public List<InventoryModel> Inventories { get; set; }

        public int LocationId { get; set; }
        public int DepartmentId { get; set; }
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }
        public int SkuId { get; set; }

    }
}