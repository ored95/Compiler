using System;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            string src = "a+";//"(ab|c)+aab|b";

            var rp = new RegexParser(src);

            Console.WriteLine(rp.GetData());

            ParseTree parseTree = rp.RegexToTree();

            // Checking for a string termination character after parsing the regex
            if (rp.FailAfterRegexParser())
            {
                Console.WriteLine("Parse error: Fail to parse the current regex");
                Environment.Exit(1);
            }

            // Show tree after parsing
            rp.PrintTree(parseTree, 1);
            Console.ReadLine();

            // Convert parse tree into NFA
            NFA nfa = NFA.TreeToNFA(parseTree);

            nfa.Show();

            //DFA dfa = SubsetMachine.SubsetConstruct(nfa);

            //dfa.Show();

            //Console.Write("\n\n");

            //Console.Write("Result: {0}", dfa.Simulate(args[2]));

            Console.ReadKey();
        }
    }
}
