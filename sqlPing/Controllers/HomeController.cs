using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sqlPing.Models;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace sqlPing.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            var connStr = _config.GetValue<string>("sql");
            try {
                using(var connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand()) {
                        cmd.CommandText = "select @@version";
                        string version = (string)cmd.ExecuteScalar();

                        ViewData["sqlVersion"] = version;
                    }
                } 
            }
            catch(Exception ex) {
                ViewData["connStr"] = connStr;
                ViewData["sqlError"] = ex.ToString();
            }

            return View();
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
