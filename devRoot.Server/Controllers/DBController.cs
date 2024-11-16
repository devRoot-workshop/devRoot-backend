using Microsoft.AspNetCore.Mvc;

namespace devRoot.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DBController : ControllerBase
    {
        private readonly Utilites _utils;

        public DBController(Utilites utils)
        {
            _utils = utils;
        }

        [HttpGet]
        [Route("DBOk")]
        public bool DBOk()
        {
            return _utils.DbOk();
        }
    }
}
