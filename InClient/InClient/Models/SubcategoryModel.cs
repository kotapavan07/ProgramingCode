using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InClient.Models
{
    public class SubcategoryModel
    {
        public int SubcategoryId { get; set; }
        public int CategoryId { get; set; }
        public string SubcategoryName { get; set; }
    }
}