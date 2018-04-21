using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainDump.util {
    static class RandomExtensions {
        public static long NextLong(this Random rnd) {
            byte[] buffer = new byte[8];
            rnd.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static long NextPositiveLong(this Random rnd) {
            long val = NextLong(rnd);
            // long.MinValue has a magnitude 1 greater than long.MaxValue
            // 0.0000000000000000054% chance of getting an OverflowException without this check
            return val == long.MinValue ? long.MaxValue : Math.Abs(val);
        }
    }
}
