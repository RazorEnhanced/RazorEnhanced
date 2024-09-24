using System;

namespace Assistant
{
    public struct TypeID
    {
        private readonly ushort m_ID;

        internal TypeID(ushort id)
        {
            m_ID = id;
        }

        internal ushort Value { get { return m_ID; } }

        public static implicit operator ushort(TypeID a)
        {
            return a.m_ID;
        }
        public static implicit operator int(TypeID a)
        {
            return a.m_ID;
        }
        public static implicit operator uint(TypeID a)
        {
            return a.m_ID;
        }

        public static implicit operator TypeID(ushort a)
        {
            return new TypeID(a);
        }

        public override string ToString()
        {
            try
            {
                return string.Format("{0} ({1:X4})", RazorEnhanced.Statics.GetItemData(m_ID).Name, m_ID);
            }
            catch
            {
                return String.Format(" ({0:X4})", m_ID);
            }
        }

        internal Ultima.ItemData ItemData
        {
            get
            {
                try
                {
                    return RazorEnhanced.Statics.GetItemData(m_ID);
                }
                catch
                {
                    return new Ultima.ItemData("", Ultima.TileFlag.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                }
            }
        }

        public override int GetHashCode()
        {
            return m_ID;
        }

        public override bool Equals(object o)
        {
            if (o == null || !(o is TypeID)) return false;

            return ((TypeID)o).m_ID == m_ID;
        }


        public static bool operator ==(TypeID l, TypeID r)
        {
            return l.m_ID == r.m_ID;
        }

        public static bool operator !=(TypeID l, TypeID r)
        {
            return l.m_ID != r.m_ID;
        }

        public static bool operator >(TypeID l, TypeID r)
        {
            return l.m_ID > r.m_ID;
        }

        public static bool operator >=(TypeID l, TypeID r)
        {
            return l.m_ID >= r.m_ID;
        }

        public static bool operator <(TypeID l, TypeID r)
        {
            return l.m_ID < r.m_ID;
        }

        public static bool operator <=(TypeID l, TypeID r)
        {
            return l.m_ID <= r.m_ID;
        }
    }
}
