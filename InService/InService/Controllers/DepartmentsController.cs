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
    public class DepartmentsController : ApiController
    {
        private LocalDBEntities db = new LocalDBEntities();

        #region commented 
        //// GET: api/Departments
        //public IQueryable<Department> GetDepartments()
        //{
        //    return db.Departments;
        //}

        //// GET: api/Departments/5
        //[ResponseType(typeof(Department))]
        //public IHttpActionResult GetDepartment(int id)
        //{
        //    Department department = db.Departments.Find(id);
        //    if (department == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(department);
        //}

        //// PUT: api/Departments/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutDepartment(int id, Department department)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != department.DepartmentId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(department).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DepartmentExists(id))
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

        //// POST: api/Departments
        //[ResponseType(typeof(Department))]
        //public IHttpActionResult PostDepartment(Department department)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Departments.Add(department);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (DepartmentExists(department.DepartmentId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = department.DepartmentId }, department);
        //}

        //// DELETE: api/Departments/5
        //[ResponseType(typeof(Department))]
        //public IHttpActionResult DeleteDepartment(int id)
        //{
        //    Department department = db.Departments.Find(id);
        //    if (department == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Departments.Remove(department);
        //    db.SaveChanges();

        //    return Ok(department);
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

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }

        // GET: api/Locations/{LocationId}/Departments 
        public List<Department> GetDepartments(int locationId)
        {
            if (db.Locations.Count(l => l.LocationId == locationId) > 0)
            {
                return db.Departments.ToList().FindAll(d => d.LocationId == locationId);
            }
            else
            {
                return new List<Department>();
            }
        }

        // GET: api/Locations/{LocationId}/Departments/{DepartmentId}
        [ResponseType(typeof(Department))]
        public IHttpActionResult GetDepartment(int locationId,int departmentId)
        {
            InventoryAgent ia = new InventoryAgent();
            if (ia.DepartmentExists(locationId, departmentId))
            {
                return Ok(db.Departments.Find(departmentId));
            }
            else
            {
                return Ok("data not found");
            }
        }

        // PUT: api/Locations/{LocationId}/Departments/{DepartmentId}
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDepartment(int locationId, int departmentId, Department department)
        {
            if (department.LocationId != locationId)
            {
                return Ok("different location values");
            }

            InventoryAgent ia = new InventoryAgent();
            if (ia.DepartmentExists(locationId, departmentId))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (departmentId != department.DepartmentId)
                {
                    return BadRequest();
                }

                db.Entry(department).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(departmentId))
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

        // POST: api/Locations/{LocationId}/Departments
        [ResponseType(typeof(Department))]
        public IHttpActionResult PostDepartment(int locationId, Department department)
        {
            if (department.LocationId != locationId)
            {
                return Ok("different location values");
            }

            if (db.Locations.Count(l => l.LocationId == locationId) > 0)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Departments.Add(department);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (DepartmentExists(department.DepartmentId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtRoute("DefaultApi", new { id = department.DepartmentId }, department);
            }
            else
            {
                return Ok("data not found");
            }
        }

        // DELETE: api/Locations/{LocationId}/Departments/5
        [ResponseType(typeof(Department))]
        public IHttpActionResult DeleteDepartment(int locationId, int departmentId)
        {
            InventoryAgent ia = new InventoryAgent();
            if (ia.DepartmentExists(locationId, departmentId))
            {
                Department department = db.Departments.Find(departmentId);
                if (department == null)
                {
                    return Ok("department not found");
                }

                db.Departments.Remove(department);
                db.SaveChanges();

                return Ok(department);
            }
            else
            {
                return Ok("data not found");
            }
        }
    }
}