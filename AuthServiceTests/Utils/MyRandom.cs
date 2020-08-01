using System;
using System.Collections.Generic;

namespace AuthServiceTests.Controllers
{
    public class MyRandom
    {
        private static Random random = new Random();
        private static char[] _chars = new char[]{'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','r','s','t','u','v','w','q','x','y','z'};
        public static string NextString(int length)
        {
            var list = new List<char>();
            for (int i = 0; i < length; i++)
            {
                list.Add(_chars[random.Next(0,_chars.Length-1)]);
            }
            return  new string(list.ToArray());
        }
    }
}