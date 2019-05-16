using System;

namespace C_Compiler
{
    class Program
    {
        public static void Main(string[] args)
        {
            var LA = new Scanner();
            LA.OpenFile(@"../../Test/main.c");

            LA.Lex();

            int current = _translation_unit.Parse(LA.tokens, 0, out TranslationUnit root);

            //Console.Write(root.ToString());
            Console.ReadLine();
        }
    }
}
