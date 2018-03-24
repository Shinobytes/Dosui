namespace Shinobytes.Console.Forms
{
    public static class AsciiCodes
    {
        private const char a179 = '\u05B3';
        public static char GetAsciiChar(int number)
        {
            return (char)(a179 + (char)(number - 179));
        }

        public static char BorderSingle_TopLeft => GetAsciiChar(218);
        public static char BorderSingle_TopRight => GetAsciiChar(191);
        public static char BorderSingle_Horizontal => GetAsciiChar(196);
        public static char BorderSingle_Vertical => GetAsciiChar(179);
        public static char BorderSingle_BottomLeft => GetAsciiChar(192);
        public static char BorderSingle_BottomRight => GetAsciiChar(217);

        public static char BorderSingle_SplitToRight => GetAsciiChar(195);
        public static char BorderSingle_SplitToLeft => GetAsciiChar(180);

        // todo: update the actual char codes to match le-double.
        public static char BorderDouble_TopLeft => GetAsciiChar(201);
        public static char BorderDouble_TopRight => GetAsciiChar(187);
        public static char BorderDouble_Horizontal => GetAsciiChar(205);
        public static char BorderDouble_Vertical => GetAsciiChar(186);
        public static char BorderDouble_BottomLeft => GetAsciiChar(200);
        public static char BorderDouble_BottomRight => GetAsciiChar(188);

        public static char BorderDouble_SplitToRight => GetAsciiChar(204);
        public static char BorderDouble_SplitToLeft => GetAsciiChar(185);
    }
}
