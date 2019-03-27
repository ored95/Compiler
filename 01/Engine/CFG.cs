using System;
using System.Collections.Generic;
using System.IO;

namespace Engine
{
    class CFG
    {
        public Set<string> Terminal;
        public Set<string> NonTerminal;
        public List<KeyValuePair<string, Set<string>>> Production;
        public string StartSymbol;

        public CFG()
        {
            Terminal = new Set<string>();
            NonTerminal = new Set<string>();
            Production = new List<KeyValuePair<string, Set<string>>>();
            //StartSymbol = null;
        }

        public CFG(Set<string> terminal, Set<string> nonTerminal, List<KeyValuePair<string, Set<string>>> production, string startSymbol)
        {
            Terminal = terminal;
            NonTerminal = nonTerminal;
            Production = production;
            StartSymbol = startSymbol;
        }

        public CFG(CFG cfg)
        {
            Terminal = cfg.Terminal;
            NonTerminal = cfg.NonTerminal;
            Production = cfg.Production;
            StartSymbol = cfg.StartSymbol;
        }

        /// <summary>
        /// Load CFG from file
        /// </summary>
        /// <param name="fileName"></param>
        public CFG(string fileName)
        {
            try
            {
                var lines = File.ReadAllLines(fileName);
                // Check input correct HERE!
                
                int n = int.Parse(lines[0]);
                Terminal = new Set<string>(lines[1].Split());
                
                if (Terminal.Count != n)
                    throw new Exception("Invalid given size of array terminals");

                n = int.Parse(lines[2]);
                NonTerminal = new Set<string>(lines[3].Split());

                if (NonTerminal.Count != n)
                    throw new Exception("Invalid given size of array nonterminals");

                Production = new List<KeyValuePair<string, Set<string>>>();
                n = int.Parse(lines[4]);
                for (int j = 0; j < n; j++)
                {
                    // Split left and right
                    var rule = lines[5 + j].Replace(" ", "").Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                    var options = rule[1].Split('|');

                    bool isExisted = false;
                    foreach (var p in Production)
                    {
                        if (p.Key == rule[0])
                        {
                            isExisted = true;
                            p.Value.AddAll(options);
                        }
                    }

                    if (!isExisted)
                        Production.Add(new KeyValuePair<string, Set<string>>(rule[0], new Set<string>(options)));
                }

                StartSymbol = lines[5 + n].Split()[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void ExportToFile(string fileName)
        {
            using (var file = new StreamWriter(fileName))
            {
                // Terminal
                file.WriteLine("{0}", Terminal.Count);
                file.WriteLine("{0}", string.Join(" ", Terminal));

                // Nonterminal 
                file.WriteLine("{0}", NonTerminal.Count);
                file.WriteLine("{0}", string.Join(" ", NonTerminal));

                // Production rules
                file.WriteLine("{0}", Production.Count);
                foreach (var rule in Production)
                    file.WriteLine("{0} -> {1}", rule.Key, string.Join(" | ", rule.Value));

                // Start state
                file.Write("{0}", StartSymbol);
            }
        }

        public void UpdateProductionByKey(string key, string removeKeyOption, Set<string> updateRuleOptions)
        {
            KeyValuePair<string, Set<string>> tmp = new KeyValuePair<string, Set<string>>();
            Set<string> src = new Set<string>();  
            
            foreach (var rule in Production)
            {
                if (rule.Key == key)
                {
                    tmp = rule;
                    src = rule.Value;

                    var remove = new Set<string>();

                    foreach (var option in src)
                    {
                        if (ContainKey(option, removeKeyOption))
                            remove.Add(option);
                    }

                    src -= remove;      // Remove old production rules

                    src += updateRuleOptions;   // Add new production rules

                    break;
                }
            }

            // Update our productions
            Production.Remove(tmp);
            Production.Add(new KeyValuePair<string, Set<string>>(tmp.Key, src));
        }

        private bool ContainKey(string ruleOptions, string key) => ruleOptions.Substring(0, key.Length) == key;
    }
}
