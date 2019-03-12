using System;
using System.Collections.Generic;

using State = System.Int32;
using Input = System.Char;
using C5;

namespace Engine
{
    class DFA
    {
        public State start;
        public Set<State> final;
        public SortedList<C5.KeyValuePair<State, Input>, State> transTable;
        public C5.SortedArray<Input> alphabet;
        public Set<State> states;

        public DFA()
        {
            final = new Set<State>();

            transTable = new SortedList<C5.KeyValuePair<State, Input>, State>(new Comparer<int>());
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
            Console.WriteLine("\n\n********** TOTAL: {0} state(s) **********", states.Count);
            Console.WriteLine("DFA start state: {0}", start);
            Console.Write("DFA final state(s): ");

            IEnumerator<State> iE = final.GetEnumerator();

            while (iE.MoveNext())
                Console.Write(iE.Current + " ");

            Console.WriteLine("\n");

            foreach (var kvp in transTable)
                Console.WriteLine("Transition[{0}, {1}] = {2}", kvp.Key.Key, kvp.Key.Value, kvp.Value);
        }
    }

    public class Comparer<T> : IComparer<C5.KeyValuePair<T, Input>>
    {
        int IComparer<C5.KeyValuePair<T, char>>.Compare(C5.KeyValuePair<T, char> x, C5.KeyValuePair<T, char> y)
        {
            if (x.Value == y.Value)
                return x.Key.GetHashCode().CompareTo(y.Key.GetHashCode());
            else
                return x.Value.CompareTo(y.Value);
        }
    }
}
