using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir); 

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

// Part 2 assignment
// Add an additional function to the Work with files and directories in a .NET app module that generates a sales summary report file.
// The file should contain simple text that shows the actual sales total from the file and a detailed report of each file's total sales.
createSalesSummary(salesTotalDir, salesTotal, salesFiles);

// File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");
// File.WriteAllText(Path.Combine(salesTotalDir, "totals2.txt"), String.Empty);

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);

        // The file name will contain the full path, so only check the end of it
        if (extension == ".json") //(file.EndsWith("sales.json"))
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {  
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

// Part 2 assignment
// Add an additional function to the Work with files and directories in a .NET app module that generates a sales summary report file.
// The file should contain simple text that shows the actual sales total from the file and a detailed report of each file's total sales.
static void createSalesSummary(string salesTotalDir, double salesTotal, IEnumerable<string> salesFiles)
{
    // Create Sales Summary file
    File.WriteAllText(Path.Combine(salesTotalDir, "salesSummary.txt"), String.Empty);

    // Add content to file
    File.AppendAllText(Path.Combine(salesTotalDir, "salesSummary.txt"), $"Sales Summary{Environment.NewLine}");
    File.AppendAllText(Path.Combine(salesTotalDir, "salesSummary.txt"), $"----------------------------{Environment.NewLine}");
    File.AppendAllText(Path.Combine(salesTotalDir, "salesSummary.txt"), $"Total Sales: {salesTotal:C2}{Environment.NewLine}");
    File.AppendAllText(Path.Combine(salesTotalDir, "salesSummary.txt"), Environment.NewLine);
    File.AppendAllText(Path.Combine(salesTotalDir, "salesSummary.txt"), $"Details:{Environment.NewLine}");

    // Add details summary
    foreach (var file in salesFiles)
    {  
        // Get file and folder names
        string folderName = Path.GetFileName(Path.GetDirectoryName(file)) ?? string.Empty;
        string fileName = Path.GetFileNameWithoutExtension(file);
        string folderAndFileName = Path.Combine(folderName, fileName);

        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add filename and data to .txt file
        File.AppendAllText(Path.Combine(salesTotalDir, "salesSummary.txt"), $"  {folderAndFileName}: {data?.Total:C2}{Environment.NewLine}");
    }
}

record SalesData (double Total);