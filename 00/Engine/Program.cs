using System;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            //string src = "a*";
            //string src = "b+";
            string src = "(ab|c)+aab|b";
            //string src = "(a|b)*aab";

            bool flag  = true;
            bool graph = true;

            var dfa = Machine.Execute(src, flag, graph);

            var fsm = Minimization.TableEquivalence(dfa);
            
            if (graph)
                Graphviz.SaveMinimizedDFA(fsm);
            fsm.Show();

            while (true)
            {
                Console.Write("\n\nRegex: {0}\nInput test: ", src);
                string test = Console.ReadLine();

                if (test.Length != 0)
                    Console.WriteLine("Result: {0}", dfa.Simulate(test));
                else
                    Environment.Exit(1);
            }
        }
    }
}
