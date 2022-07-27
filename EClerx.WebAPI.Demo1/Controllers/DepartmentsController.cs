using EClerx.WebAPI.Demo1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace EClerx.WebAPI.Demo1.Controllers
{
    [RoutePrefix("api/Departments")]
    public class DepartmentsController : ApiController
    {
        private readonly HREntities _dbContext = null;

        public DepartmentsController()
        {
            _dbContext = new HREntities();
        }

        [HttpGet]
        [Route("GetAll")]
        //GET: Departments/GetAll
        public IEnumerable<Department> GetAll()
        {
            var departments = _dbContext.Departments.ToList();
            return departments;
        }

        [HttpGet]
        [Route("Get/{departmentCode}", Name = "AddNewDepartment")]
        [ResponseType(typeof(Department))]
        public IHttpActionResult GetDepartmentByCode([FromUri] string departmentCode)
        {
            var department = _dbContext.Departments.FirstOrDefault(x => x.cDepartmentCode == departmentCode);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }


        //api/Departments/AddNew
        [HttpPost]
        [Route("AddNew")]
        [ResponseType(typeof(Department))]
        public IHttpActionResult CreateDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _dbContext.Departments.Add(department);
            _dbContext.SaveChanges();
            return CreatedAtRoute("AddNewDepartment", new { departmentCode = department.cDepartmentCode }, department);
        }

        [HttpPut]
        [Route("UpdateDepartment/{departmentCode}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateDepartment([FromUri]string departmentCode, [FromBody]Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (departmentCode != department.cDepartmentCode)
            {
                return BadRequest();
            }

            _dbContext.Entry(department).State = System.Data.Entity.EntityState.Modified;

            try
            {
                _dbContext.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                var departmentFromDb = _dbContext.Departments.FirstOrDefault(d => d.cDepartmentCode == departmentCode);
                if (departmentFromDb == null)
                {
                    return NotFound();
                }
                else
                {
                    throw ex;
                }
            }
            // return Ok();
            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}
