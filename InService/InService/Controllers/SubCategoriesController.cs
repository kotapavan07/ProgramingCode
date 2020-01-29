using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using InService;
using InService.Util;

namespace InService.Controllers
{
    [Authorize]
    public class SubCategoriesController : ApiController
    {
        private LocalDBEntities db = new LocalDBEntities();

        #region commented
        //// GET: api/SubCategories
        //public IQueryable<SubCategory> GetSubCategories()
        //{
        //    return db.SubCategories;
        //}

        //// GET: api/SubCategories/5
        //[ResponseType(typeof(SubCategory))]
        //public IHttpActionResult GetSubCategory(int id)
        //{
        //    SubCategory subCategory = db.SubCategories.Find(id);
        //    if (subCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(subCategory);
        //}

        //// PUT: api/SubCategories/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutSubCategory(int id, SubCategory subCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != subCategory.SubCategoryId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(subCategory).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SubCategoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/SubCategories
        //[ResponseType(typeof(SubCategory))]
        //public IHttpActionResult PostSubCategory(SubCategory subCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.SubCategories.Add(subCategory);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (SubCategoryExists(subCategory.SubCategoryId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = subCategory.SubCategoryId }, subCategory);
        //}

        //// DELETE: api/SubCategories/5
        //[ResponseType(typeof(SubCategory))]
        //public IHttpActionResult DeleteSubCategory(int id)
        //{
        //    SubCategory subCategory = db.SubCategories.Find(id);
        //    if (subCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    db.SubCategories.Remove(subCategory);
        //    db.SaveChanges();

        //    return Ok(subCategory);
        //}
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubCategoryExists(int id)
        {
            return db.SubCategories.Count(e => e.SubCategoryId == id) > 0;
        }

        // GET: api/Locations/{locationId}/Departments/{departmentId}/Categories/{categoryId}/SubCategories
        public List<SubCategory> GetSubCategories(int locationId, int departmentId, int categoryId)
        {
            InventoryAgent ia = new InventoryAgent();
            if (ia.CategoryExists(locationId, departmentId, categoryId))
            {
                return db.SubCategories.ToList().FindAll(sc => sc.CategoryId == categoryId);
            }
            else
            {
                return new List<SubCategory>();
            }
        }

        // GET: api/Locations/{locationId}/Departments/{departmentId}/Categories/{categoryId}/SubCategories/{subCategoryId}
        [ResponseType(typeof(SubCategory))]
        public IHttpActionResult GetSubCategory(int locationId, int departmentId, int categoryId, int subCategoryId)
        {
            InventoryAgent ia = new InventoryAgent();
            if (ia.SubCategoryExists(locationId, departmentId, categoryId, subCategoryId))
            {
                return Ok(db.SubCategories.Find(subCategoryId));
            }
            else
            {
                return Ok("subcategory not found");
            }
        }

        // PUT: api/Locations/{locationId}/Departments/{departmentId}/Categories/{categoryId}/SubCategories/{subCategoryId}
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubCategory(int locationId, int departmentId, int categoryId, int subCategoryId, SubCategory subCategory)
        {
            if (subCategory.CategoryId != categoryId)
            {
                return Ok("different category values");
            }

            InventoryAgent ia = new InventoryAgent();
            if (ia.SubCategoryExists(locationId, departmentId, categoryId, subCategoryId))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (subCategoryId != subCategory.SubCategoryId)
                {
                    return BadRequest();
                }

                db.Entry(subCategory).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(subCategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return Ok("data not found");
            }
        }

        // POST: api/Locations/{locationId}/Departments/{departmentId}/Categories/{categoryId}/SubCategories
        [ResponseType(typeof(SubCategory))]
        public IHttpActionResult PostSubCategory(int locationId, int departmentId, int categoryId, SubCategory subCategory)
        {
            if (subCategory.CategoryId != categoryId)
            {
                return Ok("different department values");
            }

            InventoryAgent ia = new InventoryAgent();
            if (ia.CategoryExists(locationId, departmentId, categoryId))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.SubCategories.Add(subCategory);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (SubCategoryExists(subCategory.SubCategoryId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtRoute("DefaultApi", new { id = subCategory.SubCategoryId }, subCategory);
            }
            else
            {
                return Ok("data not found");
            }
        }

        // DELETE: api/Locations/{locationId}/Departments/{departmentId}/Categories/{categoryId}/SubCategories/{subCategoryId}
        [ResponseType(typeof(SubCategory))]
        public IHttpActionResult DeleteSubCategory(int locationId, int departmentId, int categoryId, int subCategoryId)
        {
            InventoryAgent ia = new InventoryAgent();
            if (ia.SubCategoryExists(locationId, departmentId, categoryId, subCategoryId))
            {
                SubCategory subCategory = db.SubCategories.Find(subCategoryId);
                if (subCategory == null)
                {
                    return NotFound();
                }

                db.SubCategories.Remove(subCategory);
                db.SaveChanges();

                return Ok(subCategory);
            }
            else
            {
                return Ok("data not found");
            }
        }
    }
}