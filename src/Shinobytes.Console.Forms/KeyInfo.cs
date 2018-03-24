using System;

namespace Shinobytes.Console.Forms
{
    public class KeyInfo
    {
        private readonly ConsoleKeyInfo keyInfo;

        public KeyInfo(ConsoleKeyInfo keyInfo)
        {
            this.keyInfo = keyInfo;
        }

        public ConsoleKey Key => this.keyInfo.Key;

        public char KeyChar => this.keyInfo.KeyChar;
    }
}