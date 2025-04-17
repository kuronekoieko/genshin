using System;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{

    public static bool ContainsBy<T>(this List<T> self, Func<T, bool> contains, out int index) where T : ISelected
    {
        index = -1;

        for (int i = 0; i < self.Count; i++)
        {
            if (contains(self[i]))
            {
                index = i;
                return true;
            }
        }
        return false;
    }


    public static string ToString(this float self, bool isInt = false)
    {
        return Mathf.FloorToInt(self).ToString();
    }

}