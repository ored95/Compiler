using System;
using System.Collections.Generic;
using C5;

using state = System.Int32;
using input = System.Char;

namespace Engine
{
    class Machine
    {
        /// <summary>
        /// Numbers of state in last DFA
        /// </summary>
        private static int num = 0;

        /// <summary>
        /// Process that generate DFA from the given regex
        /// </summary>
        /// <param name="in"></param>
        /// <param name="flagShow"></param>
        /// <param name="graphShow"></param>
        /// <returns></returns>
        public static DFA Execute(string @in, bool flagShow, bool graphShow)
        {
            // Preprocess for string
            var rp = new RegexParser(@in);
            
            // Create tree from preprocessed string
            ParseTree parseTree = rp.RegexToTree();

            // Check for a string termination character after parsing the regex
            if (rp.FailAfterRegexParser())
            {
                Console.WriteLine("Parse error: Fail to parse the current regex");
                return null;
            }
            
            // Convert parse tree into NFA
            var nfa = NFA.TreeToNFA(parseTree);
            
            // Process NFA to DFA
            var dfa = ProcessToDFA(nfa);

            // Show results
            if (flagShow)
            {
                rp.PrintTree(parseTree, 1);
                
                nfa.Show();

                dfa.Show();
            }

            if (graphShow)
            {
                Graphviz.SaveNFA(nfa);
                Graphviz.SaveDFA(dfa);
            }

            return dfa;
        }

        /// <summary>
        /// Process that employs the powerset construction or subset construction algorithm.
        /// It creates a DFA that recognizes the same language as the given NFA.
        /// </summary>
        /// <param name="nfa"></param>
        /// <returns></returns>
        private static DFA ProcessToDFA(NFA nfa)
        {
            DFA dfa = new DFA();

            // Sets of NFA states which is represented by some DFA state
            Set<Set<state>> markedStates = new Set<Set<state>>();
            Set<Set<state>> unmarkedStates = new Set<Set<state>>();

            // Gives a number to each state in the DFA
            HashDictionary<Set<state>, state> dfaStateNum = new HashDictionary<Set<state>, state>();

            Set<state> nfaInitial = new Set<state>
            {
                nfa.initial
            };

            // Initially, EpsilonClosure(nfa.initial) is the only state in the DFAs states and it's unmarked.
            Set<state> first = EpsilonClosure(nfa, nfaInitial);
            unmarkedStates.Add(first);

            // The initial dfa state
            state dfaInitial = GenNewState();
            dfaStateNum[first] = dfaInitial;
            dfa.start = dfaInitial;

            while (unmarkedStates.Count > 0)
            {
                // Takes out one unmarked state and posteriorly mark it
                Set<state> aState = unmarkedStates.Choose();

                // Removes from the unmarked set
                unmarkedStates.Remove(aState);

                // Inserts into the marked list
                markedStates.Add(aState);

                // If this state contains the NFA's final state, add it to the DFA's set of
                // final states
                if (aState.Contains(nfa.final))
                {
                    dfa.final.Add(dfaStateNum[aState]);
                }

                // Set our alphabets
                dfa.alphabet = nfa.inputs;
                IEnumerator<input> iE = dfa.alphabet.GetEnumerator();

                // For each input symbol the nfa knows ...
                while (iE.MoveNext())
                {
                    // Next state
                    Set<state> next = EpsilonClosure(nfa, nfa.Move(aState, iE.Current));

                    // If we haven't examined this state before, add it to the unmarkedStates and make up a new number for it.
                    if (!unmarkedStates.Contains(next) && !markedStates.Contains(next))
                    {
                        unmarkedStates.Add(next);
                        dfaStateNum.Add(next, GenNewState());
                    }

                    var transition = new C5.KeyValuePair<state, input>
                    {
                        Key = dfaStateNum[aState],
                        Value = iE.Current
                    };

                    dfa.transTable[transition] = dfaStateNum[next];
                }

                // Collect all states
                dfa.states = new Set<state>
                {
                    dfa.start
                };

                foreach (var tf in  dfa.transTable)
                {
                    if (!dfa.states.Contains(tf.Value))
                        dfa.states.Add(tf.Value);
                }
            }

            return dfa;
        }

        /// <summary>
        /// Builds the Epsilon closure of states for the given NFA 
        /// </summary>
        /// <param name="nfa"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        static Set<state> EpsilonClosure(NFA nfa, Set<state> states)
        {
            // Push all states onto a stack
            Stack<state> uncheckedStack = new Stack<state>(states);

            // Initialize EpsilonClosure(states) to states
            Set<state> epsilonClosure = states;

            while (uncheckedStack.Count > 0)
            {
                // Pop state t, the top element, off the stack
                state t = uncheckedStack.Pop();
                
                int i = 0;

                // For each state u with an edge from t to u labeled Epsilon
                foreach (input input in nfa.transTable[t])
                {
                    if (input == (char)NFA.Const.Epsilon)
                    {
                        state u = Array.IndexOf(nfa.transTable[t], input, i);

                        // If u is not already in epsilonClosure, add it and push it onto stack
                        if (!epsilonClosure.Contains(u))
                        {
                            epsilonClosure.Add(u);
                            uncheckedStack.Push(u);
                        }
                    }

                    i = i + 1;
                }
            }

            return epsilonClosure;
        }

        /// <summary>
        /// Creates unique state numbers for DFA states
        /// </summary>
        /// <returns></returns>
        private static state GenNewState() => ++num;
    }
}
