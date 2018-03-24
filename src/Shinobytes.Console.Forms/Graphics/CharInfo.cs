using System.Runtime.InteropServices;

namespace Shinobytes.Console.Forms.Graphics
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
    public struct CharInfo
    {
        [FieldOffset(0)]
        public char UnicodeChar;
        [FieldOffset(0)]
        public byte bAsciiChar;
        [FieldOffset(2)] public short Attributes;

        public override bool Equals(object obj)
        {
            if (!(obj is CharInfo))
            {
                return false;
            }

            var info = (CharInfo)obj;
            return UnicodeChar == info.UnicodeChar &&
                   bAsciiChar == info.bAsciiChar &&
                   Attributes == info.Attributes;
        }

        public override int GetHashCode()
        {
            var hashCode = 1676292764;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + UnicodeChar.GetHashCode();
            hashCode = hashCode * -1521134295 + bAsciiChar.GetHashCode();
            hashCode = hashCode * -1521134295 + Attributes.GetHashCode();
            return hashCode;
        }
    }
}