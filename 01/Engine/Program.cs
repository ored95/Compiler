using System;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = @"../../src/grammar.txt";
            var cfg = new CFG(fileName);

            var newCFG = LeftRecursion.EliminateLeftRecursion(cfg);
            newCFG.ExportToFile(@"../../src/ELR.txt");

            Console.ReadKey();
        }
    }
}
