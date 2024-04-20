using Microsoft.AspNetCore.Mvc;
using Sistema.Models;
using Sistema.Data;
using Sistema.Helpers;
using Sistema.Provaiders;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;


/*
TABLA DE CONTENIDO

1. Internal class transfer
2. Variables readonly
    2.1 Constructor
3. Controladores
    3.1 EntryDate
    3.2 ExitDate
    3.3 Index
    3.9 Details
    3.10 Delete
    3.11 confirmDelete
    3.12 Update
    3.13 confirmUpdate
    3.14 User
*/

namespace Sistema.Controllers
{   
    //1. Internal class transfer
    //Sistema de odt para manejo de datos de ambos models

    internal class Transfer{
        public string Name { get; set; }
        public int Id { get; set; }
        public bool Estado { get; set; }

    }
    public class EmployeesController : Controller
    {

        //2. Variables readonly y constructor
        //variables readonly y constructor que nos crea las instancias para nuestros archivos externos
        private readonly ExamenContext _context;
        private readonly HelperUploadFiles _helperUploadFiles;
        private readonly PathProvaider _pathProvaider;
        private readonly IWebHostEnvironment _hostEnvironment;

        //2.1 Constructor
        public EmployeesController(ExamenContext context, HelperUploadFiles helperUploadFiles, PathProvaider pathProvaider, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _helperUploadFiles = helperUploadFiles;
            _pathProvaider = pathProvaider;
            _hostEnvironment = hostEnvironment;
        }

        //3. Controladores

        //3.1 EntryDate obtener la fecha exacta de cuando se oprime boton de entrada
        public IActionResult EntryDate()
        {

            string? sesion = HttpContext.Session.GetString("sesion");
            Console.WriteLine("esta es la sesion " + sesion);
            if (sesion != null && sesion.Equals("si"))
            {
                    DateHistory register = new DateHistory();
                    int? id = HttpContext.Session.GetInt32("empleado");
                    if (id!= null){
                    register.EmployeesId = (int)HttpContext.Session.GetInt32("empleado");
                    register.Entries = DateTime.Now;
                    _context.DateHistory.Add(register);
                    _context.SaveChanges();
                    }
                
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        //3.2 ExitDate obtener la fecha exacta de cuando se oprime boton de salida
        public IActionResult ExitDate()
        {
             string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
                int? registro = HttpContext.Session.GetInt32("otro");
                if (registro!= null)
                {
                    var date = _context.DateHistory.Find((int)registro);
                    date.Exits = DateTime.Now;
                    _context.DateHistory.Update(date);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        //3.3 Index
        public IActionResult Index()
        {
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
                int? id = HttpContext.Session.GetInt32("empleado");
                var employee = _context.Employees.Find((int)id);
                var TotalRegistres = from e in _context.DateHistory where e.Exits == null && e.EmployeesId == employee.Id select e;
                    Transfer obj = new Transfer();
                    obj.Name = employee.Name;
                    obj.Id = employee.Id;
                    obj.Estado = false;
                
                if (TotalRegistres.Count() > 0)
                {
                    //HttpContext.Session.SetString("registro", TotalRegistres.First().Id.ToString());
                    HttpContext.Session.SetInt32("otro", TotalRegistres.First().Id);
                    obj.Estado = true;
                }

                return View(obj);
            }
            else
            {
                return RedirectToAction("Login", "Uservalidations");

            }

        }

        //3.9 Details detalles del respectivo usuario
        public IActionResult Details(int? id)
        {
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
                if (id != null)
                {
                    var user = _context.Employees.Find(id);
                    var registros = from register in _context.DateHistory select register;
                    registros = registros.Where(x => x.EmployeesId == user.Id);
                    var path = Path.Combine(_hostEnvironment.WebRootPath, "images", user.Image);
                    Console.WriteLine(path);
                    Regex regex = new Regex(@"/images/\w+\W\w+");
                    ViewData["id"] = user.Id;
                    ViewData["image"] = regex.Match(path);
                    ViewData["name"] = user.Name;
                    ViewData["Lastnames"] = user.LastNames;
                    ViewData["email"] = user.Email;
                    ViewData["phone"] = user.Phone;
                    return View(registros);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }


        }

        //3.10 Delete Envia a la view para eliminar
        public IActionResult Delete(int? id)
        {
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
                if (id != null)
                {
                    var user = _context.Employees.Find(id);
                    return View(user);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        //3.11 confirmDelete elimina el registro de la base de datos
        public async Task<IActionResult> confirmDelete(int id)
        {
            var registros = from registro in _context.DateHistory where registro.EmployeesId == id select registro;
            _context.DateHistory.RemoveRange(registros);
            _context.Employees.Remove(_context.Employees.Find(id));
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //3.12 Update Envia a la view para actualizar registro
        public async Task<IActionResult> Update(int id)
        {
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
                var user = await _context.Employees.FindAsync(id);
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        //3.13 confirmUpdate Actualiza el registro de la base de datos        
        public IActionResult confirmUpdate(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(employee);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //3.14 Envia a la tabla de todos los empleados registrado
        public async Task<IActionResult> User()
        {
            string? sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
                var user = await _context.Employees.ToListAsync();
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        
    }
}