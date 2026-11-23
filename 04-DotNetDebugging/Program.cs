// Module: Interactively debug .NET apps with the Visual Studio Code debugger
// The original code had an off-by-one bug (loop condition `i < n`), found by setting a
// breakpoint and watching n1/n2/sum in the VS Code debugger. Fixed version below.

int result = Fibonacci(5);
Console.WriteLine(result);

static int Fibonacci(int n)
{
    int n1 = 0;
    int n2 = 1;
    int sum;

    for (int i = 2; i <= n; i++)   // bug fix: was `i < n`
    {
        sum = n1 + n2;
        n1 = n2;
        n2 = sum;
    }

    return n == 0 ? n1 : n2;
}
