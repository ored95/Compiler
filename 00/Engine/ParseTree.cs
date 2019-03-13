using input = System.Char;

namespace Engine
{
    class ParseTree
    {
        public enum NodeType
        {
            Chr,
            Star,
            Plus,
            Alter,
            Concat
        }

        public NodeType type;
        public input? data;
        public ParseTree left;
        public ParseTree right;

        public ParseTree(NodeType type, input? data, ParseTree left, ParseTree right)
        {
            this.type = type;
            this.data = data;
            this.left = left;
            this.right = right;
        }
    }
}
