using System;

namespace AST
{
    public class Utils
    {

        // class StoreEntry
        // ================
        // the inner storage of entries
        // 
        public class StoreEntry
        {
            public StoreEntry(String name, ExprType type, Int32 offset)
            {
                this.name = name;
                this.type = type;
                this.offset = offset;
            }
            public readonly String name;
            public readonly ExprType type;
            public readonly Int32 offset;
        }

        public static Int32 RoundUp(Int32 value, Int32 alignment)
        {
            return (value + alignment - 1) & ~(alignment - 1);
        }
    }
}