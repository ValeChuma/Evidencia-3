using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class HtmlProcessor
{
    public string BasePath { get; set; }

    public HtmlProcessor(string path)
    {
        BasePath = path;
        HtmlFileHandler.LoadIndexFiles("posting.txt", "diccionario_consolidado.txt");
    }


    public string ProcesarTodo()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        HtmlFileHandler handler = new HtmlFileHandler();

        string[] htmlFiles = Directory.GetFiles(BasePath, "*.html");

        handler.OpenAllHtmlFilesInFolder(BasePath);

        float seconds = HtmlFileHandler.RemoveTagsFromFile(htmlFiles);
        float sortSeconds = HtmlFileHandler.CreateAndSortWordList(htmlFiles);
        float consolidadoSeconds = HtmlFileHandler.CreateConsolidatedFrequencyFile(htmlFiles);

        float dicSeconds = HtmlFileHandler.CreateConsolidatedDictionaryFile(
            htmlFiles,
            "diccionario_consolidado.txt",
            "posting.txt"
        );

        HtmlFileHandler.LoadIndexFiles("posting.txt", "diccionario_consolidado.txt");

        stopwatch.Stop();

        return $"Procesado completado en {stopwatch.ElapsedMilliseconds} ms\n" +
               $"RemoveTags: {seconds}\n" +
               $"SortWords: {sortSeconds}\n" +
               $"Crear frecuencias: {consolidadoSeconds}\n" +
               $"Crear diccionario: {dicSeconds}";
    }

    public string BuscarArchivo(string filename)
    {
        return HtmlFileHandler.SearchFileWeb(filename);
    }

    public string BuscarToken(string token)
    {
        return HtmlFileHandler.SearchTokenWeb(token);
    }


    public static string SearchFileWeb(string filename)
    {
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            HtmlFileHandler.SearchFile(filename);
            return sw.ToString();
        }
    }

    public static string SearchTokenWeb(string token)
    {
        using (StringWriter sw = new StringWriter())
        {
            Console.SetOut(sw);
            HtmlFileHandler.SearchToken(token);
            return sw.ToString();
        }
    }


    public string ProcesamientoCompleto()
    {
        StringBuilder log = new StringBuilder();

        log.AppendLine("=== INICIANDO PROCESAMIENTO COMPLETO ===");

        string[] archivos = Directory.GetFiles(BasePath, "*.html");
        log.AppendLine($"{archivos.Length} archivos encontrados.");

        log.AppendLine("Eliminando etiquetas HTML...");
        float removeTags = HtmlFileHandler.RemoveTagsFromFile(archivos);
        log.AppendLine($"RemoveTags: {removeTags} s");

        log.AppendLine("Creando y ordenando lista de palabras...");
        float sortWords = HtmlFileHandler.CreateAndSortWordList(archivos);
        log.AppendLine($"SortWords: {sortWords} s");

        log.AppendLine("Generando frecuencias consolidadas...");
        float freq = HtmlFileHandler.CreateConsolidatedFrequencyFile(archivos);
        log.AppendLine($"Consolidado: {freq} s");

        log.AppendLine("Creando diccionario y posting...");
        float dic = HtmlFileHandler.CreateConsolidatedDictionaryFile(
            archivos,
            "diccionario_consolidado.txt",
            "posting.txt"
        );
        log.AppendLine($"Diccionario: {dic} s");

        HtmlFileHandler.LoadIndexFiles("posting.txt", "diccionario_consolidado.txt");
        log.AppendLine("Índices recargados.");

        log.AppendLine("=== PROCESAMIENTO COMPLETO FINALIZADO ===");

        return log.ToString();
    }
}

