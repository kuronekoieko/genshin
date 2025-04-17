using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows;
using System;

[Serializable]
public class Data : BaseData
{
    public WeaponData weapon;
    public ArtMainData artMainData;
    public ArtSetData artSetData;
    public ArtSubData artSubData;
    public PartyData partyData;
    public Status status;
    public Ascend ascend;
    public float BaseAtk => status.baseAtk + weapon.base_atk;

    public Data(
        WeaponData weapon,
        ArtifactGroup artifactGroup,
        PartyData partyData,
        Status status,
        Ascend ascend)
    {
        this.weapon = weapon;
        this.artMainData = artifactGroup.artMainData;
        this.artSetData = artifactGroup.artSetData;
        this.artSubData = artifactGroup.artSubData;
        this.partyData = partyData;
        this.status = status;
        this.ascend = ascend;

        var baseDatas = new BaseData[] { weapon, artMainData, artSetData, partyData, artSubData, };
        var baseData = Utils.AddInstances(baseDatas);
        Utils.CopyBaseFields(baseData, this);
        SetFields();
        SetCharaData();
    }


    void SetFields()
    {
        heal_bonus += ascend.heal_bonus;
        hp_rate += ascend.hpPer;
        energy_recharge += 1 + ascend.energyRecharge;
        elemental_mastery += ascend.elemental_mastery;
        def_rate += ascend.defPer;
        atk_rate += ascend.atkPer;
        dmg_bonus += ascend.dmgBonus;
        crit_rate += status.defaultCritRate + ascend.critRate;
        crit_dmg += status.defaultCritDmg + ascend.critDmg;
    }

    void SetCharaData()
    {
        hp = status.baseHp * (1 + hp_rate) + hp;
        def = status.baseDef * (1 + def_rate) + def;

        var dmgAdd_sekikaku = def * weapon.sekikaku;
        add_normal_atk += dmgAdd_sekikaku;
        add_charged_atk += dmgAdd_sekikaku;

        var dmgAdd_cinnabar = def * weapon.cinnabar;
        add_skill += dmgAdd_cinnabar;

        var homa_atkAdd = hp * weapon.homa;
        var sekisa_atkAdd = elemental_mastery * weapon.sekisha;
        var kusanagi_atkAdd = (energy_recharge - 1) * weapon.kusanagi;

        atk
            = BaseAtk * (1 + atk_rate)
            + atk
            + homa_atkAdd
            + sekisa_atkAdd
            + kusanagi_atkAdd;
    }

    bool isSkip = false;
    string skipReason = "";

    void SetSkip(string reason)
    {
        //   Debug.Log("しめ縄4");
        skipReason += reason + "、";
        isSkip = isSkip || true;
    }


    public bool IsSkip(out string reason)
    {
        isSkip = false;

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

        if (IsSkipGorou(out string gorouReason))
        {
            SetSkip($"ゴロー{gorouReason}");
        }

        if (IsNotUseArtSet("深林4")) SetSkip("深林4");

        if (IsNotUseArtSet("翠緑4")) SetSkip("翠緑4");

        if (IsNotUseArtSet("灰燼4")) SetSkip("灰燼4");

        // TODO:残響

        if (skip > 0)
        {
            //   Debug.Log("weapon " + weapon.skip);
            //   Debug.Log("artMainData " + artMainData.skip);
            //   Debug.Log("artSetData " + artSetData.skip);
            //   Debug.Log("artSubData " + artSubData.skip);
            //   Debug.Log("partyData " + partyData.skip);
            SetSkip("csv");
        }

        reason = skipReason;

        return isSkip;
    }

    bool IsCitlali()
    {
        bool esxist = partyData.members.Count((member) => member.name.Contains("シトラリ")) > 0;
        if (esxist)
        {
            int eCount = partyData.ElementCounts[ElementType.Pyro] + partyData.ElementCounts[ElementType.Hydro];
            if (eCount == 0) return true;
        }
        return false;
    }

    bool IsSkipXilonen()
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

    bool IsSkipGorou(out string reason)
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

    bool IsNotUseArtSet(string setName)
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
