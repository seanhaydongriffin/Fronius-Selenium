using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Threading;

namespace Toolkit
{
    public static class Identifier
    {
        private static Random _random = new Random();
        private static object locker = new object();

        public static String get_random(int size)
        {
            byte[] buffer = new byte[size];
            _random.NextBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        public static String get_small()
        {
            byte[] buffer = new byte[4];
            _random.NextBytes(buffer);
            return Convert.ToBase64String(buffer).Replace("=", "").Replace("+", "a").Replace("/", "b");
        }

        public static String get_medium()
        {
            byte[] buffer = new byte[8];
            _random.NextBytes(buffer);
            return Convert.ToBase64String(buffer).Replace("=", "").Replace("+", "a").Replace("/", "b");
        }

        public static String get_large_numeric()
        {
            lock (locker)
            {
                Thread.Sleep(100);
                return System.DateTime.Now.ToString("yyyyMMddHHmmssf");
            }
        }





        public static String GetGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
