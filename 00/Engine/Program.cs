using System;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            //string src = "a+";
            //string src = "(ab|c)+aab|b";
            string src = "(a|b)*aab";
            bool flag  = true;

            var dfa = Machine.Execute(src, flag);
            
            Console.Write("\n\nInput regex: ");
            string test = Console.ReadLine();

            Console.WriteLine("Result: {0}", dfa.Simulate(test));

            Console.ReadKey();
        }
    }
}
