using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
                _logger.LogError("Exception: " + e);
            }
        }
    }
}
