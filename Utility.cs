﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zxcvbn
{
    /// <summary>
    /// A few useful extension methods
    /// </summary>
    static class Utility
    {
        /// <summary>
        /// Convert a number of seconds into a human readable form. Rounds up.
        /// To be consistent with zxcvbn, it returns the unit + 1 (i.e. 60 * 10 seconds = 10 minutes would come out as "11 minutes"
        /// this is probably to avoid ever needing to deal with plurals
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string DisplayTime(double seconds)
        {
            long minute = 60, hour = minute * 60, day = hour * 24, month = day * 31, year = month * 12, century = year * 100;

            if (seconds < minute) return "instant";
            else if (seconds < hour) return "{0} minutes".F(1 + Math.Ceiling(seconds / minute));
            else if (seconds < day) return "{0} hours".F(1 + Math.Ceiling(seconds / hour));
            else if (seconds < month) return "{0} days".F(1 + Math.Ceiling(seconds / day));
            else if (seconds < year) return "{0} months".F(1 + Math.Ceiling(seconds / month));
            else if (seconds < century) return "{0} years".F(1 + Math.Ceiling(seconds / year));
            else return "centuries";
        }

        public static string F(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        public static string StringReverse(this string str)
        {
            return new string(str.Reverse().ToArray());
        }

        /// <summary>
        /// A convenience for parsing a substring as an int and returning the results. Uses TryParse, and so returns zero where there is no valid int
        /// </summary>
        /// <param name="r">Substring parsed as int or zero</param>
        /// <returns>True if the parse succeeds</returns>
        public static bool IntParseSubstring(this string str, int startIndex, int length, out int r)
        {
            return Int32.TryParse(str.Substring(startIndex, length), out r);
        }

        public static int ToInt(this string str)
        {
            int r = 0;
            Int32.TryParse(str, out r);
            return r;
        }
    }
}
