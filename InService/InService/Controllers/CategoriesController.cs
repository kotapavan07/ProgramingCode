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
    [MyAuthorize]
    public class CategoriesController : ApiController
    {
        private LocalDBEntities db = new LocalDBEntities();

        #region commented 
        //// GET: api/Categories
        //public IQueryable<Category> GetCategories()
        //{
        //    return db.Categories;
        //}

        //// GET: api/Categories/5
        //[ResponseType(typeof(Category))]
        //public IHttpActionResult GetCategory(int id)
        //{
        //    Category category = db.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(category);
        //}

        //// PUT: api/Categories/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutCategory(int id, Category category)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != category.CategoryId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(category).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CategoryExists(id))
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

        //// POST: api/Categories
        //[ResponseType(typeof(Category))]
        //public IHttpActionResult PostCategory(Category category)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Categories.Add(category);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (CategoryExists(category.CategoryId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        //}

        //// DELETE: api/Categories/5
        //[ResponseType(typeof(Category))]
        //public IHttpActionResult DeleteCategory(int id)
        //{
        //    Category category = db.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Categories.Remove(category);
        //    db.SaveChanges();

        //    return Ok(category);
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

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }

        [AllowAnonymous]
        // GET: api/Locations/{locationId}/Departments/{departmentId}/Categories
        public List<Category> GetCategories(int locationId, int departmentId)
        {
            InventoryAgent ia = new InventoryAgent();
            if (ia.DepartmentExists(locationId, departmentId))
            {
                return db.Categories.ToList().FindAll(c => c.DepartmentId == departmentId);
            }
            else
            {
                return null;
            }
        }

        // GET: api/Locations/{locationId}/Departments/{departmentId}/Categories/{categoryId}
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int locationId, int departmentId, int categoryId)
        {
            InventoryAgent ia = new InventoryAgent();
            if (ia.CategoryExists(locationId, departmentId, categoryId))
            {
                return Ok(db.Categories.Find(categoryId));
            }
            else
            {
                return Ok("category not found");
            }
        }

        // PUT: api/Locations/{locationId}/Departments/{departmentId}/Categories/{categoryId}
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int locationId, int departmentId, int categoryId, Category category)
        {
            if (category.DepartmentId != departmentId)
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

                if (categoryId != category.CategoryId)
                {
                    return BadRequest();
                }

                db.Entry(category).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(categoryId))
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

        // POST: api/Locations/{locationId}/Departments/{departmentId}/Categories
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(int locationId, int departmentId, Category category)
        {
            if (category.DepartmentId != departmentId)
            {
                return Ok("different department values");
            }

            InventoryAgent ia = new InventoryAgent();
            if (ia.DepartmentExists(locationId, departmentId))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Categories.Add(category);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (CategoryExists(category.CategoryId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
            }
            else
            {
                return Ok("data not found");
            }
        }

        // DELETE: api/Locations/{locationId}/Departments/{departmentId}/Categories/{categoryId}
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int locationId, int departmentId, int categoryId)
        {
            InventoryAgent ia = new InventoryAgent();
            if (ia.CategoryExists(locationId, departmentId, categoryId))
            {
                Category category = db.Categories.Find(categoryId);
                if (category == null)
                {
                    return Ok("category not found");
                }

                db.Categories.Remove(category);
                db.SaveChanges();

                return Ok(category);
            }
            else
            {
                return Ok("data not found");
            }
        }
    }
}