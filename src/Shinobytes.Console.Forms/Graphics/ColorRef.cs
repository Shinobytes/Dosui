using System.Drawing;
using System.Runtime.InteropServices;

namespace Shinobytes.Console.Forms.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorRef
    {
        private uint ColorDWORD;

        internal ColorRef(Color color)
        {
            ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }

        internal ColorRef(uint r, uint g, uint b)
        {
            ColorDWORD = r + (g << 8) + (b << 16);
        }

        public override string ToString()
        {
            return ColorDWORD.ToString();
        }
    }
}