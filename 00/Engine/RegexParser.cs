using System;
using System.Text;

namespace Engine
{
    class RegexParser
    {
        private string data;
        private int next = 0;

        public RegexParser(string data=null)
        {
            this.data = PreProcess(data);
            next = 0;
        }

        /// <summary>
        /// Get the current char without changing index
        /// </summary>
        /// <returns></returns>
        private char Peek() => (next < data.Length) ? data[next] : '\0';

        public bool FailAfterRegexParser() => Peek() != '\0';

        /// <summary>
        /// Get the current char and increase index
        /// </summary>
        /// <returns></returns>
        private char Pop()
        {
            char current = Peek();

            if (next < data.Length)
                ++next;

            return current;
        }

        /// <summary>
        /// Get the current index of char
        /// </summary>
        /// <returns></returns>
        private int GetPosition() => next;

        private string PreProcess(string @in)
        {
            StringBuilder @out = new StringBuilder();

            CharEnumerator c, up;

            c = @in.GetEnumerator();
            up = @in.GetEnumerator();

            up.MoveNext();

            while (up.MoveNext())
            {
                c.MoveNext();
                @out.Append(c.Current);

                if ((char.IsLetterOrDigit(c.Current) || c.Current == ')' || c.Current == '*' || c.Current == '+')
                    && (up.Current != ')' && (up.Current != '|' && up.Current != '*' && up.Current != '+')))
                    @out.Append('.');
            }

            // Do not forget the last char
            if (c.MoveNext())
                @out.Append(c.Current);

            return @out.ToString();
        }

        public string GetData() => data;

        #region Operations with Tree

        private void MessageError(string error, int exitCode=1)
        {
            Console.WriteLine(error);

            Console.ReadKey();
            Environment.Exit(exitCode);
        }

        /// <summary>
        /// check char is alphanumberic character or not?
        /// </summary>
        /// <returns></returns>
        private ParseTree Chr()
        {
            char c = Peek();

            if (char.IsLetterOrDigit(c) || c == '\0')
            {
                return new ParseTree(ParseTree.NodeType.Chr, Pop(), null, null);
            }
            else
            {
                MessageError(string.Format("Parse error: expected alphanumberic, but got {0} at #(1}", Peek(), GetPosition()));
                return null;
            }
        }

        /// <summary>
        /// atom = char | '(' expression ')'
        /// </summary>
        /// <returns></returns>
        private ParseTree Atom()
        {
            ParseTree atomNode;

            if (Peek() == '(')
            {
                Pop();

                atomNode = Expression();

                if (Pop() != ')')
                {
                    MessageError("Parse error: Expected ')'");
                }
            }
            else
                atomNode = Chr();

            return atomNode;
        }

        /// <summary>
        /// rep = atom '*' | atom '+' | atom
        /// </summary>
        /// <returns></returns>
        private ParseTree Rep()
        {
            ParseTree atomNode = Atom();

            if (Peek() == '*')
            {
                Pop();
                return new ParseTree(ParseTree.NodeType.Star, null, atomNode, null);
            }
            else if (Peek() == '+')
            {
                Pop();
                return new ParseTree(ParseTree.NodeType.Plus, null, atomNode, null);
            }
            else
                return atomNode;
        }

        /// <summary>
        /// concat = rep . concat | rep
        /// </summary>
        /// <returns></returns>
        private ParseTree Concat()
        {
            ParseTree left = Rep();

            if (Peek() == '.')
            {
                Pop();

                ParseTree right = Concat();

                return new ParseTree(ParseTree.NodeType.Concat, null, left, right);
            }
            else
                return left;
        }

        /// <summary>
        /// expression = concat '|' expression | concat
        /// </summary>
        /// <returns></returns>
        private ParseTree Expression()
        {
            ParseTree left = Concat();

            if (Peek() == '|')
            {
                Pop();

                ParseTree right = Expression();

                return new ParseTree(ParseTree.NodeType.Alter, null, left, right);
            }
            else
                return left;
        }

        /// <summary>
        /// Creating a parse tree with the preprocessed regex
        /// </summary>
        /// <returns></returns>
        public ParseTree RegexToTree() => Expression();

        /// <summary>
        /// Show Parse Tree
        /// </summary>
        /// <param name="root"></param>
        /// <param name="offset"></param>
        public void PrintTree(ParseTree root, int offset)
        {
            if (root == null)
                return;

            for (int i = 0; i < offset; ++i)
                Console.Write(" ");

            switch (root.type)
            {
                case ParseTree.NodeType.Chr:
                    Console.WriteLine(root.type);
                    break;
                case ParseTree.NodeType.Star:
                    Console.WriteLine("*");
                    break;
                case ParseTree.NodeType.Plus:
                    Console.WriteLine("+");
                    break;
                case ParseTree.NodeType.Alter:
                    Console.WriteLine("|");
                    break;
                case ParseTree.NodeType.Concat:
                    Console.WriteLine(".");
                    break;
            }

            Console.Write("");

            PrintTree(root.left, offset + 8);
            PrintTree(root.right, offset + 8);
        }

        #endregion
    }
}
