using System;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = @"../../src/grammar.txt";
            //var cfg = new CFG(fileName);

            //var newCFG = LeftRecursion.EliminateLeftRecursion(cfg);
            //newCFG.ExportToFile(@"../../src/ELR.txt");

            string testFileName = @"../../src/test.txt";
            var testcfg = new CFG(testFileName);

            UselessSymbol.Eliminate(testcfg);
            Console.ReadKey();
        }
    }
}
