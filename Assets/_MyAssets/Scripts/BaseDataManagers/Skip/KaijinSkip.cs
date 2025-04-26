using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IekoLibrary;


public static class KaijinSkip
{
    static WeaponData weaponData;
    static PartyData partyData;
    static Status status;
    static Ascend ascend;
    static ArtMainData artMainData;
    static ArtSetData artSetData;
    static ArtSubData artSubData;


    public static bool IsSkip(BaseDataSet baseDataSet, out string reason)
    {
        weaponData = baseDataSet.weaponData;
        artMainData = baseDataSet.artMainData;
        artSetData = baseDataSet.artSetData;
        artSubData = baseDataSet.artSubData;
        partyData = baseDataSet.partyData;
        status = baseDataSet.status;
        ascend = baseDataSet.ascend;



        return IsSkipDuplicationSetER("灰燼4", out reason);
    }



    static MemberData[] GetArtSetMembers(string setName)
    {
        var setMembers = partyData.members.Where((member) => member.art_set == setName).ToArray();
        return setMembers;
    }

    static bool IsSkipDuplicationSetER(string setName, out string reason)
    {
        var artSetMembers = GetArtSetMembers(setName);
        reason = "";
        if (artSetMembers.Length == 1)
        {
            if (!CanElementalReaction(artSetMembers[0]))
            {
                reason = $"{setName} 元素反応不可 {partyData.name}";
                return true;
            }
        }
        if (artSetMembers.Length > 1)
        {
            reason = $"{setName} 重複 {partyData.name}";

            return true;
        }
        return false;
    }

    static bool CanElementalReaction(MemberData from)
    {
        if (status.canElementalApplication)
        {
            return partyData.CanElementalReaction(from.ElementType);
        }

        // メインキャラが元素付着できない場合
        // メインキャラと同じ元素がパーティにいるとき

        int count = partyData.members.Count(m => m.ElementType == status.elementType);
        if (count == 0) return false;

        // fromがほかのメンバーと元素反応できるかどうか
        foreach (var to in partyData.members)
        {
            //if (to.ElementType != status.elementType) continue;
            var elementalReactionType = ElementalReaction.GetElementalReactionType(from.ElementType, to.ElementType);
            // Debug.Log(elementalReactionType);
            if (elementalReactionType != ElementalReactionType.None) return true;
        }


        return false;
    }
}
