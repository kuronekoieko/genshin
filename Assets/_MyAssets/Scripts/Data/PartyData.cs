using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PartyData : BaseData, IComparable<PartyData>
{
    public float add_count;
    public string option;

    public string CombinedName
    {
        get
        {
            if (string.IsNullOrEmpty(option)) return name;
            return $"{name}({option})";
        }
    }


    // https://learn.microsoft.com/ja-jp/dotnet/api/system.string.system-icomparable-compareto?view=netstandard-1.6
    public int CompareTo(PartyData other)
    {
        // https://learn.microsoft.com/ja-jp/dotnet/api/system.string.compare?view=net-8.0
        return string.Compare(name, other.name);
    }

    // https://www.mum-meblog.com/entry/tyr-utility/csharp-hashset
    public override bool Equals(object obj)
    {
        PartyData other = obj as PartyData;
        if (other == null) return false;
        return this.name == other.name;
    }

    public override int GetHashCode()
    {
        // hashsetは通るが、sortedsetは通らない
        return name.GetHashCode();
    }
}
