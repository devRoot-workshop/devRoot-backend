using System.Collections;

namespace devRoot.Server
{
    public class Utilites
    {
        private readonly devRootContext _context;
        public Utilites(devRootContext context)
        {
            _context = context;
        }

        public bool DbOk()
        {
            return _context.Database.CanConnect();
        }
    }
}
