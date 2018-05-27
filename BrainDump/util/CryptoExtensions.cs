using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainDump.util {
    static class CryptoExtensions {
        public static bool slowEquals(this byte[] arr1, byte[] arr2) {
            // Source: http://bryanavery.co.uk/cryptography-net-avoiding-timing-attack/
            uint diff = (uint)arr1.Length ^ (uint)arr2.Length;

            for (int i = 0; i < arr1.Length && i < arr2.Length; i++) {
                diff |= (uint) (arr1[i] ^ arr2[i]);
            }

            return diff == 0;
        }
    }
}
