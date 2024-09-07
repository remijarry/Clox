using System.IO;
using Clox.Printer;
using static Clox.Expr;

namespace Clox;

public class Program
{
    static bool hadError;
    static void Main(string[] args)
    {
        // if (args.Length > 1)
        // {
        //     Console.WriteLine("Usage: clox [script]");
        //     Environment.Exit(64);
        // }``
        // if (args.Length == 1)
        // {
        //     RunFile(args[0]);
        // }
        // else
        // {
        //     RunPrompt();
        // }

        var expression = new Binary(
            new Unary(
                new Token(TokenType.MINUS, "-", null, 1),
                new Literal(123)),
            new(TokenType.STAR, "*", null, 1),
            new Grouping(
                new Literal(45.67)
            ));
        var astPrinter = new AstPrinter();
        Console.WriteLine(astPrinter.Print(expression));
    }

    private static void RunFile(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"The provided file does not exist. {path}");
        }

        var text = File.ReadAllText(path);
        Run(text);

        if (hadError)
        {
            Environment.Exit(65);
        }
    }

    private static void RunPrompt()
    {
        for (; ; )
        {
            Console.WriteLine("> ");
            string line = Console.ReadLine();
            if (line == null)
            {
                break;
            }

            Run(line);
            hadError = false;
        }
    }

    private static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[line {line}] Error {where}: {message}");
        hadError = true;
    }
}