using CodeGeneration;
using LexicalAnalysis;
using Parsing;
using System;
using System.IO;
using System.Linq;

namespace Driver
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());

            var files = Directory
                .GetFiles("../../../C-source")
                .Where(_ => _.EndsWith(".c"));

            var sources = files.Select(File.ReadAllText).ToArray();

            foreach (var source in sources)
            {
                var scanner = new Scanner(source);
                var tokens = scanner.Tokens;
                var env = new ParserEnvironment();
                var input = new ParserInput(env, tokens);
                var parserResult = CParsers.TranslationUnit.Parse(input);
                var semantResult = parserResult.Result.GetTranslnUnit();
                semantResult.Value.CodeGenerate(new CGenState());
            }

            Console.ReadLine();
        }
    }
}
