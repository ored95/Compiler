using System;
using System.Collections.Generic;

using State = System.Int32;
using Input = System.Char;


namespace Engine
{
    class FSTM
    {
        private Set<Set<State>> start;
        private Set<Set<State>> final;
        private C5.SortedArray<Input> alphabet;
        private SortedList<C5.KeyValuePair<Set<State>, Input>, Set<State>> transTable;
        private Set<Set<State>> states;

        public FSTM(State start, Set<State> final, C5.SortedArray<Input> alphabet, SortedList<C5.KeyValuePair<State, Input>, State> transTable, Set<Set<State>> states)
        {
            // Initialization
            this.start = new Set<Set<State>>();
            this.final = new Set<Set<State>>();
            this.transTable = new SortedList<C5.KeyValuePair<Set<State>, Input>, Set<State>>(new Comparer<Set<State>>());

            // Fill states
            this.states = Set<Set<State>>.MakeCopy(states);

            // Fill alphabet
            this.alphabet = alphabet;

            // Fill start
            foreach (var s in states)
            {
                if (s.Contains(start))
                    this.start.Add(s);
            }

            // Fill final
            foreach (var finalState in final)
            {
                foreach (var s in states)
                {
                    if (s.Contains(finalState))
                        this.final.Add(s);
                }
            }

            // Fill transtable by alphabet
            foreach (var state in states)
            {
                var item = state.Choose();
                foreach (Input @in in this.alphabet)
                {
                    var linkedState = transTable[new C5.KeyValuePair<State, Input>(item, @in)];
                    foreach (var accepted in states)
                    {
                        if (accepted.Contains(linkedState))
                        {
                            this.transTable.Add(
                                new C5.KeyValuePair<Set<State>, Input>(Set<State>.MakeCopy(state), @in), 
                                Set<State>.MakeCopy(accepted)
                            );
                        }
                    }
                }
            }
        }
        
        public void Show()
        {
            Console.WriteLine("\n\n********** TOTAL: {0} state(s) **********", states.Count);
            Console.WriteLine("FSTM start state: {0}", start.ToString());
            Console.WriteLine("FSTM final state: {0}\n", final.ToString());
            
            foreach (var kvp in transTable)
                Console.WriteLine("Transition[{0}, {1}] = {2}", kvp.Key.Key.ToString(), kvp.Key.Value, kvp.Value.ToString());
        }
    }
}
