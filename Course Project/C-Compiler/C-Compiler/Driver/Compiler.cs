using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using CodeGeneration;
using LexicalAnalysis;
using Parsing;

namespace Driver
{
    public class Compiler
    {
        private Compiler(string source)
        {
            Source = source;

            // Lexical analysis
            Scanner scanner = new Scanner(source);
            Tokens = scanner.Tokens.ToImmutableList();

            // Parse
            var parserResult = CParsers.Parse(Tokens);
            if (parserResult.Source.Count() != 1)
            {
                throw new InvalidOperationException("Error: not finished parsing");
            }
            SyntaxTree = parserResult.Result;

            // Semantic analysis
            var semantReturn = SyntaxTree.GetTranslnUnit();
            AbstractSyntaxTree = semantReturn.Value;
            Environment = semantReturn.Env;

            // Code generation
            var state = new CGenState();
            AbstractSyntaxTree.CodeGenerate(state);
            Assembly = state.ToString();
        }

        public static Compiler FromSource(string src)
        {
            return new Compiler(src);
        }

        public static Compiler FromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                return new Compiler(File.ReadAllText(fileName));
            }
            throw new FileNotFoundException($"{fileName} does not exist!");
        }

        public void SaveAssembly(string fileName)
        {
            File.WriteAllText(fileName, Assembly);
        }

        public readonly string Source;
        public readonly ImmutableList<Token> Tokens;
        public readonly AST.TranslnUnit SyntaxTree;
        public readonly ABT.TranslnUnit AbstractSyntaxTree;
        public readonly ABT.Env Environment;
        public readonly string Assembly;
    }
}