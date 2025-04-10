using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class PartyData : BaseData, IComparable<PartyData>
{
    public float add_count;
    public int pyro_count, hydro_count, electro_count, cryo_count, geo_count, anemo_count, dendro_count;
    public List<MemberData> members = new();

    public void SetElementalResonance(ElementType elementType)
    {

        switch (elementType)
        {
            case ElementType.Pyro:
                pyro_count++;
                break;
            case ElementType.Hydro:
                hydro_count++;
                break;
            case ElementType.Electro:
                electro_count++;
                break;
            case ElementType.Cryo:
                cryo_count++;
                break;
            case ElementType.Geo:
                geo_count++;
                break;
            case ElementType.Anemo:
                anemo_count++;
                break;
            case ElementType.Dendro:
                dendro_count++;
                break;
            default:
                break;
        }


        if (pyro_count >= 2)
        {
            atk_rate += 0.25f;
        }
        if (hydro_count >= 2)
        {
            hp_rate += 0.25f;
        }
        if (cryo_count >= 2)
        {
            crit_rate += 0.15f;
        }
        if (geo_count >= 2)
        {
            dmg_bonus += 0.15f;
            // TODO岩耐性ダウン
            if (elementType == ElementType.Geo)
            {
                res += -0.2f;
            }
        }
        if (dendro_count >= 2)
        {
            elemental_mastery += 100;
        }
    }


    public void CheckDuplicateOptions()
    {
        int kaijinCount = members.Count((member) => member.option.Contains("灰燼"));
        if (kaijinCount > 1)
        {
            dmg_bonus -= 0.4f * (kaijinCount - 1);
            Debug.Log("灰燼 " + dmg_bonus);
        }

        int suiryokuCount = members.Count((member) => member.option.Contains("翠緑"));
        if (suiryokuCount > 1)
        {
            res += 0.4f * (suiryokuCount - 1);
            Debug.Log("翠緑 " + res);

        }
    }



    public int ElementalTypeCount()
    {
        int count = 0;
        count += pyro_count > 0 ? 1 : 0;
        count += hydro_count > 0 ? 1 : 0;
        count += electro_count > 0 ? 1 : 0;
        count += cryo_count > 0 ? 1 : 0;
        count += geo_count > 0 ? 1 : 0;
        count += anemo_count > 0 ? 1 : 0;
        count += dendro_count > 0 ? 1 : 0;
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
