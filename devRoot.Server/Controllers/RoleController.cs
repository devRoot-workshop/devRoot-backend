using Microsoft.AspNetCore.Mvc;

namespace devRoot.Server.Controllers
{
    public class RoleController : Controller
    {
        private readonly IHostEnvironment hostEnvironment;

        public RoleController(IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult GetRole(string uid)
        {
            return View();
        }
    }
}
