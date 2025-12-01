using Microsoft.AspNetCore.Mvc;
using MiSitioWeb.Models;

namespace MiSitioWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Procesar()
        {
            string path = "C:\\Users\\ValCh\\OneDrive\\documentos\\Escuela\\6\\CS13309_Archivos_HTML\\Files";

            HtmlProcessor processor = new HtmlProcessor(path);
            string result = processor.ProcesarTodo();

            return Content(result); 
        }

        public IActionResult BuscarToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Content("Debe proporcionar un token.");

            HtmlProcessor processor = new HtmlProcessor("");
            string result = processor.BuscarToken(token);

            return Content(result);
        }

        public IActionResult BuscarArchivo(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return Content("Debe proporcionar un nombre de archivo.");

            HtmlProcessor processor = new HtmlProcessor("");
            string result = processor.BuscarArchivo(nombre);

            return Content(result);
        }
public IActionResult BuscarTokenAjax(string token)
{
    HtmlFileHandler.LoadIndexFiles("posting.txt", "diccionario_consolidado.txt");

    HtmlProcessor p = new HtmlProcessor("C:\\Users\\ValCh\\OneDrive\\documentos\\Escuela\\6\\CS13309_Archivos_HTML\\Files");
    return Content(p.BuscarToken(token));
}

public IActionResult BuscarArchivoAjax(string archivo)
{
    if (!archivo.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
        archivo += ".html";

    HtmlFileHandler.LoadIndexFiles("posting.txt", "diccionario_consolidado.txt");

    HtmlProcessor p = new HtmlProcessor("C:\\Users\\ValCh\\OneDrive\\documentos\\Escuela\\6\\CS13309_Archivos_HTML\\Files");
    return Content(p.BuscarArchivo(archivo));
}
public IActionResult ProcesarArchivos()
{
    try
    {
        HtmlFileHandler.LoadIndexFiles("posting.txt", "diccionario_consolidado.txt");

        HtmlProcessor p = new HtmlProcessor("C:\\Users\\ValCh\\OneDrive\\documentos\\Escuela\\6\\CS13309_Archivos_HTML\\Files");

        string resultado = p.ProcesamientoCompleto();

        return Content(resultado);
    }
    catch (Exception ex)
    {
        return Content("Error durante el procesamiento: " + ex.Message);
    }
}

public IActionResult AbrirArchivo(string nombre)
{
    if (string.IsNullOrWhiteSpace(nombre))
        return BadRequest("El nombre del archivo es requerido.");

    string basePath = "C:\\Users\\ValCh\\OneDrive\\documentos\\Escuela\\6\\CS13309_Archivos_HTML\\Files";
    
    if (!nombre.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
        nombre += ".html";

    string filePath = Path.Combine(basePath, nombre);

    if (!System.IO.File.Exists(filePath))
        return NotFound("El archivo no existe.");

    try
    {
        string content = System.IO.File.ReadAllText(filePath);
        return Content(content, "text/html");
    }
    catch (Exception ex)
    {
        return StatusCode(500, "Error al abrir el archivo: " + ex.Message);
    }
}



    }
}
