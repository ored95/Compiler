using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class LeftRecursion
    {
        private static string Epsilon = "ε";

        public static CFG EliminateLeftRecursion(CFG src)
        {
            var cfg = new CFG(src);
            var A = cfg.Terminal.ToArray();

            var modifiedProductions = new List<KeyValuePair<string, Set<string>>>();

            for (int i = 0; i < A.Length; i++)
            {
                for (int j = 0; j < A.Length; j++)
                {
                    if (j != i)
                    {
                        // 1. Replace each production of the form A[i] -> A[j]Y
                        //    by the productions:
                        //          A[i] -> B[0]Y | B[1]Y | ... | B[k]Y
                        //    where,
                        //          A[j] -> B[0] | B[1] | ... | B[k]
                        //    are all the current A[j] productions
                        foreach (var production in GetProductionByKey(cfg, A[i]))
                        {
                            if (ContainSymbol(production, A[j]))
                            {
                                var jProduction = GetProductionByKey(cfg, A[j]);
                                var remaining = production.Substring(A[j].Length);
                                var mjProduction = Replace(jProduction, remaining);

                                cfg.UpdateProductionByKey(A[i], A[j], mjProduction);
                            }
                        }

                        // 2. Eliminate the direct left recursion from the A[i] productions
                        
                    }
                }
            }

            var g = new CFG();
            g.Terminal = cfg.Terminal;
            g.NonTerminal = cfg.NonTerminal;
            g.StartSymbol = cfg.StartSymbol;

            foreach (var rule in cfg.Production)
            {
                if (isDirectLeftRecursion(rule))
                {
                    var modifiedRule = DirectLeftRecursion(rule);
                    g.Production.Add(modifiedRule[0]);
                    g.Production.Add(modifiedRule[1]);

                    g.Terminal.Add(modifiedRule[1].Key);
                }
                else
                {
                    g.Production.Add(rule);
                }
            }

            return g;
        }

        private static bool ContainSymbol(string src, string value) => src.Substring(0, value.Length) == value;
        
        private static Set<string> GetProductionByKey(CFG cfg, string key)
        {
            var result = new Set<string>();

            foreach (var p in cfg.Production)
            {
                if (p.Key == key)
                {
                    result = p.Value;
                    break;
                }
            }

            return result;      // ALWAYS NOT NULL
        }

        private static Set<string> Replace(Set<string> src, string remaining)
        {
            var re = new Set<string>();

            foreach (var item in src)
                re.Add(item + remaining);

            return re;
        }

        private static bool isDirectLeftRecursion(KeyValuePair<string, Set<string>> rule)
        {
            bool result = false;
            foreach (var r in rule.Value)
            {
                if (r.Substring(0, rule.Key.Length) == rule.Key)
                {
                    result = true;
                    break;
                }    
            }

            return result;
        }

        private static List<KeyValuePair<string, Set<string>>> DirectLeftRecursion(KeyValuePair<string, Set<string>> rule)
        {
            var newRules = new List<KeyValuePair<string, Set<string>>>();
            var LR = new Set<string>();
            var nonLR = new Set<string>();

            foreach (var r in rule.Value)
            {
                if (r.Substring(0, rule.Key.Length) == rule.Key)
                    LR.Add(r);
                else
                    nonLR.Add(r);
            }

            string newSymbol = rule.Key + "'";

            var firstRuleOptions = new Set<string>();

            if (nonLR.Count > 0)
            {
                foreach (var item in nonLR)
                    firstRuleOptions.Add(item + newSymbol);
            }
            else
                firstRuleOptions.Add(newSymbol);

            newRules.Add(new KeyValuePair<string, Set<string>>(rule.Key, firstRuleOptions));

            var secondRuleOptions = new Set<string>();

            foreach (var item in LR)
            {
                secondRuleOptions.Add(item.Substring(rule.Key.Length) + newSymbol);
            }

            secondRuleOptions.Add(Epsilon);

            newRules.Add(new KeyValuePair<string, Set<string>>(newSymbol, secondRuleOptions));

            return newRules;
        }
    }
}
