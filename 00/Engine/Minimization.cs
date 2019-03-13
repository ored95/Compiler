using System.Collections.Generic;
using C5;

using State = System.Int32;
using Input = System.Char;

namespace Engine
{
    class Minimization
    {
        /// <summary>
        /// Minimization DFA by table equivalence
        /// </summary>
        /// <param name="dfa"></param>
        /// <returns></returns>
        public static FSTM TableEquivalence(DFA dfa)
        {
            // Build class of equivalence by generations
            var P = new Set<Set<State>>
            {
                dfa.final,
            };

            // Fixed bug: remove empty set of states
            if ((dfa.states - dfa.final).Count > 0)
                P.Add(dfa.states - dfa.final);

            while (true)
            {
                var newP = Set<Set<State>>.MakeCopy(P);

                foreach (Set<State> e in P)
                {
                    if (e.Count > 1)
                    {
                        // Remove current set
                        newP.Remove(e);

                        // Generate equivalent sets from e
                        foreach (Set<State> vs in ProcessAll(e, dfa.alphabet, P, dfa.transTable))
                            newP.Add(vs);
                    }
                }
                
                if (newP != P)
                {
                    P = Set<Set<State>>.MakeCopy(newP);
                    newP = null;
                }
                else
                    break;
            }

            // Build minimization DFA
            return new FSTM(dfa.start, dfa.final, dfa.alphabet, dfa.transTable, P);
        }

        private static Set<Set<State>> ProcessAll(Set<State> states, SortedArray<Input> alphabet, Set<Set<State>> dict, SortedList<C5.KeyValuePair<State, Input>, State> table)
        {
            Set<Set<State>> vs = null;
            
            var item = ProcessOne(states, alphabet, dict, table);

            if (item != null)
            {
                vs = new Set<Set<State>>
                {
                    item
                };

                var remainStates = ProcessAll(states - item, alphabet, dict, table);
                if (remainStates != null)
                {
                    foreach (var s in remainStates)
                        vs.Add(s);
                }
            }

            return vs;
        }

        private static Set<State> ProcessOne(Set<State> states, SortedArray<Input> alphabet, Set<Set<State>> dict, SortedList<C5.KeyValuePair<State, Input>, State> table)
        {
            Set<State> result = null;

            if (states.Count > 0)
            {
                State s = states.Choose();
                //states.Remove(s);             // Bugs here

                result = new Set<State> { s };

                if (states.Count > 0)
                {
                    foreach (State i in states)
                    {
                        if (isEquivalent(alphabet, s, i, dict, table))
                            result.Add(i);
                    }
                }
            }

            return result;
        }

        private static bool isEquivalent(SortedArray<Input> alphabet, State s1, State s2, Set<Set<State>> dict, SortedList<C5.KeyValuePair<State, Input>, State> table)
        {
            bool result = true;

            foreach (Input @in in alphabet)
            {
                result = result && isEquivalent(@in, s1, s2, dict, table);
            }

            return result;
        }

        private static bool isEquivalent(Input key, State s1, State s2, Set<Set<State>> dict, SortedList<C5.KeyValuePair<State, Input>, State> table)
        {
            State outS1 = table[new C5.KeyValuePair<State, Input>(s1, key)];
            State outS2 = table[new C5.KeyValuePair<State, Input>(s2, key)];

            return isInSameSet(outS1, outS2, dict);
        }

        private static bool isInSameSet(State s1, State s2, Set<Set<State>> dict)
        {
            bool result = false;

            foreach (var set in dict)
            {
                if (set.Contains(s1) && set.Contains(s2))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Minimization DFA by Hopscroft (Debuging..)
        /// Source: https://en.wikipedia.org/wiki/DFA_minimization#Hopcroft's_algorithm
        /// </summary>
        /// <param name="dfa"></param>
        /// <returns></returns>
        public static DFA Hopcroft(DFA dfa)
        {
            var P = new Set<Set<State>>
            {
                dfa.final,
                dfa.states - dfa.final,
            };

            var W = new Set<Set<State>>
            {
                dfa.final
            };

            while (W.Count > 0)
            {
                var A = W.Choose();
                W.Remove(A);

                foreach (Input c in dfa.alphabet)
                {
                    var X = LeadToStateByChr(dfa, c, A);

                    if (X.Count > 0)
                    {
                        foreach (Set<State> Y in P)
                        {
                            Set<State> intersectYX = X * Y,
                                       differentYX = Y - X;

                            if (intersectYX.Count > 0 && differentYX.Count > 0)
                            {
                                P.Remove(Y);
                                P.Add(intersectYX);
                                P.Add(differentYX);

                                if (W.Contains(Y))
                                {
                                    W.Remove(Y);

                                    W.Add(intersectYX);
                                    W.Add(differentYX);
                                }
                                else
                                {
                                    if (intersectYX.Count <= differentYX.Count)
                                        W.Add(intersectYX);
                                    else
                                        W.Add(differentYX);
                                }
                            }
                        }
                    }
                }
            }

            return dfa;
        }

        /// <summary>
        /// let X be the set of states for which a transition on c leads to a state in A
        /// </summary>
        /// <param name="dfa"></param>
        /// <param name="c"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        private static Set<State> LeadToStateByChr(DFA dfa, Input c, Set<State> A)
        {
            var from = new Set<State>();

            foreach (var s in dfa.states)
            {
                bool exist = false;
                foreach (var to in A)
                {
                    if (dfa.transTable[new C5.KeyValuePair<State, Input>(s, c)] == to)
                        exist = true;
                }

                if (exist)
                    from.Add(s);
            }

            return from;
        }
    }
}
