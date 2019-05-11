using System;

namespace C_Compiler
{
    class Program
    {
        public static void Main(string[] args)
        {
            var LA = new LexicalAnalysis
            {
                src =
                "int main(int argc, char *argv[]) " +
                "{" +
                "   int x = 0;" +
                "   return x + 2; " +
                "}"
            };

            LA.Lex();
            //List<Token> tokens = LA.tokens;

            Console.Write(LA.ToString());
            Console.ReadLine();
        }
    }
}
