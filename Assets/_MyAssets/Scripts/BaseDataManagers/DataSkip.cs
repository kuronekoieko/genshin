using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IekoLibrary;


public static class DataSkip
{
    static WeaponData weaponData;
    static PartyData partyData;
    static Status status;
    static Ascend ascend;
    static ArtMainData artMainData;
    static ArtSetData artSetData;
    static ArtSubData artSubData;

    static bool isSkip = false;
    static string skipReason = "";

    static void SetSkip(string reason)
    {
        //   Debug.Log("しめ縄4");
        skipReason += reason + "、";
        isSkip = isSkip || true;
    }


    public static bool IsSkip(BaseDataSet baseDataSet)
    {
        weaponData = baseDataSet.weaponData;
        artMainData = baseDataSet.artMainData;
        artSetData = baseDataSet.artSetData;
        artSubData = baseDataSet.artSubData;
        partyData = baseDataSet.partyData;
        status = baseDataSet.status;
        ascend = baseDataSet.ascend;


        isSkip = false;
        skipReason = "";

        if (artSetData.name == "しめ縄4" && status.notUseShimenawa)
        {
            SetSkip("しめ縄4");
        }

        bool isGakudan = status.weaponType == WeaponType.Catalyst || status.weaponType == WeaponType.Bow;

        if (artSetData.name == "楽団4" && isGakudan == false)
        {
            SetSkip("楽団4");
        }
        if (artSetData.name == "剣闘士4" && isGakudan == true)
        {
            SetSkip("剣闘士4");
        }

        if (artSetData.name == "ファントム4" || artSetData.name == "花海4" || artSetData.name == "辰砂4")
        {
            bool hasSelfHarm = status.hasSelfHarm || partyData.has_self_harm;
            if (!hasSelfHarm)
            {
                SetSkip("ファントム4");
            }
        }

        if (artSetData.name == "劇団4(控え)" && status.isFront)
        {
            SetSkip("劇団4");
        }
        if (artSetData.name == "劇団4(表)" && !status.isFront)
        {
            SetSkip("劇団4");
        }

        bool isFrozen = partyData.ElementCounts[ElementType.Cryo] > 0 && partyData.ElementCounts[ElementType.Hydro] > 0;

        if (artSetData.name == "氷風4(凍結)" && isFrozen == false)
        {
            SetSkip("氷風4");
        }
        if (artSetData.name == "氷風4(凍結無し)" && partyData.ElementCounts[ElementType.Cryo] == 0)
        {
            SetSkip("氷風4");
        }
        if (artSetData.name == "雷4" && partyData.ElementCounts[ElementType.Electro] == 0)
        {
            SetSkip("雷4");
        }
        if (artSetData.is_night_soul)
        {
            if (!status.isNightSoul)
            {
                SetSkip("夜魂");
            }
        }

        if (IsCitlali())
        {
            SetSkip("シトラリ");
        }

        if (IsSkipXilonen())
        {
            SetSkip("シロネン");
        }

        if (IsSkipFrina())
        {
            SetSkip("フリーナ");
        }

        if (IsSkipGorou(out string reason))
        {
            SetSkip(reason);
        }

        if (IsDuplication("深林4"))
        {
            SetSkip("深林4");
        }


        if (IsSkipDuplicationSetER("翠緑4", out reason))
        {
            SetSkip(reason);
        }


        if (IsSkipDuplicationSetER("灰燼4", out reason))
        {
            SetSkip(reason);
        }

        if (status.isRequiredShields)
        {
            int count = partyData.members.Count(memberData => memberData.has_shields);
            if (count == 0)
            {
                SetSkip("シールド無し");
            }
        }

        // TODO:残響

        // Debug.Log("skip: " + skipReason);

        return isSkip;
    }



    static bool IsSkipFrina()
    {
        bool esxistFrina = partyData.members.Any((member) => member.name.Contains("フリーナ"));
        if (esxistFrina)
        {
            bool esxistHealer = partyData.members
                .Where((member) => member.name.Contains("フリーナ") == false)
                .Any((member) => member.HealerType != HealerType.None);

            if (esxistHealer == false) return true;
        }
        return false;
    }

    static bool IsCitlali()
    {
        bool esxist = partyData.members.Count((member) => member.name.Contains("シトラリ")) > 0;
        if (esxist)
        {
            int eCount = partyData.ElementCounts[ElementType.Pyro] + partyData.ElementCounts[ElementType.Hydro];
            if (eCount == 0) return true;
        }
        return false;
    }

    static bool IsSkipXilonen()
    {
        MemberData xilonen = partyData.members.FirstOrDefault((member) => member.name.Contains("シロネン"));
        if (xilonen == null) return false;

        int eCount = partyData.ElementCounts[ElementType.Pyro] + partyData.ElementCounts[ElementType.Cryo] + partyData.ElementCounts[ElementType.Electro] + partyData.ElementCounts[ElementType.Hydro];
        bool isNotActiveSampleMusic = eCount < 2;

        // 2凸未満の場合
        if (xilonen.constellation < 2)
        {
            return isNotActiveSampleMusic;
        }

        // 2凸以上の場合
        if (status.elementType == ElementType.Geo)
        {
            // メインキャラが岩ならスキップしない
            return false;
        }

        return isNotActiveSampleMusic;
    }

    static bool IsSkipGorou(out string reason)
    {
        reason = "";
        MemberData gorou = partyData.members.FirstOrDefault((member) => member.name.Contains("ゴロー"));
        if (gorou == null) return false;
        int geoCount = partyData.ElementCounts[ElementType.Geo];

        // 岩0,1,2人の場合
        if (geoCount < 3)
        {
            reason = "ゴロー岩2";
            return gorou.option != "岩2";
        }

        reason = "ゴロー岩3";

        // 岩3,4人人の場合
        return gorou.option != "岩3";
    }

    static MemberData[] GetArtSetMembers(string setName)
    {
        var setMembers = partyData.members.Where((member) => member.art_set == setName).ToArray();
        return setMembers;
    }

    static bool IsDuplication(string setName)
    {
        // Debug.Log("============");

        var setMembers = partyData.members.Where((member) => member.art_set == setName).ToArray();
        // Debug.Log(setMembers.Length);
        if (setMembers.Length == 0) return false;

        if (setMembers.Length == 1)
        {
            return false;
        }

        return true;
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
        else
        {
            // メインキャラが元素付着できない場合に、メインキャラと同じ元素がパーティにいて、かつ元素反応できるかどうか
            foreach (var to in partyData.members)
            {
                if (to.ElementType != status.elementType) continue;
                var elementalReactionType = ElementalReaction.GetElementalReactionType(from.ElementType, to.ElementType);
                // Debug.Log(elementalReactionType);
                if (elementalReactionType != ElementalReactionType.None) return true;
            }
        }

        return false;
    }
}
