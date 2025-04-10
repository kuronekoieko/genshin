using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MemberData : BaseData, IComparable<MemberData>
{
    public string option = "";

    public string CombinedName
    {
        get
        {
            if (string.IsNullOrEmpty(option)) return name;
            return $"{name}({option})";
        }
    }

    // https://learn.microsoft.com/ja-jp/dotnet/api/system.string.system-icomparable-compareto?view=netstandard-1.6
    // SortedSetに使う
    public int CompareTo(MemberData other)
    {
        // https://learn.microsoft.com/ja-jp/dotnet/api/system.string.compare?view=net-8.0
        return string.Compare(name, other.name);
    }

    // https://www.mum-meblog.com/entry/tyr-utility/csharp-hashset
    // HashSetで使う
    public override bool Equals(object obj)
    {
        MemberData other = obj as MemberData;
        if (other == null) return false;
        return this.name == other.name;
    }

    public override int GetHashCode()
    {
        // hashsetは通るが、sortedsetは通らない
        return name.GetHashCode();
    }
}
