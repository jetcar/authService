using System;
using System.Collections.Generic;

namespace CommonTools.Utils
{
    public class MyRandom
    {
        private static Random random = new Random();
        private static string _chars = "abcdefghijklmnopqrstuvwxyz0123456789";

        public static string NextString(int length)
        {
            var list = new List<char>();
            for (int i = 0; i < length; i++)
            {
                list.Add(_chars[random.Next(0, _chars.Length - 1)]);
            }
            return new string(list.ToArray());
        }
    }
}