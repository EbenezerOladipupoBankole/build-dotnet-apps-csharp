// Module: Introduction to .NET
// A small .NET console app that shows the runtime it is running on and does some simple work.

using System.Runtime.InteropServices;

Console.WriteLine("Hello from .NET!");
Console.WriteLine($".NET runtime : {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"OS           : {RuntimeInformation.OSDescription}");

// Small piece of logic: print the first 10 square numbers.
var squares = Enumerable.Range(1, 10).Select(n => n * n);
Console.WriteLine($"First 10 squares: {string.Join(", ", squares)}");
