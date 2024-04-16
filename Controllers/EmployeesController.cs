using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UsersCorp.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}