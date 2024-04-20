using Microsoft.AspNetCore.Mvc;
using Sistema.Models;
using Sistema.Data;
using Sistema.Helpers;
using Sistema.Provaiders;
using Microsoft.EntityFrameworkCore;


/*
TABLA DE CONTENIDO

2. Variables readonly
    2.1 Constructor
3. Controladores
    3.4 Login
    3.5 Logout
    3.6 Register
    3.7 Register2
    3.8 Create
*/

namespace Sistema.Controllers
{   
    //1. Internal class transfer
    //Sistema de odt para manejo de datos de ambos models

    public class UservalidationsController : Controller
    {

        //2. Variables readonly y constructor
        //variables readonly y constructor que nos crea las instancias para nuestros archivos externos
        private readonly ExamenContext _context;
        private readonly HelperUploadFiles _helperUploadFiles;
        private readonly PathProvaider _pathProvaider;

        //2.1 Constructor
        public UservalidationsController(ExamenContext context, HelperUploadFiles helperUploadFiles, PathProvaider pathProvaider)
        {
            _context = context;
            _helperUploadFiles = helperUploadFiles;
            _pathProvaider = pathProvaider;
        }

        //3. Controladores

        //3.4 Login
        public IActionResult Login(string Email, string Password)
        {
            var sesion = HttpContext.Session.GetString("sesion");
            if (sesion != null && sesion.Equals("si"))
            {
                return RedirectToAction("Index", "Employees");
            }

            var employees = from employee in _context.Employees select employee;

            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
            {
                employees = employees.Where(x => x.Email.Equals(Email) && x.Password.Equals(Password));
                if (employees.Count() > 0)
                {
                    HttpContext.Session.SetString("sesion", "si");
                    HttpContext.Session.SetInt32("empleado", employees.First().Id);
                    return RedirectToAction("Index", "Employees");
                }else{
                    ViewBag.error = "Usuario o contraseña incorrectos";
                    return View();
                }
            }else{
                    ViewBag.error = "ingresar datos validos";
                    return View();
                }
        }

        //3.5 Logout
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpGet]
        //3.6 Register muestra la view para el formulario
        public IActionResult Register()
        {

            return View();
        }

        //3.7 Register2 muestra la view para el formulario
        [HttpGet]
        public IActionResult Register2()
        {

            return View();
        }

        //3.8 Create crea el nuevo usuario 
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee, IFormFile imagen)
        {   
            string nombreImagen = imagen.FileName;
            string path = await _helperUploadFiles.UploadFileAsync(imagen, nombreImagen, Folders.Images);

            employee.Image = path+"/"+nombreImagen;

            if (ModelState.IsValid)
            {
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Employees");
        }

        
    }
}