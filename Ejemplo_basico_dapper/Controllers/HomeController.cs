using Ejemplo_basico_dapper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.Data.SqlClient;
using Dapper;

namespace Ejemplo_basico_dapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly string _Conexion;
        public HomeController(ILogger<HomeController> logger, IConfiguration conf)
        {
            _logger = logger;

            _logger.LogInformation(conf.GetConnectionString("ConexionUsuarios"));

            _Conexion = conf.GetConnectionString("ConexionUsuarios");
        }

        // Obtener lista de usuarios de la base de datos y retornar hacia la vista
        public IActionResult Index()
        {

            SqlConnection sqlConnect = new SqlConnection(_Conexion);

            sqlConnect.Open();

            List<UsuariosViewModel> listadeusuarios = new List<UsuariosViewModel>();

            listadeusuarios = sqlConnect.Query<UsuariosViewModel>("Select * from Usuarios").ToList();

            sqlConnect.Close();

            return View(listadeusuarios);
            
        }

        [HttpPost] // Insercción de usuarios en la base de datos
        public IActionResult Index(int ID, string Usuario)
        {

            SqlConnection sqlConnect = new SqlConnection(_Conexion);
            sqlConnect.Open();

            sqlConnect.Query($"insert into Usuarios (ID, Usuario) values ({ID}, '{Usuario}');");

            sqlConnect.Close();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}