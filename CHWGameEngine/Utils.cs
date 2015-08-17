using System;
using System.Collections.Generic;
using System.Linq;

namespace CHWGameEngine
{
    static class Utils
    {
        /// <summary>
        /// Converts a string array to an integer array
        /// </summary>
        /// <param name="array">Array to convert</param>
        /// <returns>returns a new integer array</returns>
        public static int[] ParseStringArray(IEnumerable<string> array)
        {
            var enumerable = array as string[] ?? array.ToArray();
            int[] tokens = new int[enumerable.Count()];
            for (int i = 0; i < enumerable.Count(); i++)
            {
                tokens[i] = Convert.ToInt16(enumerable.ElementAt(i));
            }

            return tokens;
        }
    }
}
