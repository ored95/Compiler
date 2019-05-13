using System;
using System.Collections.Generic;

namespace C_Compiler
{
    class Program
    {
        public static void Main(string[] args)
        {
            var LA = new LexicalAnalysis();
            LA.OpenFile(@"../../Test/main.c");

            LA.Lex();
            //List<Token> tokens = LA.tokens;
            int current = _translation_unit.Parse(LA.tokens, 0, out List<IASTNode> root);
            //Console.Write(current);
            Console.ReadLine();
        }
    }
}
