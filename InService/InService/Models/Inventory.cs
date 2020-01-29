using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InService.Models
{
    public class Inventory
    {
        public int SkuId { set; get; }
        public string SkuName { set; get; }
        public string LocationName { set; get; }
        public string DepartmentName { set; get; }
        public string CategoryName { set; get; }
        public string SubCategoryName { set; get; }
    }
}