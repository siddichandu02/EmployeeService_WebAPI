using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;
using System.Threading;

namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {
        [BasicAuthenticationAttribbute]
        public HttpResponseMessage Get(string gender="All")
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                switch(username.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.Employees.Where(e=>e.Gender.ToLower()=="male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateResponse("hello");
                }
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {

                var entity= entities.Employees.FirstOrDefault(e=>e.ID==id);
                if(entity!=null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Employww with id = " + id.ToString() + " not found");
                }

            }
        }
        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                    var messeage = Request.CreateErrorResponse(HttpStatusCode.Created, employee.ToString());
                    messeage.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return messeage;
                }
            }
            catch(Exception ex)
            {
               return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }
    }
}
