using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class UselessSymbol
    {
        private static Set<string> Ne;

        /// <summary>
        /// Algorithm check non-empty grammar L(G)
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        private static bool isNotEmptyGrammar(CFG cfg)
        {
            Ne = new Set<string>();

            var tmp = cfg.Terminal.ToArray();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (isCorrectKey(cfg, tmp[i], Ne))
                {
                    Ne.Add(tmp[i]);
                }   
            }
            
            return Ne.Contains(cfg.StartSymbol);
        }

        /// <summary>
        /// check correct key
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="Key"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        private static bool isCorrectKey(CFG cfg, string Key, Set<string> current)
        {
            bool re = false;

            // set of alpha
            var set = cfg.NonTerminal + current;
            
            // Search production rules for key A
            foreach (var rule in cfg.Production)
            {
                if (rule.Key == Key)
                {
                    // option is alpha in rule: A -> alpha
                    foreach (var option in rule.Value)
                    {
                        if (Compatible(option, set))
                        {
                            re = true;
                            break;
                        }
                    }

                    break;
                }
            }

            return re;
        }

        private static int GetTotalLength(string src, string word)
        {
            int i = 0, n = word.Length, counter = 0;

            while (i <= src.Length - n)
            {
                int step = 1;

                if (src.Substring(i, n) == word)
                {
                    counter++;
                    step = n;    
                }

                i += step;
            }

            return counter * n;
        }

        private static bool Compatible(string src, Set<string> options)
        {
            int totalLength = 0;

            foreach (var item in options)
                totalLength += GetTotalLength(src, item);

            return totalLength == src.Length;
        }

        /// <summary>
        /// Get new Grammar from given new non-terminals
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="newTerminal"></param>
        /// <returns></returns>
        private static CFG SubGrammar(CFG cfg, Set<string> newTerminal)
        {
            var subCFG = new CFG();

            subCFG.Terminal = newTerminal;
            subCFG.NonTerminal = cfg.NonTerminal;
            subCFG.StartSymbol = cfg.StartSymbol;

            var totalSet = subCFG.NonTerminal + subCFG.Terminal;

            foreach (var rule in cfg.Production)
            {
                if (subCFG.Terminal.Contains(rule.Key))
                {
                    var ruleOptions = new Set<string>();

                    foreach (var option in rule.Value)
                    {
                        if (Compatible(option, totalSet))
                        {
                            ruleOptions.Add(option);
                        }
                    }

                    subCFG.Production.Add(new KeyValuePair<string, Set<string>>(rule.Key, ruleOptions));
                }
            }

            return subCFG;
        }

        private static CFG EliminateUnreachableSymbol(CFG cfg)
        {
            var subCFG = new CFG();

            var V = new Set<string> { cfg.StartSymbol };
            var totalSet = cfg.NonTerminal + cfg.Terminal;

            bool flag = false;
            while (!flag)
            {
                foreach (var rule in cfg.Production)
                {
                    var newV = V;

                    if (V.Contains(rule.Key))
                    {
                        foreach (var option in rule.Value.ToArray())
                        {
                            if (totalSet.Contains(option))
                                newV.Add(option);
                        }
                    }

                    flag = (V == newV);
                    if (flag)
                        break;

                    V = newV;
                }
            }

            subCFG.StartSymbol = cfg.StartSymbol;
            foreach (var item in V)
            {
                if (cfg.NonTerminal.Contains(item))
                    subCFG.NonTerminal.Add(item);

                if (cfg.Terminal.Contains(item))
                    subCFG.Terminal.Add(item);
            }

            foreach (var rule in cfg.Production)
            {
                if (subCFG.Terminal.Contains(rule.Key))
                {
                    var ruleOptions = new Set<string>();

                    foreach (var option in rule.Value)
                    {
                        if (Compatible(option, V))
                        {
                            ruleOptions.Add(option);
                        }
                    }

                    subCFG.Production.Add(new KeyValuePair<string, Set<string>>(rule.Key, ruleOptions));
                }
            }

            return subCFG;
        }

        public static void Eliminate(CFG cfg)
        {
            cfg.Show("G");

            if (isNotEmptyGrammar(cfg))
            {
                // Step 1: Check non-empty grammar
                var G1 = SubGrammar(cfg, Ne);

                // Step 2: Eliminate unreachable symbols from grammar
                var G2 = EliminateUnreachableSymbol(G1);

                G1.Show("G1");
                G2.Show("G2");

                if (!cfg.Equal(G2))
                    Console.WriteLine("G2 - Eliminated Useless Symbols!");
            }
            else
            {
                Console.WriteLine("Sorry, your grammar is limited!");
            }
        }
    }
}
