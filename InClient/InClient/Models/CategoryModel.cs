using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InClient.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public int DepartmentId { get; set; }
        public string CategoryName { get; set; }
    }
}