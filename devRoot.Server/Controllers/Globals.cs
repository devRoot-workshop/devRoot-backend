namespace devRoot.Server.Controllers;

public class Globals
{
    private readonly IWebHostEnvironment _env;

    public Globals(IWebHostEnvironment env)
    {
        _env = env;
    }
}