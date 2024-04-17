using Microsoft.AspNetCore.Mvc;
using Sistema.Models;
using Sistema.Data;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace Sistema.Controllers
{
    public class EmployeesController : Controller
    {
        
        
        private readonly ExamenContext _context;

        public EmployeesController(ExamenContext context)
        {
            _context = context;
        }

       /*   public async Task <IActionResult> Delete(){
            return View(await _context.Employees.ToListAsync());
        } */

        public IActionResult Index()
        {
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
                    var employee = JsonConvert.DeserializeObject<Employee>(HttpContext.Session.GetString("empleado"));
                if (HttpContext.Session.GetString("jornada").Equals("1"))
                {   
                    DateHistory newHistory = new DateHistory();
                    newHistory.Entries = DateTime.Now;
                    newHistory.EmployeesId = employee.Id;
                    string historyJson = JsonConvert.SerializeObject(newHistory);
                    HttpContext.Session.SetString("history", historyJson);
                    HttpContext.Session.SetString("jornada", "0");
                }
                return View(employee);
            }
            else{
            return RedirectToAction("Login");

            }

        }

        public IActionResult Login(string Email, string Password)
        {
            var sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
                return RedirectToAction("Index");
            }

            var employees = from employee in _context.Employees select employee;

            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Email))
            {
                employees = employees.Where(x => x.Email.Equals(Email) && x.Password.Equals(Password));
                if (employees.Count() > 0)
                {
                    HttpContext.Session.SetString("sesion", "si");
                    HttpContext.Session.SetString("jornada", "1");
                    var ObjEmployee = employees.First();
                    string employee = JsonConvert.SerializeObject(ObjEmployee);
                    HttpContext.Session.SetString("empleado", employee);
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        public async Task <IActionResult> Logout()
        {
            string? datestring = HttpContext.Session.GetString("history");
            DateHistory date = JsonConvert.DeserializeObject<DateHistory>(datestring);
            if (date != null){
                date.Exits = DateTime.Now;
                await _context.DateHistory.AddAsync(date);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("sesion");
                }
                return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Register(){
           
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Create(Employee employee){
            if(ModelState.IsValid){
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Details(int? id){
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
            if(id != null){
                var user = _context.Employees.Find(id);
                var registros = from register in _context.DateHistory select register;
                registros = registros.Where(x => x.EmployeesId == user.Id);
                ViewData["id"] = user.Id;
                ViewData["name"] = user.Name;
                ViewData["Lastnames"] = user.LastNames;
                ViewData["email"] = user.Email;
                ViewData["phone"] = user.Phone;
                return View(registros);
            }
            return RedirectToAction("Index");
            }else{
                return RedirectToAction("Index");
            }
            
            
        }
        public IActionResult Delete(int? id){
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
            if(id !=  null){
                var user = _context.Employees.Find(id);
            return View(user);
            }
            return RedirectToAction("Index");
            }else{
                return RedirectToAction("Index");
            }
            
        }
        public async Task <IActionResult> confirmDelete(int id){
            var registros = from registro in _context.DateHistory where registro.EmployeesId == id select registro;
            _context.DateHistory.RemoveRange(registros);
            _context.Employees.Remove(_context.Employees.Find(id));
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task <IActionResult> Update(int id){
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
           var user = await _context.Employees.FindAsync(id);
            return View(user);
            }else{
                return RedirectToAction("Index");
            }
            
        }

        public async Task <IActionResult> confirmUpdate(int id, Employee employee){
            if(ModelState.IsValid){
                _context.Employees.Update(employee);
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task <IActionResult> User(){
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
            var user = await _context.Employees.ToListAsync();
            return View(user);
            }else{
                return RedirectToAction("Index");
            }
            
        }
    }
}