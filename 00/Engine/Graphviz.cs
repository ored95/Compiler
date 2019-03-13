using System.IO;

using State = System.Int32;
using Input = System.Char;
using System.Collections.Generic;

namespace Engine
{
    class Graphviz
    {
        private static string Extension = ".gv";
        private static string directoryPath = @"..\\..\\gv";

        private static string getLocation(string fileName)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return directoryPath + "\\" + fileName + Extension;
        }

        public static void SaveNFA(NFA nfa, string fileName = "NFA")
        {
            using (FileStream target = File.Create(getLocation(fileName)))
            {
                using (StreamWriter sw = new StreamWriter(target))
                {
                    sw.WriteLine("digraph {");

                    sw.WriteLine("\t{0} [style=filled, fillcolor=green]", nfa.initial);

                    sw.WriteLine("\t{0} [style=filled, fillcolor=red]", nfa.final);

                    for (State from = 0; from < nfa.transTable.Length; ++from)
                    {
                        for (State to = 0; to < nfa.transTable[0].Length; ++to)
                        {
                            Input @in = nfa.transTable[from][to];

                            if (@in != (char)NFA.Const.None)
                            {
                                sw.WriteLine("\t{0} -> {1} [label=\"{2}\"]", from, to, (char)@in);
                            }
                        }
                    }

                    sw.Write("}");
                }
            }
        }

        public static void SaveDFA(DFA dfa, string fileName="DFA")
        {
            using (FileStream target = File.Create(getLocation(fileName)))
            {
                using (StreamWriter sw = new StreamWriter(target))
                {
                    sw.WriteLine("digraph {");

                    sw.WriteLine("\t{0} [style=filled, fillcolor=green]", dfa.start);

                    foreach (State s in dfa.final)
                    {
                        sw.WriteLine("\t{0} [style=filled, fillcolor=red]", s);
                    }

                    foreach (var kvp in dfa.transTable)
                    {
                        sw.WriteLine("\t{0} -> {1} [label=\"{2}\"]", kvp.Key.Key, kvp.Value, kvp.Key.Value);
                    }
                    
                    sw.Write("}");
                }
            }
        }

        public static void SaveMinimizedDFA(FSTM fst, string fileName="FST")
        {
            using (FileStream target = File.Create(getLocation(fileName)))
            {
                using (StreamWriter sw = new StreamWriter(target))
                {
                    sw.WriteLine("digraph {");

                    Dictionary<State, Set<State>> dict = new Dictionary<State, Set<State>>();

                    var tmp = fst.states;

                    while (tmp.Count > 0)
                    {
                        var item = tmp.Choose();

                        dict.Add(tmp.Count, item);

                        string result = string.Format("\t{0} [label=\"{1}\"", tmp.Count, item);

                        string extend = fst.start.Contains(item) ? ", style=filled, fillcolor=green]" :
                                            (fst.final.Contains(item) ? ", style=filled, fillcolor=red]" : "]");

                        sw.WriteLine(result + extend);

                        tmp.Remove(item);
                    }

                    foreach (var link in fst.transTable)
                    {
                        sw.WriteLine("\t{0} -> {1} [label=\"{2}\"]", getState(link.Key.Key, dict), getState(link.Value, dict), link.Key.Value);
                    }

                    sw.Write("}");
                }
            }
        }

        private static State getState(Set<State> value, Dictionary<State, Set<State>> dict)
        {
            State index = -1;

            foreach (var item in dict)
            {
                if (item.Value == value)
                {
                    index = item.Key;
                    break;
                }
            }

            return index;
        }
    }
}
