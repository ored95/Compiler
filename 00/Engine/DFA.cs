using System;
using System.Collections.Generic;

using State = System.Int32;
using Input = System.Char;

namespace Engine
{
    class DFA
    {
        public State start;
        public Set<State> final;
        public SortedList<C5.KeyValuePair<State, Input>, State> transTable;

        public DFA()
        {
            final = new Set<State>();

            transTable = new SortedList<C5.KeyValuePair<State, Input>, State>(new Comparer());
        }

        public string Simulate(string @in)
        {
            State curState = start;

            CharEnumerator i = @in.GetEnumerator();

            while (i.MoveNext())
            {
                var transition = new C5.KeyValuePair<State, Input>(curState, i.Current);

                if (!transTable.ContainsKey(transition))
                    return "Rejected";

                curState = transTable[transition];
            }

            return final.Contains(curState) ? "Accepted" : "Rejected";
        }

        public void Show()
        {
            Console.Write("DFA start state: {0}\n", start);
            Console.Write("DFA final state(s): ");

            IEnumerator<State> iE = final.GetEnumerator();

            while (iE.MoveNext())
                Console.Write(iE.Current + " ");

            Console.WriteLine("\n");

            foreach (var kvp in transTable)
                Console.WriteLine("Transition[{0}, {1}] = {2}", kvp.Key.Key, kvp.Key.Value, kvp.Value);
        }
    }

    public class Comparer : IComparer<C5.KeyValuePair<State, Input>>
    {
        public int Compare(C5.KeyValuePair<int, char> T1, C5.KeyValuePair<int, char> transition2)
        {
            if (T1.Key == transition2.Key)
                return T1.Value.CompareTo(transition2.Value);
            else
                return T1.Key.CompareTo(transition2.Key);
        }
    }
}
