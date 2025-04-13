using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MemberData : BaseData, IComparable<MemberData>
{
    public bool isRequired = false;
    public string weapon = "";
    public string art_set = "";
    public string option = "";
    public string element_type_name = "";
    public float add_count;// しんかくとか閑雲の加算回数上限

    public ElementType ElementType
    {
        get
        {
            return element_type_name switch
            {
                "炎" => ElementType.Pyro,
                "水" => ElementType.Hydro,
                "氷" => ElementType.Cryo,
                "雷" => ElementType.Electro,
                "岩" => ElementType.Geo,
                "風" => ElementType.Anemo,
                "草" => ElementType.Dendro,
                "物理" => ElementType.Physics,
                _ => ElementType.None,
            };
        }
    }


    public string CombinedName
    {
        get
        {
            //  string combinedName = $"{name}({weapon},{art_set},{option})";
            List<string> infos = new();
            if (string.IsNullOrEmpty(weapon) == false) infos.Add(weapon);
            if (string.IsNullOrEmpty(art_set) == false) infos.Add(art_set);
            if (string.IsNullOrEmpty(option) == false) infos.Add(option);
            string combinedInfo = string.Join("、", infos);
            if (string.IsNullOrEmpty(combinedInfo)) return name;
            return $"{name}({combinedInfo})";
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
