// Module: Write your first C# code
// Reviews basic C# syntax: Console output, literals, variables, and string formatting.

Console.WriteLine("Hello, World!");
Console.Write("Congratulations! ");
Console.Write("You wrote your first lines of code.");
Console.WriteLine();

// Literal values
Console.WriteLine('b');            // char
Console.WriteLine(123);            // int
Console.WriteLine(0.25F);          // float
Console.WriteLine(2.625);          // double
Console.WriteLine(12.39816m);      // decimal
Console.WriteLine(true);           // bool

// Variables and implicitly typed locals
string firstName = "Bob";
int messages = 3;
decimal temperature = 34.4m;
var greeting = "Hello";

Console.WriteLine($"{greeting}, {firstName}! You have {messages} messages in your inbox. The temperature is {temperature} celsius.");
