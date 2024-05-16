using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Timers;
using System.Diagnostics;

namespace Toolkit
{
    public static class Dictionary
    {

        public static string get(this Dictionary<string, string> dict, string key)
        {
            if (dict.ContainsKey(key))
            
                return dict[key];

            return null;
        }

        //public static Timer get(this Dictionary<string, Timer> dict, string key)
        //{
        //    if (dict.ContainsKey(key))

        //        return dict[key];

        //    return null;
        //}

        public static Stopwatch get(this Dictionary<string, Stopwatch> dict, string key)
        {
            if (dict.ContainsKey(key))

                return dict[key];

            return null;
        }

        public static void put(this Dictionary<string, string> dict, string key, string value)
        {
            if (dict.ContainsKey(key))

                dict[key] = value;
            else

                dict.Add(key, value);
        }

        //public static void put(this Dictionary<string, Timer> dict, string key, Timer value)
        //{
        //    if (dict.ContainsKey(key))

        //        dict[key] = value;
        //    else

        //        dict.Add(key, value);
        //}

        public static void put(this Dictionary<string, Stopwatch> dict, string key, Stopwatch value)
        {
            if (dict.ContainsKey(key))

                dict[key] = value;
            else

                dict.Add(key, value);
        }

	

    }
}
