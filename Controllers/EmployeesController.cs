using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class EmployeesController : ApiController
    {
        [BasicAuthentication]
        public HttpResponseMessage Get(string gender = "All")
        {
            var username = Thread.CurrentPrincipal.Identity.Name;
            using(PracticeEntities pe = new PracticeEntities())
            {
                switch (username.ToLower())
                {
                    case "male": return Request.CreateResponse(HttpStatusCode.OK, pe.Employees.Where(x => x.Gender.ToLower().Equals("male")).ToList());
                    case "female": return Request.CreateResponse(HttpStatusCode.OK, pe.Employees.Where(x => x.Gender.ToLower().Equals("female")).ToList());
                    default: return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                //switch (gender.ToLower())
                //{
                //    case "all": return Request.CreateResponse(HttpStatusCode.OK, pe.Employees.ToList());
                //    case "male": return Request.CreateResponse(HttpStatusCode.OK, pe.Employees.Where(x => x.Gender.ToLower().Equals("male")).ToList());
                //    case "female": return Request.CreateResponse(HttpStatusCode.OK, pe.Employees.Where(x => x.Gender.ToLower().Equals("female")).ToList());
                //    default: return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"The gender values must be All, Female or Male.{gender} is not a valid value.");
                //}


            }
        }

        public HttpResponseMessage Get(int id)
        {
            using(PracticeEntities pe = new PracticeEntities())
            {
                var entity =  pe.Employees.FirstOrDefault(x => x.ID == id);
                if (entity != null) return Request.CreateResponse(HttpStatusCode.OK, entity);
                else return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"The Employee with ID = {id} is not found");
            }
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (PracticeEntities pe = new PracticeEntities())
                {
                    pe.Employees.Add(employee);
                    pe.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using(PracticeEntities pe = new PracticeEntities())
                {
                    var entity = pe.Employees.FirstOrDefault(x => x.ID == id);
                    if (entity == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"The employee with ID = {id} not found to delete");
                    pe.Employees.Remove(entity);
                    pe.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody]Employee employee)
        {
            try
            {
                using(PracticeEntities pe = new PracticeEntities())
                {
                    var entity = pe.Employees.FirstOrDefault(x => x.ID == id);
                    if (entity == null) return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"The employee with ID = {id} could not be found to update!");
                    entity.FirstName = employee.FirstName;
                    entity.LastName = employee.LastName;
                    entity.Gender = employee.Gender;
                    entity.Salary = employee.Salary;
                    pe.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
