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
                    var parser = new Parser(tokens);
                    if (parser.Parse())
                        Console.WriteLine("Result: Accepted!\n");
                    else
                        Console.WriteLine("Result: Rejected!\n");
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
