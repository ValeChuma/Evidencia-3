using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Collections; 

public class HtmlFileHandler
{
    private static readonly string OutputFolder = "C:\\Users\\ValCh\\OneDrive\\documentos\\Escuela\\6\\codigo\\act1\\guarda";
    private static readonly string ResultFolder = "C:\\Users\\ValCh\\OneDrive\\documentos\\Escuela\\6\\codigo\\act1\\Resultados";
    private static readonly string LogFileName = Path.Combine(OutputFolder, "a7_matricula.txt");

    public string ReadHtmlFile(string fileName)
    {
        string filePath = "C:\\Users\\ValCh\\OneDrive\\documentos\\Escuela\\6\\CS13309_Archivos_HTML\\Files" + "\\" + fileName + ".html";
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return null;
        }
    }

    public void OpenHtmlFile(string filePath)
    {
        try
        {

            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening file: {ex.Message}");
        }
    }
    public void OpenAllHtmlFilesInFolder(string folderPath)
    {
        try
        {
            string[] htmlFiles = Directory.GetFiles(folderPath, "*.html");
            foreach (var file in htmlFiles)
            {
                SimulateOpenHtmlFile(file);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening files: {ex.Message}");
        }
    }
    public void SimulateOpenHtmlFile(string filePath)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        Console.WriteLine($"Simulating opening file: {filePath}");
        try
        {
            string content = File.ReadAllText(filePath);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    stopwatch.Stop();
    Console.WriteLine($"Execution time: {stopwatch.Elapsed.TotalMilliseconds:f3} ms");
}
public static string ExtractTextFromHtml(string htmlContent)
{
    if (string.IsNullOrEmpty(htmlContent))
        return string.Empty;

    string textOnly = Regex.Replace(htmlContent, @"<[^>]*>", " ");
    textOnly = System.Net.WebUtility.HtmlDecode(textOnly);
    textOnly = Regex.Replace(textOnly, @"\s+", " ");

    return textOnly.Trim();
}

public static void AppendLog(string message)
{
    try
    {
        if (!Directory.Exists(OutputFolder))
            Directory.CreateDirectory(OutputFolder);

        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}{Environment.NewLine}";
        File.AppendAllText(LogFileName, line, Encoding.ASCII);
    }
    catch
    {
    }
}

    public static float RemoveTagsFromFile(string[] files)
    {
        if (!Directory.Exists(OutputFolder))
            Directory.CreateDirectory(OutputFolder);

        AppendLog("RemoveTagsFromFile START");
        Stopwatch totalStopwatch = Stopwatch.StartNew();

        foreach (var file in files)
        {
            var fileStopwatch = Stopwatch.StartNew();
            try
            {
                string content = File.ReadAllText(file);
                content = ExtractTextFromHtml(content);
                string fileName = Path.GetFileNameWithoutExtension(file);
                string textFilePath = Path.Combine(OutputFolder, $"{fileName}.txt");
                File.WriteAllText(textFilePath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file '{file}': {ex.Message}");
                AppendLog($"Error processing file '{file}': {ex.Message}");
            }
            fileStopwatch.Stop();
            string perMsg = $"Processed '{Path.GetFileName(file)}' in {fileStopwatch.Elapsed.TotalMilliseconds:f3} ms";
            Console.WriteLine(perMsg);
            AppendLog(perMsg);
        }

        totalStopwatch.Stop();
        string totalMsg = $"Total RemoveTagsFromFile time: {totalStopwatch.Elapsed.TotalMilliseconds:f3} ms";
        Console.WriteLine(totalMsg);
        AppendLog(totalMsg);
        return (float)totalStopwatch.Elapsed.TotalSeconds;
    }
public static float CreateAndSortWordList(string[] files)
{
    if (!Directory.Exists(OutputFolder))
        Directory.CreateDirectory(OutputFolder);

    AppendLog("CreateAndSortWordList START");
    Stopwatch totalStopwatch = Stopwatch.StartNew();

    char[] separators = new[] { ' ', '\r', '\n', '\t', ',', '.', ';', ':', '-', '!', '?', '\"', '\'', '(', ')', '[', ']', '{', '}', '/', '\\' };

    foreach (var file in files)
    {
        var fileStopwatch = Stopwatch.StartNew();
        try
        {
            string content = File.ReadAllText(file);
            string text = ExtractTextFromHtml(content);
            var tokens = text
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => Regex.Replace(w.ToLowerInvariant(), @"[^\p{L}\p{N}]+", ""))
                .Where(w => !string.IsNullOrWhiteSpace(w) && !Regex.IsMatch(w, @"\d")) 
                .ToList();

            tokens.Sort(StringComparer.Ordinal);
            string fileName = Path.GetFileNameWithoutExtension(file);
            string outputFilePath = Path.Combine(OutputFolder, $"{fileName}_PalabrasOrdenadas.txt");
            File.WriteAllLines(outputFilePath, tokens);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file '{file}': {ex.Message}");
            AppendLog($"Error processing file '{file}': {ex.Message}");
        }
        fileStopwatch.Stop();
        string perMsg = $"Sorted tokens for '{Path.GetFileName(file)}' in {fileStopwatch.Elapsed.TotalMilliseconds:f3} ms";
        Console.WriteLine(perMsg);
        AppendLog(perMsg);
    }

    totalStopwatch.Stop();
    string totalMsg = $"Total CreateAndSortWordList time: {totalStopwatch.Elapsed.TotalMilliseconds:f3} ms";
    Console.WriteLine(totalMsg);
    AppendLog(totalMsg);

    return (float)totalStopwatch.Elapsed.TotalSeconds;
}
public static float CreateConsolidatedLowercaseSortedFile(string[] files, string outputFileName = "consolidado_palabras.txt")
{
    if (!Directory.Exists(OutputFolder))
        Directory.CreateDirectory(OutputFolder);

    AppendLog("CreateConsolidatedLowercaseSortedFile START");
    var stopwatch = Stopwatch.StartNew();

    var allWords = new List<string>();
    char[] separators = new[] { ' ', '\r', '\n', '\t', ',', '.', ';', ':', '-', '!', '?', '\"', '\'', '(', ')', '[', ']', '{', '}', '/', '\\', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    foreach (var file in files)
    {
        try
        {
            string content = File.ReadAllText(file);
            string text = ExtractTextFromHtml(content);
            var words = text
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim().ToLowerInvariant())
                .Select(w => Regex.Replace(w, @"[^\p{L}\p{N}]+", "")) 
                .Where(w => w.Length > 0 && !Regex.IsMatch(w, @"\d")); 

            allWords.AddRange(words);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file '{file}': {ex.Message}");
            AppendLog($"Error processing file '{file}': {ex.Message}");
        }
    }

    var sorted = allWords.OrderBy(w => w, StringComparer.Ordinal).ToArray();

    string outputPath = Path.Combine(OutputFolder, outputFileName);
    File.WriteAllLines(outputPath, sorted);

    stopwatch.Stop();
    string msg = $"Archivo consolidado guardado en: {outputPath} -- {stopwatch.Elapsed.TotalMilliseconds:f3} ms";
    Console.WriteLine(msg);
    AppendLog(msg);

    return (float)stopwatch.Elapsed.TotalSeconds;
}
public static float CreateConsolidatedFrequencyFile(string[] files, string outputFileName = "consolidado_frecuencias.txt")
{
    if (!Directory.Exists(ResultFolder))
        Directory.CreateDirectory(ResultFolder);

    AppendLog("CreateConsolidatedFrequencyFile START");
    var stopwatch = Stopwatch.StartNew();

    var freq = new Hashtable(); 
    char[] separators = new[] { ' ', '\r', '\n', '\t', ',', '.', ';', ':', '-', '!', '?', '\"', '\'', '(', ')', '[', ']', '{', '}', '/', '\\', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'  };

    foreach (var file in files)
    {
        try
        {
            string content = File.ReadAllText(file);
            string text = ExtractTextFromHtml(content);

            var words = text
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => Regex.Replace(w.ToLowerInvariant(), @"[^\p{L}\p{N}]+", "")) 
                .Where(w => !string.IsNullOrWhiteSpace(w) && !Regex.IsMatch(w, @"\d")); 

            foreach (var w in words)
            {
                if (freq.ContainsKey(w)) freq[w] = (int)freq[w] + 1;
                else freq[w] = 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file '{file}': {ex.Message}");
            AppendLog($"Error processing file '{file}': {ex.Message}");
        }
    }

    var sortedKeys = freq.Keys.Cast<string>().OrderBy(k => k, StringComparer.Ordinal).ToArray();
    string outputPath = Path.Combine(OutputFolder, outputFileName);

    using (var writer = new StreamWriter(outputPath, false))
    {
        foreach (var key in sortedKeys)
        {
            int count = freq.ContainsKey(key) ? (int)freq[key] : 0;
            writer.WriteLine($"{key}\t{count}");
        }
    }

    stopwatch.Stop();
    string msg2 = $"Archivo consolidado de frecuencias guardado en: {outputPath} -- {stopwatch.Elapsed.TotalMilliseconds:f3} ms";
    Console.WriteLine(msg2);
    AppendLog(msg2);

    return (float)stopwatch.Elapsed.TotalSeconds;
}
private static HashSet<string> ReadStopList()
{
    var stopSet = new HashSet<string>(StringComparer.Ordinal);
    try
    {
        if (!Directory.Exists(ResultFolder)) return stopSet;

        string[] candidates = new[]
        {
            Path.Combine(ResultFolder, "StopList"),
            Path.Combine(ResultFolder, "StopList.txt")
        };

        string path = candidates.FirstOrDefault(File.Exists);
        if (path == null) return stopSet;

        foreach (var line in File.ReadAllLines(path, Encoding.ASCII))
        {
            var token = Regex.Replace((line ?? string.Empty).Trim().ToLowerInvariant(), @"[^\p{L}\p{N}]+", "");
            if (string.IsNullOrWhiteSpace(token)) continue;
            if (Regex.IsMatch(token, @"\d")) continue; 
            stopSet.Add(token);
        }
    }
    catch (Exception ex)
    {
        AppendLog($"ReadStopList error: {ex.Message}");
    }
    return stopSet;
}

public static float CreateConsolidatedDictionaryFile(string[] files, string outputFileName = "diccionario_consolidado.txt", string postingFileName = "posting.txt")
{
    if (!Directory.Exists(OutputFolder))
        Directory.CreateDirectory(OutputFolder);

    AppendLog("CreateConsolidatedDictionaryFile START");
    var stopwatch = Stopwatch.StartNew();

    var stopList = ReadStopList();

    var postings = new Hashtable();
    var totalTokens = new Hashtable(); 

    char[] separators = new[] { ' ', '\r', '\n', '\t', ',', '.', ';', ':', '-', '!', '?', '\"', '\'', '(', ')', '[', ']', '{', '}', '/', '\\' };

    foreach (var file in files)
    {
        try
        {
            string content = File.ReadAllText(file);
            string text = ExtractTextFromHtml(content);

            var countsInFile = new Hashtable();
            foreach (var raw in text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
            {
                var token = Regex.Replace(raw.ToLowerInvariant(), @"[^\p{L}\p{N}]+", "");
                if (string.IsNullOrWhiteSpace(token)) continue;
                if (Regex.IsMatch(token, @"\d")) continue;       
                if (stopList.Contains(token)) continue;          

                if (countsInFile.ContainsKey(token)) countsInFile[token] = (int)countsInFile[token] + 1;
                else countsInFile[token] = 1;
            }

            var fileName = Path.GetFileName(file);

            int total = countsInFile.Values.Cast<int>().Sum();
            totalTokens[fileName] = total;

            foreach (DictionaryEntry kv in countsInFile)
            {
                string token = (string)kv.Key;
                int cnt = (int)kv.Value;

                if (!postings.ContainsKey(token))
                {
                    postings[token] = new Hashtable();
                }

                var docDict = (Hashtable)postings[token];
                docDict[fileName] = cnt;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file '{file}': {ex.Message}");
            AppendLog($"Error processing file '{file}': {ex.Message}");
        }
    }

    var sortedKeys = postings.Keys.Cast<string>().OrderBy(k => k, StringComparer.Ordinal).ToArray();
    string postingPath = Path.Combine(ResultFolder, postingFileName);
    var firstPositions = new Hashtable(); 

    using (var writer = new StreamWriter(postingPath, false, Encoding.ASCII))
    {
        int postingIndex = 0;
        foreach (var key in sortedKeys)
        {
            firstPositions[key] = postingIndex;

            var docDict = (Hashtable)postings[key];
            foreach (var docName in docDict.Keys.Cast<string>().OrderBy(d => d, StringComparer.Ordinal))
            {
                int f = (int)docDict[docName];
                int total = totalTokens.ContainsKey(docName) ? (int)totalTokens[docName] : 0;
                double score = total > 0 ? ((double)f * 100.0) / total : 0.0;
                string line = $"{docName}\t{score.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)}%";
                writer.WriteLine(line);
                postingIndex++;
            }
        }
    }
    AppendLog($"Posting guardado en: {postingPath}");

    string dictPath = Path.Combine(ResultFolder, outputFileName);
    using (var writer2 = new StreamWriter(dictPath, false, Encoding.ASCII))
    {
        writer2.WriteLine("Token\t#Documentos\tPosiciónPrimerRegistro");
        foreach (var key in sortedKeys)
        {
            var docsTable = (Hashtable)postings[key];
            int docsWith = docsTable?.Count ?? 0;
            int pos = firstPositions.ContainsKey(key) ? (int)firstPositions[key] : -1;
            writer2.WriteLine($"{key}\t{docsWith}\t{pos}");
        }
    }
    AppendLog($"Diccionario guardado en: {dictPath}");

    stopwatch.Stop();
    string endMsg = $"Tiempo para crear diccionario y posting: {stopwatch.Elapsed.TotalMilliseconds:f3} ms";
    Console.WriteLine(endMsg);
    AppendLog(endMsg);

    return (float)stopwatch.Elapsed.TotalSeconds;
}
    private static string[] PostingLines = null; 
    private static Hashtable DictInfo = new Hashtable(); 

    public static void LoadIndexFiles(string postingFileName = "posting.txt", string dictFileName = "diccionario_consolidado.txt")
    {
        try
        {
            string postingPath = Path.Combine(ResultFolder, postingFileName);
            string dictPath = Path.Combine(ResultFolder, dictFileName);

            if (!File.Exists(postingPath) || !File.Exists(dictPath))
            {
                AppendLog("LoadIndexFiles: posting o diccionario no encontrados.");
                return;
            }

            PostingLines = File.ReadAllLines(postingPath, Encoding.ASCII);

            DictInfo = new Hashtable();
            var dictLines = File.ReadAllLines(dictPath, Encoding.ASCII)
                                .Skip(1); 
            foreach (var line in dictLines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split('\t');
                if (parts.Length < 3) continue;
                string token = parts[0];
                if (!int.TryParse(parts[1], out int docsCount)) continue;
                if (!int.TryParse(parts[2], out int pos)) continue;
                DictInfo[token] = Tuple.Create(docsCount, pos);
            }

            AppendLog($"LoadIndexFiles: cargado posting ({PostingLines.Length} líneas) y diccionario ({DictInfo.Count} tokens).");
        }
        catch (Exception ex)
        {
            AppendLog($"LoadIndexFiles error: {ex.Message}");
        }
    }

    public static void SearchToken(string token)
    {
        token = Regex.Replace(token.ToLowerInvariant(), @"[^\p{L}\p{N}]+", "");
        AppendLog($"SearchToken: {token}");
        if (DictInfo == null || PostingLines == null)
        {
            Console.WriteLine("Índice no cargado. Ejecute LoadIndexFiles primero (se hace automáticamente después del procesamiento).");
            return;
        }

        if (!DictInfo.ContainsKey(token))
        {
            Console.WriteLine($"Token '{token}' no encontrado en el diccionario.");
            return;
        }

        var info = (Tuple<int, int>)DictInfo[token];
        int docsCount = info.Item1;
        int pos = info.Item2;

        Console.WriteLine($"Token: {token}");
        Console.WriteLine($"# Documentos: {docsCount}");
        Console.WriteLine("Postings (Archivo \\t Score):");
        for (int i = pos; i < pos + docsCount && i < PostingLines.Length; i++)
        {
            var parts = PostingLines[i].Split('\t');
            if (parts.Length >= 2)
            {
                string fileName = parts[0];
                string score = parts[1];
                Console.WriteLine($"<a href=\"#\" onclick=\"openHtmlFile('{fileName}'); return false;\">{fileName}</a>\t{score}");
            }
            else
            {
                Console.WriteLine(PostingLines[i]);
            }
        }
    }

    public static void SearchFile(string fileName)
    {
        AppendLog($"SearchFile: {fileName}");
        if (DictInfo == null || PostingLines == null)
        {
            Console.WriteLine("Índice no cargado. Ejecute LoadIndexFiles primero (se hace automáticamente después del procesamiento).");
            return;
        }

        bool foundAny = false;
        foreach (DictionaryEntry de in DictInfo)
        {
            string token = (string)de.Key;
            var info = (Tuple<int, int>)de.Value;
            int docsCount = info.Item1;
            int pos = info.Item2;

            for (int i = pos; i < pos + docsCount && i < PostingLines.Length; i++)
            {
                var parts = PostingLines[i].Split('\t');
                if (parts.Length >= 2)
                {
                    string f = parts[0];
                    string score = parts[1];
                    if (string.Equals(f, fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!foundAny)
                        {
                            Console.WriteLine($"Resultados para archivo: <a href=\"#\" onclick=\"openHtmlFile('{fileName}'); return false;\">{fileName}</a>");
                            Console.WriteLine("Token\tScore");
                            foundAny = true;
                        }
                        Console.WriteLine($"{token}\t{score}");
                    }
                }
            }
            
        }

        if (!foundAny)
        {
            Console.WriteLine($"No se encontraron tokens para el archivo '{fileName}'. Asegúrese de usar exactamente el nombre (ej. 049.html).");
        }
    }
    private static string CaptureConsoleOutput(Action action)
{
    var originalOut = Console.Out;
    using (var sw = new StringWriter())
    {
        Console.SetOut(sw);
        action();
        Console.SetOut(originalOut);
        return sw.ToString();
    }
}
public static string SearchTokenWeb(string token)
{
    return CaptureConsoleOutput(() => SearchToken(token));
}
public static string SearchFileWeb(string fileName)
{
    return CaptureConsoleOutput(() => SearchFile(fileName));
}
public static string OpenAllHtmlFilesWeb(string folderPath)
{
    return CaptureConsoleOutput(() => 
    {
        HtmlFileHandler handler = new HtmlFileHandler();
        handler.OpenAllHtmlFilesInFolder(folderPath);
    });
}

}