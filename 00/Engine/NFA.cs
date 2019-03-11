using System;
using System.Collections.Generic;
using C5;

using State = System.Int32;
using Input = System.Char;

namespace Engine
{
    class NFA
    {
        public State initial;       // Initial state
        public State final;         // Final state
        private int size;           // Amount of states

        // Inputs this NFA responds to
        public SortedArray<Input> inputs;
        public Input[][] transTable;

        /// <summary>
        /// Provides default values for epsilon and none
        /// </summary>
        public enum Const
        {
            Epsilon = 'ε', 
            None = '\0'
        }

        public NFA(int size, State initial, State final)
        {
            this.initial = initial;
            this.final = final;
            this.size = size;

            IsLegalState(initial);
            IsLegalState(final);

            inputs = new SortedArray<Input>();
            transTable = new Input[size][];

            for (int i = 0; i < size; ++i)
                transTable[i] = new Input[size];
        }

        public NFA(NFA nfa)
        {
            initial = nfa.initial;
            final = nfa.final;
            size = nfa.size;

            transTable = nfa.transTable;
            inputs = nfa.inputs;
        }

        private bool IsLegalState(State s) => (s < 0 || s >= size) ? false : true;

        /// <summary>
        /// Adds a transition between two states.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="in"></param>
        public void AddTrans(State from, State to, Input @in)
        {
            IsLegalState(from);
            IsLegalState(to);

            transTable[from][to] = @in;

            if (@in != (char)Const.Epsilon)
                inputs.Add(@in);
        }

        /// <summary>
        /// Fills states 0 up to other.size with other's states.
        /// </summary>
        /// <param name="other"></param>
        public void FillStates(NFA other)
        {
            for (State i = 0; i < other.size; ++i)
            {
                for (State j = 0; j < other.size; ++j)
                {
                    transTable[i][j] = other.transTable[i][j];
                }
            }

            IEnumerator<Input> ie = other.inputs.GetEnumerator();

            while (ie.MoveNext())
                inputs.Add(ie.Current);
        }

        /// <summary>
        /// Renames all the NFA's states. For each nfa state: number += shift.
        /// Functionally, this doesn't affect the NFA, it only makes it larger and renames
        /// its states.
        /// </summary>
        /// <param name="shift"></param>
        public void ShiftStates(int shift)
        {
            int newSize = size + shift;

            if (shift < 1) return;

            Input[][] newTransTable = new Input[newSize][];
            for (int i = 0; i < newSize; ++i)
                newTransTable[i] = new Input[newSize];

            for (State i = 0; i < size; ++i)
            {
                for (State j = 0; j < size; ++j)
                    newTransTable[i + shift][j + shift] = transTable[i][j];
            }

            size = newSize;
            initial += shift;
            final += shift;
            transTable = newTransTable;
        }

        /// <summary>
        /// Appends a new, empty state to the NFA.
        /// </summary>
        public void AppendEmptyState()
        {
            transTable = Resize(transTable, size + 1);

            size += 1;
        }

        /// <summary>
        /// Resize transTable
        /// </summary>
        /// <param name="transTable"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        private Input[][] Resize(Input[][] transTable, int newSize)
        {
            Input[][] newTransTable = new Input[newSize][];

            for (int i = 0; i < newSize; ++i)
                newTransTable[i] = new Input[newSize];

            for (int i = 0; i <= transTable.Length - 1; i++)
            {
                for (int j = 0; j <= transTable[i].Length - 1; j++)
                {
                    if (transTable[i][j] != '\0')
                        newTransTable[i][j] = transTable[i][j];
                }
            }

            return newTransTable;
        }

