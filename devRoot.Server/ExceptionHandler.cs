using System.Runtime.CompilerServices;

namespace devRoot.Server
{
    public class ExceptionHandler
    {
        private static ILogger<ExceptionHandler>? _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;    
        }

        public static async Task Handle(Exception e)
        {
            if (_logger != null)
            {
                _logger.LogError("Exception: "+e.ToString());
            }
        }
    }
}
