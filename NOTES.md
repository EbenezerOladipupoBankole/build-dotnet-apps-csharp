# Build .NET Applications with C# – Assignment Notes

**SDK used:** .NET 10.0.404 (`dotnet --version`), the current LTS. All projects target `net10.0`.

| Folder | Microsoft Learn module |
|---|---|
| `01-FirstCSharpCode` | Write your first C# code |
| `02-IntroToDotNet` | Introduction to .NET |
| `03-DotNetDependencies` | Create a new .NET project and work with dependencies (Humanizer) |
| `04-DotNetDebugging` | Interactively debug .NET apps with the VS Code debugger |
| `05-FilesAndDirectories` | Work with files and directories in a .NET app (+ sales summary report) |
| `06-ContosoPizza` | Create a web API with ASP.NET Core controllers |

---

## 1. Existing pizzas plus my additional pizza

`06-ContosoPizza/Services/PizzaService.cs` seed data. I added **BBQ Chicken** (Id 3) and changed `nextId` to 4:

```csharp
Pizzas = new List<Pizza>
{
    new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false },
    new Pizza { Id = 2, Name = "Veggie", IsGlutenFree = true },
    // Additional pizza added for the assignment
    new Pizza { Id = 3, Name = "BBQ Chicken", IsGlutenFree = false }
};
```

Response from `GET http://localhost:5166/pizza` after running the finished app with `dotnet run`:

```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

[{"id":1,"name":"Classic Italian","isGlutenFree":false},{"id":2,"name":"Veggie","isGlutenFree":true},{"id":3,"name":"BBQ Chicken","isGlutenFree":false}]
```

---

## 2. CRUD request and response examples

I started the API with `dotnet run` (http://localhost:5166) and sent the requests below in order with `curl -i`. The same requests are saved in `06-ContosoPizza/ContosoPizza.http`.

### GET: read one pizza. Status **200 OK**

Request:
```
curl -i http://localhost:5166/pizza/3
```
Response:
```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{"id":3,"name":"BBQ Chicken","isGlutenFree":false}
```
(`GET /pizza` for the full list also returned **200 OK**. See section 1.)

### POST: create a pizza. Status **201 Created**

Request:
```
curl -i -X POST http://localhost:5166/pizza -H "Content-Type: application/json" -d '{"name":"Hawaii","isGlutenFree":false}'
```
Response:
```
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8
Location: http://localhost:5166/Pizza/4

{"id":4,"name":"Hawaii","isGlutenFree":false}
```

### PUT: update the pizza. Status **204 No Content**

Request:
```
curl -i -X PUT http://localhost:5166/pizza/4 -H "Content-Type: application/json" -d '{"id":4,"name":"Hawaiian","isGlutenFree":false}'
```
Response:
```
HTTP/1.1 204 No Content
```
Check with `GET /pizza/4`, which returned **200 OK**:
```
{"id":4,"name":"Hawaiian","isGlutenFree":false}
```

### DELETE: delete the pizza. Status **204 No Content**

Request:
```
curl -i -X DELETE http://localhost:5166/pizza/4
```
Response:
```
HTTP/1.1 204 No Content
```
Check with `GET /pizza/4`, which now returns **404 Not Found**:
```
HTTP/1.1 404 Not Found
Content-Type: application/problem+json; charset=utf-8

{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.5","title":"Not Found","status":404,"traceId":"..."}
```

---

## 3. Sales summary function (Work with files and directories)

From `05-FilesAndDirectories/Program.cs`. It is called after the module's `FindFiles` and `CalculateSalesTotal` steps:

```csharp
var reportPath = Path.Combine(salesTotalDir, "salesSummary.txt");
GenerateSalesSummary(salesFiles, storesDirectory, reportPath);
```

```csharp
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
```

The program sets `CultureInfo.CurrentCulture = new CultureInfo("en-US");` at startup so `:C` always prints dollars.

Output in `05-FilesAndDirectories/salesTotalDir/salesSummary.txt`:

```
Sales Summary
----------------------------
 Total Sales: $389,852.53

 Details:
  sales.json: $22,385.32
  201\sales.json: $41,289.55
  202\sales.json: $86,759.12
  203\sales.json: $53,790.08
  204\sales.json: $38,418.67
  204\new\sales.json: $147,209.79
```
