using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using IekoLibrary;

[Serializable]
public class PartyData : BaseData, IComparable<PartyData>
{
    public MemberData[] members;

    public Dictionary<ElementType, int> ElementCounts { get; private set; } = new();

    public string GetMemberName(int index)
    {
        if (members.TryGetValue(index, out var member))
        {
            return member.name;
        }
        return "";
    }

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
        // CheckDuplicateOptions();
    }

    void SetElementalResonance(ElementType characterElementType)
    {
        //  Debug.Log("characterElementType: " + characterElementType);

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


    void CheckDuplicateOptions()
    {
        // 計算がややこしいので
        // csvの方で、灰燼持ってるのと持ってないの両方用意する
        // Data.IsSkip()で灰燼2人以上ならスキップにする
        CheckDuplicate_kaijin();
        CheckDuplicate_suiryoku();
    }
    void CheckDuplicate_kaijin()
    {
        var kaijinMembers = members.Where((member) => member.art_set.Contains("灰燼")).ToArray();

        if (kaijinMembers.Length == 0) return;

        var kaijinMembers_CanER = kaijinMembers.Where((member) => CanElementalReaction(member.ElementType)).ToArray();


        foreach (var kaijinMember in kaijinMembers)
        {
            if (CanElementalReaction(kaijinMember.ElementType) == false)
            {
                dmg_bonus -= 0.4f;
            }
        }

        dmg_bonus -= 0.4f * (kaijinMembers.Length - 1);
        // Debug.Log("灰燼 " + dmg_bonus);

    }


    void CheckDuplicate_suiryoku()
    {
        int suiryokuCount = members.Count((member) => member.art_set.Contains("翠緑"));
        if (suiryokuCount == 0) return;

        pyro_res += 0.4f * (suiryokuCount - 1);
        cryo_res += 0.4f * (suiryokuCount - 1);
        hydro_res += 0.4f * (suiryokuCount - 1);
        electro_res += 0.4f * (suiryokuCount - 1);
        //  Debug.Log("翠緑 " + res);

    }


    public bool CanElementalReaction(ElementType from)
    {
        foreach (var kvp in ElementCounts)
        {
            if (kvp.Value == 0) continue;
            var elementalReactionType = ElementalReaction.GetElementalReactionType(from, kvp.Key);
            // Debug.Log(from + " " + kvp.Key);
            // Debug.Log(elementalReactionType);
            if (elementalReactionType == ElementalReactionType.None) continue;
            return true;
        }
        return false;
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
