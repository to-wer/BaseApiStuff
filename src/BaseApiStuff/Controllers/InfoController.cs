using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace BaseApiStuff.Controllers;

public class InfoController : BaseController
{
    // GET: api/Info/Version
    [HttpGet("Version")]
    public IActionResult GetVersion()
    {
        // Get the assembly where this code is executing
        var assembly = Assembly.GetEntryAssembly();

        // Get the assembly version
        if (assembly == null) return Ok(string.Empty);
        var version = assembly.GetName().Version;

        // Convert version to a string
        if (version == null) return Ok(string.Empty);
        var assemblyVersion = version.ToString();

        return Ok(assemblyVersion);
    }
}