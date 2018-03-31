namespace Shinobytes.Console.Forms.Graphics
{
    public struct BorderThickness
    {
        public readonly int Top;
        public readonly int Left;
        public readonly int Right;
        public readonly int Bottom;
        public BorderThickness(int top, int left, int right, int bottom)
        {
            this.Top = top;
            this.Left = left;
            this.Right = right;
            this.Bottom = bottom;
        }

        public int Size => Top + Left + Right + Bottom;

        public static BorderThickness Zero => new BorderThickness();
        public static BorderThickness One => new BorderThickness(1, 1, 1, 1);
    }
}