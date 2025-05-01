using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class PartyData : BaseData, IComparable<PartyData>
{
    public MemberData[] members;

    public Dictionary<ElementType, int> ElementCounts { get; private set; } = new();

    public string Log { get; private set; }
    public int MembersEnergyCostSum { get; private set; }

    public PartyData(MemberData[] members, ElementType characterElementType)
    {
        this.members = members;

        if (members == null)
        {
            name = "なし";
            return;
        }

        MembersEnergyCostSum = members.Sum(x => x.energy_cost);
        SetFields();
        ElementCounts = GetElementCounts(characterElementType);
        SetElementalResonance(ElementCounts);
        CheckXilonenConstellation(characterElementType);
        Log = GetLog(ElementCounts);
    }

    void SetFields()
    {
        MemberData sumMemberData = FastInstanceAdder.AddInstances(members);
        FastFieldCopier.CopyBaseFields<BaseData>(sumMemberData, this);
        string[] combinedNames = members.Select(memberData => memberData.CombinedName).ToArray();
        name = string.Join("+", combinedNames);
    }

    string GetLog(Dictionary<ElementType, int> elementCounts)
    {
        string log = "";
        foreach (var kvp in elementCounts)
        {
            if (kvp.Value > 0) log += kvp.Key + ":" + kvp.Value + "、";
        }

        return log;
    }

    Dictionary<ElementType, int> GetElementCounts(ElementType characterElementType)
    {
        Dictionary<ElementType, int> elementCounts = new();
        // 初期化
        foreach (ElementType et in Enum.GetValues(typeof(ElementType)))
        {
            elementCounts.Add(et, 0);
        }

        elementCounts[characterElementType] += 1;

        foreach (var memberData in members)
        {
            elementCounts[memberData.ElementType] += 1;
        }
        return elementCounts;
    }

    void SetElementalResonance(Dictionary<ElementType, int> elementCounts)
    {
        //  Debug.Log("characterElementType: " + characterElementType);

        if (elementCounts[ElementType.Pyro] >= 2)
        {
            atk_rate += 0.25f;
        }
        if (elementCounts[ElementType.Hydro] >= 2)
        {
            hp_rate += 0.25f;
        }
        if (elementCounts[ElementType.Cryo] >= 2)
        {
            crit_rate += 0.15f;
        }
        if (elementCounts[ElementType.Geo] >= 2)
        {
            dmg_bonus += 0.15f;
            geo_res += -0.2f;
        }
        if (elementCounts[ElementType.Dendro] >= 2)
        {
            elemental_mastery += 100;
        }

    }

    void CheckXilonenConstellation(ElementType characterElementType)
    {
        var xilonen = members.FirstOrDefault((member) => member.name.Contains("シロネン"));
        if (xilonen == null) return;
        if (xilonen.constellation < 2) return;

        switch (characterElementType)
        {
            case ElementType.Pyro:
                atk_rate += 0.45f;
                break;
            case ElementType.Cryo:
                crit_dmg += 0.6f;
                break;
            case ElementType.Hydro:
                hp_rate += 0.45f;
                break;
            default:
                break;
        }
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
