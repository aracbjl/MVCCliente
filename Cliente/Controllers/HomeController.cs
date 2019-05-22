using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cliente.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace Cliente.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }
        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IActionResult Index()
        {
        
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Clientes cliente)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:SQLConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = $"Insert Into Cliente (Nombre_Cliente, Ciudad, Tiempo_Curso) Values ('{cliente.Nombre_Cliente}', '{cliente.Ciudad}','{cliente.Tiempo_Curso}')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        connection.Dispose();
                    }
                    return RedirectToAction("List");
                }
            }
            else
                return View();
        }

        public IActionResult List()
        {
            List<Clientes> clienteList = new List<Clientes>();
            string connectionString = Configuration["ConnectionStrings:SQLConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataReader
                connection.Open();

                string sql = "Select * From Cliente"; SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Clientes cliente = new Clientes();
                        cliente.id = Convert.ToInt32(dataReader["Id"]);
                        cliente.Nombre_Cliente = Convert.ToString(dataReader["Nombre_Cliente"]);
                        cliente.Ciudad = Convert.ToString(dataReader["Ciudad"]);
                        cliente.Tiempo_Curso = Convert.ToInt32(dataReader["Tiempo_Curso"]);
                        //alumno.FechaNacimientoAlumno = Convert.ToDateTime(dataReader["FechaNacimientoAlumno"]);
                        clienteList.Add(cliente);
                    }
                }
                connection.Close();
                connection.Dispose();
            }
            return View(clienteList);
        }

        public IActionResult Update(int id)
        {
            string connectionString = Configuration["ConnectionStrings:SQLConnection"];
            Clientes cliente = new Clientes();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * From Cliente Where Id='{id}'";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        cliente.id = Convert.ToInt32(dataReader["Id"]);
                        cliente.Nombre_Cliente = Convert.ToString(dataReader["Nombre_Cliente"]);
                        cliente.Ciudad = Convert.ToString(dataReader["Ciudad"]);
                        cliente.Tiempo_Curso = Convert.ToInt32(dataReader["Tiempo_Curso"]);
                        //alumno.FechaNacimientoAlumno = Convert.ToDateTime(dataReader["FechaNacimientoAlumno"]);
                    }
                }
                connection.Close();
                connection.Dispose();
            }
            return View(cliente);
        }
        [HttpPost]
        [ActionName("Update")]
        public IActionResult Update(Clientes cliente)
        {
            string connectionString = Configuration["ConnectionStrings:SQLConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update Cliente SET Nombre_Cliente='{cliente.Nombre_Cliente}', Ciudad='{cliente.Ciudad}', Tiempo_Curso='{cliente.Tiempo_Curso}' Where id='{cliente.id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                }
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            string connectionString = Configuration["ConnectionStrings:SQLConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Delete From Cliente Where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        ViewBag.Result = "Operation error:" + ex.Message;
                    }
                    connection.Close();
                    connection.Dispose();
                }
            }
            return RedirectToAction("List");
        }

        public IActionResult Details(int id)
        {
            string connectionString = Configuration["ConnectionStrings:SQLConnection"];
            Clientes cliente = new Clientes();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * From Cliente Where Id='{id}'";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        cliente.id = Convert.ToInt32(dataReader["Id"]);
                        cliente.Nombre_Cliente = Convert.ToString(dataReader["Nombre_Cliente"]);
                        cliente.Ciudad = Convert.ToString(dataReader["Ciudad"]);
                        cliente.Tiempo_Curso = Convert.ToInt32(dataReader["Tiempo_Curso"]);
                        //alumno.FechaNacimientoAlumno = Convert.ToDateTime(dataReader["FechaNacimientoAlumno"]);
                    }
                }
                connection.Close();
                connection.Dispose();
            }
            return View(cliente);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
