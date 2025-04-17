using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IekoLibrary;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;


public class DataManager
{
    public static async Task<List<Data>> GetDatas(BaseCharacterSO character, WeaponData[] weaponDatas, PartyData[] partyDatas, List<ArtifactGroup> artifactGroups)
    {
        return await GetDatas(character.status, character.ascend, weaponDatas, partyDatas, artifactGroups);
    }

    static async UniTask<List<Data>> GetDatas(Status status, Ascend ascend, WeaponData[] weaponDatas, PartyData[] partyDatas, List<ArtifactGroup> artifactGroups)
    {
        Debug.Log("組み合わせ作成開始");
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        int progress = 0;
        int max = weaponDatas.Length * partyDatas.Length * artifactGroups.Count;


        List<Data> datas = new();

        foreach (var weapon in weaponDatas)
        {
            foreach (var partyData in partyDatas)
            {
                foreach (var artifactGroup in artifactGroups)
                {

                    if (IsSkip(weapon, artifactGroup, partyData, status, ascend, out string reason))
                    {
                        // Debug.Log("skip: " + reason);
                    }
                    else
                    {
                        Data data = new(weapon, artifactGroup, partyData, status, ascend);
                        datas.Add(data);
                    }


                    if (sw.ElapsedMilliseconds % 1000 == 0)
                    {
                        await UniTask.DelayFrame(1);

                        progress++;
                        //await UniTask.DelayFrame(1);
                        int per = (int)((float)progress / (float)max * 100f);

                        Debug.Log("progress: " + progress + "/" + max + " " + per + "%");
                    }
                }
            }
        }

        sw.Stop();
        Debug.Log("処理時間 " + sw.ElapsedMilliseconds / 1000f + "s");


        return datas;
    }



    static bool isSkip = false;
    static string skipReason = "";

    static void SetSkip(string reason)
    {
        //   Debug.Log("しめ縄4");
        skipReason += reason + "、";
        isSkip = isSkip || true;
    }


    static bool IsSkip(
        WeaponData weapon,
        ArtifactGroup artifactGroup,
        PartyData partyData,
        Status status,
        Ascend ascend,
        out string reason)
    {

        ArtMainData artMainData = artifactGroup.artMainData;
        ArtSetData artSetData = artifactGroup.artSetData;
        ArtSubData artSubData = artifactGroup.artSubData;

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

        if (IsCitlali(partyData))
        {
            SetSkip("シトラリ");
        }

        if (IsSkipXilonen(partyData, status))
        {
            SetSkip("シロネン");
        }

        if (IsSkipGorou(partyData, out string gorouReason))
        {
            SetSkip($"ゴロー{gorouReason}");
        }

        if (IsNotUseArtSet(partyData, "深林4")) SetSkip("深林4");

        if (IsNotUseArtSet(partyData, "翠緑4")) SetSkip("翠緑4");

        if (IsNotUseArtSet(partyData, "灰燼4")) SetSkip("灰燼4");

        // TODO:残響

        /*
                if (skip > 0)
                {
                    //   Debug.Log("weapon " + weapon.skip);
                    //   Debug.Log("artMainData " + artMainData.skip);
                    //   Debug.Log("artSetData " + artSetData.skip);
                    //   Debug.Log("artSubData " + artSubData.skip);
                    //   Debug.Log("partyData " + partyData.skip);
                    SetSkip("csv");
                }*/

        reason = skipReason;

        return isSkip;
    }

    static bool IsCitlali(PartyData partyData)
    {
        bool esxist = partyData.members.Count((member) => member.name.Contains("シトラリ")) > 0;
        if (esxist)
        {
            int eCount = partyData.ElementCounts[ElementType.Pyro] + partyData.ElementCounts[ElementType.Hydro];
            if (eCount == 0) return true;
        }
        return false;
    }

    static bool IsSkipXilonen(PartyData partyData, Status status)
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

    static bool IsSkipGorou(PartyData partyData, out string reason)
    {
        reason = "";
        MemberData gorou = partyData.members.FirstOrDefault((member) => member.name.Contains("ゴロー"));
        if (gorou == null) return false;
        int geoCount = partyData.ElementCounts[ElementType.Geo];

        // 岩0,1,2人の場合
        if (geoCount < 3)
        {
            reason = "岩2";
            return gorou.option != "岩2";
        }

        reason = "岩3";

        // 岩3,4人人の場合
        return gorou.option != "岩3";
    }

    static bool IsNotUseArtSet(PartyData partyData, string setName)
    {
        // Debug.Log("============");

        var setMembers = partyData.members.Where((member) => member.art_set == setName).ToArray();
        // Debug.Log(setMembers.Length);

        if (setMembers.Length == 0) return false;
        if (setMembers.Length > 1) return true;
        // Debug.Log("============");
        // Debug.Log(setName);
        // Debug.Log(partyData.CanElementalReaction(setMembers[0].ElementType));

        return partyData.CanElementalReaction(setMembers[0].ElementType) == false;
    }
}
