using Driver;
using System;
using System.IO;
using System.Linq;

namespace C_Compiler
{
    class Program
    {
        static void Main(String[] args)
        {
            var files = Directory
                .GetFiles("../../Tests")
                .Where(_ => _.EndsWith(".c"));

            Compiler compiler;
            foreach (var file in files)
            {
                compiler = Compiler.FromFile(file);
                compiler.SaveAssembly("../../TestResults/" + Path.GetFileNameWithoutExtension(file) + ".s");
            }

            compiler = null;
        }
    }
}
