using FirebaseAdmin.Auth;

namespace devRoot.Server
{
    public partial class Utilities
    {
        private readonly devRootContext _context;
        private readonly IWebHostEnvironment _environment;
        public Utilities(devRootContext context, IWebHostEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        public async Task<UserRecord> GetUserAsync(string uid)
        {
            return await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
        }


        public static Dictionary<string, string> EnvRead(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");

            var _dict = new Dictionary<string, string>();

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length != 2)
                    continue;

                var key = parts[0].Trim();
                var value = parts[1].Trim();
                _dict[key] = value;
            }
            return _dict;
        }
    }
}
