using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class PartyData : BaseData, IComparable<PartyData>
{
    public MemberData[] members;

    public Dictionary<ElementType, int> ElementCounts { get; private set; } = new();

    public string Log
    {
        get
        {
            string log = "";
            foreach (var kvp in ElementCounts)
            {
                if (kvp.Value > 0) log += kvp.Key + ":" + kvp.Value + "、";
            }

            return log;
        }
    }


    public PartyData(MemberData[] members, ElementType characterElementType)
    {
        this.members = members;

        if (members == null)
        {
            name = "なし";
            return;
        }

        MemberData sumMemberData = Utils.AddInstances(members);
        Utils.CopyBaseFields<BaseData>(sumMemberData, this);
        string[] combinedNames = members.Select(memberData => memberData.CombinedName).ToArray();
        name = string.Join("+", combinedNames);
        SetElementalResonance(characterElementType);
        CheckDuplicateOptions();
    }

    public void SetElementalResonance(ElementType characterElementType)
    {
        // 初期化
        foreach (ElementType et in Enum.GetValues(typeof(ElementType)))
        {
            ElementCounts.Add(et, 0);
        }

        ElementCounts[characterElementType] += 1;


        foreach (var memberData in members)
        {
            ElementCounts[memberData.ElementType] += 1;
        }


        if (ElementCounts[ElementType.Pyro] >= 2)
        {
            atk_rate += 0.25f;
        }
        if (ElementCounts[ElementType.Hydro] >= 2)
        {
            hp_rate += 0.25f;
        }
        if (ElementCounts[ElementType.Cryo] >= 2)
        {
            crit_rate += 0.15f;
        }
        if (ElementCounts[ElementType.Geo] >= 2)
        {
            dmg_bonus += 0.15f;
            geo_res += -0.2f;
        }
        if (ElementCounts[ElementType.Dendro] >= 2)
        {
            elemental_mastery += 100;
        }
    }


    public void CheckDuplicateOptions()
    {
        int kaijinCount = members.Count((member) => member.art_set.Contains("灰燼"));
        if (kaijinCount > 1)
        {
            dmg_bonus -= 0.4f * (kaijinCount - 1);
            // Debug.Log("灰燼 " + dmg_bonus);
        }

        int suiryokuCount = members.Count((member) => member.art_set.Contains("翠緑"));
        if (suiryokuCount > 1)
        {
            res += 0.4f * (suiryokuCount - 1);
            //  Debug.Log("翠緑 " + res);
        }
    }

    public int ElementalTypeCount()
    {
        int count = 0;
        foreach (var kvp in ElementCounts)
        {
            if (kvp.Value > 0) count++;
        }
        return count;
    }


    // https://learn.microsoft.com/ja-jp/dotnet/api/system.string.system-icomparable-compareto?view=netstandard-1.6
    // SortedSetに使う
    public int CompareTo(PartyData other)
    {
        // https://learn.microsoft.com/ja-jp/dotnet/api/system.string.compare?view=net-8.0
        return string.Compare(name, other.name);
    }

    // https://www.mum-meblog.com/entry/tyr-utility/csharp-hashset
    // HashSetで使う
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
