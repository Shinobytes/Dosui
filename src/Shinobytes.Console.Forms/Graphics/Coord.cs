using System.Runtime.InteropServices;

namespace Shinobytes.Console.Forms.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Coord
    {
        public short X;
        public short Y;

        public Coord(short x, short y)
        {
            X = x;
            Y = y;
        }
    };
}