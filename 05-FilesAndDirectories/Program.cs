// Module: Work with files and directories in a .NET app
// Finds every sales.json under ./stores, totals them, appends the total to
// salesTotalDir/totals.txt, and (assignment addition) writes a sales summary report.

using System.Globalization;
using System.Text;
using Newtonsoft.Json;

// Make the :C currency format always print US dollars, whatever the machine locale is.
CultureInfo.CurrentCulture = new CultureInfo("en-US");

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

// Assignment addition: generate the sales summary report.
var reportPath = Path.Combine(salesTotalDir, "salesSummary.txt");
GenerateSalesSummary(salesFiles, storesDirectory, reportPath);
Console.WriteLine(File.ReadAllText(reportPath));
Console.WriteLine($"Report written to {reportPath}");

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);

        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

void GenerateSalesSummary(IEnumerable<string> salesFiles, string storesDirectory, string reportPath)
{
    var details = new StringBuilder();
    double total = 0;

    foreach (var file in salesFiles)
    {
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(File.ReadAllText(file));
        double fileTotal = data?.Total ?? 0;
        total += fileTotal;

        // Several files are named sales.json, so show the path relative to the stores folder.
        string fileName = Path.GetRelativePath(storesDirectory, file);
        details.AppendLine($"  {fileName}: {fileTotal:C}");
    }

    var report = new StringBuilder();
    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");
    report.AppendLine($" Total Sales: {total:C}");
    report.AppendLine();
    report.AppendLine(" Details:");
    report.Append(details);

    File.WriteAllText(reportPath, report.ToString());
}

record SalesData(double Total);