        public Set<State> Move(Set<State> states, Input @input)
        {
            Set<State> result = new Set<State>();

            foreach (State s in states)
            {
                int i = 0;

                foreach (Input ip in inputs)
                {
                    // If the transition is on input inp, add it to the resulting set
                    if (ip == @input)
                    {
                        State u = Array.IndexOf(transTable[s], ip, i);

                        if (u >= 0)             // Fixed bug: IndexOutOfRangeException
                            result.Add(u);
                    }

                    i += 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Prints out the NFA.
        /// </summary>
        public void Show()
        {
            Console.WriteLine("This NFA has {0} states: 0 - {1}", size, size - 1);
            Console.WriteLine("The initial state is {0}", initial);
            Console.WriteLine("The final state is {0}\n", final);

            for (State from = 0; from < size; ++from)
            {
                for (State to = 0; to < size; ++to)
                {
                    Input @in = transTable[from][to];

                    if (@in != (char)Const.None)
                    {
                        Console.Write("Transition from {0} to {1} by ", from, to);

                        if (@in == (char)Const.Epsilon)
                            Console.WriteLine("Epsilon");
                        else
                            Console.WriteLine("{0}", @in);
                    }
                }
            }

            Console.WriteLine("\n");
        }

        /// <summary>
        /// Generate NFA from parsed tree
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static NFA TreeToNFA(ParseTree tree)
        {
            switch (tree.type)
            {
                case ParseTree.NodeType.Chr:
                    return BuildNFA_Basic(tree.data.Value);
                case ParseTree.NodeType.Alter:
                    return BuildNFA_Alter(TreeToNFA(tree.left), TreeToNFA(tree.right));
                case ParseTree.NodeType.Concat:
                    return BuildNFA_Concat(TreeToNFA(tree.left), TreeToNFA(tree.right));
                case ParseTree.NodeType.Star:
                    return BuildNFA_Star(TreeToNFA(tree.left));
                case ParseTree.NodeType.Plus:
                    return BuildNFA_Plus(TreeToNFA(tree.left));
                default:
                    return null;
            }
        }

        #region Main Operations
        /////////////////////////////////////////////////////////////////
        //
        // NFA building functions
        //
        // Using Thompson Construction, build NFAs from basic inputs or 
        // compositions of other NFAs.
        //
        /////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Single input NFA
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static NFA BuildNFA_Basic(Input @input)
        {
            NFA basic = new NFA(2, 0, 1);

            basic.AddTrans(0, 1, @input);

            return basic;
        }

        /// <summary>
        /// Builds an alternation of nFA1 and nFA2 (nFA1|nFA2)
        /// </summary>
        /// <param name="nFA1"></param>
        /// <param name="nFA2"></param>
        /// <returns></returns>
        private static NFA BuildNFA_Alter(NFA nFA1, NFA nFA2)
        {
            // make room for the new initial state
            nFA1.ShiftStates(1);

            // make room for nFA1
            nFA2.ShiftStates(nFA1.size);

            // create a new nfa and initialize it with (the shifted) nFA2
            NFA nfa = new NFA(nFA2);

            // nfa1's states take their places in new nfa
            nfa.FillStates(nFA1);

            // Set new initial state and the transitions from it
            nfa.AddTrans(0, nFA1.initial, (char)Const.Epsilon);
            nfa.AddTrans(0, nFA2.initial, (char)Const.Epsilon);

            nfa.initial = 0;

            // Make up space for the new final state
            nfa.AppendEmptyState();

            // Set new final state
            nfa.final = nfa.size - 1;

            // Set new final state and the transitions for it
            nfa.AddTrans(nFA1.final, nfa.final, (char)Const.Epsilon);
            nfa.AddTrans(nFA2.final, nfa.final, (char)Const.Epsilon);

            return nfa;
        }

        /// <summary>
        /// Builds an concaternation of nFA1 and nFA2 (nFA1 . nFA2)
        /// </summary>
        /// <param name="nFA1"></param>
        /// <param name="nFA2"></param>
        /// <returns></returns>
        private static NFA BuildNFA_Concat(NFA nFA1, NFA nFA2)
        {
            // initial state replaced with nFA1's final state
            nFA2.ShiftStates(nFA1.size - 1);

            // Creates a new NFA and initialize it with (the shifted) nFA2
            NFA nfa = new NFA(nFA2);

            // nFA1's states take their places in new NFA
            nfa.FillStates(nFA1);

            // Sets the new initial state (the final state stays nFA2's final state,
            // and was already copied)
            nfa.initial = nFA1.initial;

            return nfa;
        }

        /// <summary>
        /// Builds a star (Kleene closure) of nfa (nfa*)
        /// </summary>
        /// <param name="nfa"></param>
        /// <returns></returns>
        private static NFA BuildNFA_Star(NFA nfa)
        {
            // Makes room for the new initial state
            nfa.ShiftStates(1);

            // Makes room for the new final state
            nfa.AppendEmptyState();

            // Adds new transitions
            nfa.AddTrans(nfa.final, nfa.initial, (char)Const.Epsilon);
            nfa.AddTrans(0, nfa.initial, (char)Const.Epsilon);
            nfa.AddTrans(nfa.final, nfa.size - 1, (char)Const.Epsilon);
            nfa.AddTrans(0, nfa.size - 1, (char)Const.Epsilon);

            nfa.initial = 0;
            nfa.final = nfa.size - 1;

            return nfa;
        }

        /// <summary>
        /// Build an nfa+
        /// </summary>
        /// <param name="nfa"></param>
        /// <returns></returns>
        private static NFA BuildNFA_Plus(NFA nfa)
        {
            // Makes room for the new initial state
            nfa.ShiftStates(1);

            // Makes room for the new final state
            nfa.AppendEmptyState();

            // Adds new transitions
            nfa.AddTrans(nfa.final, nfa.initial, (char)Const.Epsilon);
            nfa.AddTrans(0, nfa.initial, (char)Const.Epsilon);
            nfa.AddTrans(nfa.final, nfa.size - 1, (char)Const.Epsilon);

            nfa.initial = 0;
            nfa.final = nfa.size - 1;

            return nfa;
        }

        #endregion
    }
}
