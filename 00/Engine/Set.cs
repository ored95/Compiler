using System;
using SCG = System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Set<T> : C5.HashSet<T>
    {
        public Set(SCG.IEnumerable<T> enm) : base() => AddAll(enm);

        public Set(params T[] elems) : this((SCG.IEnumerable<T>)elems) { }

        #region Set operations: union (+), difference (-), and intersection (*)
        
        public static Set<T> operator +(Set<T> s1, Set<T> s2)
        {
            if (s1 == null || s2 == null)
                throw new ArgumentNullException("Union 2 sets");
            else
            {
                Set<T> res = new Set<T>(s1);
                res.AddAll(s2);
                return res;
            }
        }

        public static Set<T> operator -(Set<T> s1, Set<T> s2)
        {
            if (s1 == null || s2 == null)
                throw new ArgumentNullException("Different between 2 sets");
            else
            {
                Set<T> res = new Set<T>(s1);
                res.RemoveAll(s2);
                return res;
            }
        }

        public static Set<T> operator *(Set<T> s1, Set<T> s2)
        {
            if (s1 == null || s2 == null)
                throw new ArgumentNullException("Intersection of 2 sets");
            else
            {
                Set<T> res = new Set<T>(s1);
                res.RetainAll(s2);
                return res;
            }
        }

        #endregion

        #region Standard operstions of template

        // Equality of sets; take care to avoid infinite loops
        public static bool operator ==(Set<T> s1, Set<T> s2) => C5.EqualityComparer<Set<T>>.Default.Equals(s1, s2);

        public static bool operator !=(Set<T> s1, Set<T> s2) => !(s1 == s2);

        public override bool Equals(object that) => this == (that as Set<T>);

        public override int GetHashCode() => C5.EqualityComparer<Set<T>>.Default.GetHashCode(this);

        // Subset (<=) and superset (>=) relation:
        public static bool operator <=(Set<T> s1, Set<T> s2)
        {
            if (s1 == null || s2 == null)
                throw new ArgumentNullException("Set <= Set");
            else
                return s1.ContainsAll(s2);
        }

        public static bool operator >=(Set<T> s1, Set<T> s2)
        {
            if (s1 == null || s2 == null)
                throw new ArgumentNullException("Set >= Set");
            else
                return s2.ContainsAll(s1);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            bool first = true;

            foreach (T x in this)
            {
                if (!first)
                    sb.Append(",");
                sb.Append(x);
                first = false;
            }

            sb.Append("}");

            return sb.ToString();
        }

        #endregion
    }
}
