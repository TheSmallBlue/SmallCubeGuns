using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericHelpers
{
    public static V DefaultGet<K, V>(this Dictionary<K, V> dict, K key, Func<V> defaultFactory)
    {
        V v;
        if (!dict.TryGetValue(key, out v))
            dict[key] = v = defaultFactory();
        return v;
    }
}
