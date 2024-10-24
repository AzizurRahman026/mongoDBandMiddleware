using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace basicMongoDBandMiddleware.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return Ok("Hello");
        }

        [HttpPost("/user")]
        public IActionResult CreateUser([FromBody]Person person)
        {
            Console.WriteLine($"Person Id: {person.Id}\nPerson Name: {person.Name}");
            return Ok(person);
        }
    }
}
