using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VladB.Utility {
    public static partial class Extensions {
        static void DebugLog(string log) {
#if UNITY_5_3_OR_NEWER
            UnityEngine.Debug.Log(log);
#else
            Console.WriteLine(log);
#endif
        }

        public static void DebugAll<T>(this IList<T> list, bool isOneDebug = false, string separator = " ") {
            if(isOneDebug) {
                DebugLog(string.Join(separator , list));
            } else {
                for(int i = 0; i < list.Count; i++) {
                    DebugLog(list[i].ToString());
                }
            }
        }

    }
}
