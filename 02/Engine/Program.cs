using System;

namespace Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
                while (true)
                {
                    Console.Write("Input: ");
                    string expression = Console.ReadLine();

                    var tokens = new Tokenizer(expression).Tokenize();
                    var parser = new ArithmeticExpressionParser(tokens);

                    Console.WriteLine("Result: {0}\n", parser.Parse());
                }
            //}
            ///catch (Exception exp)
            //{
              //  Console.WriteLine(exp.ToString());
            //}

            Console.ReadKey();
        }
    }
}
