using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeService.Controllers
{
    class EmployeeClass
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int age { get; set; }
        public double weight { get; set; }
    }

    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        List<EmployeeClass> elist = new List<EmployeeClass>();

        // GET: /<controller>/
        public IActionResult Index()
        {
            elist.Add(new EmployeeClass()
            {
                firstName = "Murali",
                lastName = "Mohan",
                age = 23,
                weight = 65.50
            });

            elist.Add(new EmployeeClass()
            {
                firstName = "Yaswanth",
                lastName = "Kumar",
                age = 22,
                weight = 63.55
            });
            return new JsonResult(elist);
        }
    }
}
