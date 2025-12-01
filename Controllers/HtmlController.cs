using Microsoft.AspNetCore.Mvc;

public class HtmlController : Controller
{
    public IActionResult Procesar()
    {
        HtmlProcessor processor = new HtmlProcessor("C:\\Users\\ValCh\\OneDrive\\documentos\\Escuela\\6\\CS13309_Archivos_HTML\\Files");
        string resultado = processor.ProcesarTodo();
        return Content(resultado);
    }

    public IActionResult BuscarArchivo(string name)
    {
        HtmlProcessor p = new HtmlProcessor("");
        return Content(p.BuscarArchivo(name));
    }

    public IActionResult BuscarToken(string token)
    {
        HtmlProcessor p = new HtmlProcessor("");
        return Content(p.BuscarToken(token));
    }
}
