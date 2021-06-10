using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Utility
{
    public static class StringUtility
    {
        public static string SubString(this string str, int at, string with)
        {
            return str.Insert(at, with);
        }
        public static string Combine(this string str, int at, string toCombine)
        {
            return str.Insert(at, toCombine);
        }

        public static string Combine(this string str, string toCombine)
        {
            return str.Insert(str.Length, toCombine);
        }
    }
}
