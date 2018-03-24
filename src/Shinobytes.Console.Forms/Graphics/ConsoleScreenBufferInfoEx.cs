using System.Runtime.InteropServices;

namespace Shinobytes.Console.Forms.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ConsoleScreenBufferInfoEx
    {
        internal int cbSize;
        internal Coord dwSize;
        internal Coord dwCursorPosition;
        internal ushort wAttributes;
        internal SmallRect srWindow;
        internal Coord dwMaximumWindowSize;
        internal ushort wPopupAttributes;
        internal bool bFullscreenSupported;
        internal ColorRef black;
        internal ColorRef darkBlue;
        internal ColorRef darkGreen;
        internal ColorRef darkCyan;
        internal ColorRef darkRed;
        internal ColorRef darkMagenta;
        internal ColorRef darkYellow;
        internal ColorRef gray;
        internal ColorRef darkGray;
        internal ColorRef blue;
        internal ColorRef green;
        internal ColorRef cyan;
        internal ColorRef red;
        internal ColorRef magenta;
        internal ColorRef yellow;
        internal ColorRef white;
    }
}