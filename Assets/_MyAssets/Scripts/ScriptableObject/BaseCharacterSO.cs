using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public interface ICalcDmg
{
    public abstract Dictionary<string, string> CalcDmg(Data data);
}

public abstract class BaseCharacterSO : ScriptableObject, ICalcDmg
{
    public Status status;
    public Ascend ascend;
    public DamageMultiplier damageMultiplier;
    public ArtifactConfig artifactConfig;
    public List<SelectedWeapon> selectedWeapons;
    public List<SelectedMember> selectedMembers;
    public List<SelectedArtSetData> selectedArtSets;
    public List<SelectedArtMainSand> selectedArtMainSands;
    public List<SelectedArtMainGoblet> selectedArtMainGoblets;
    public List<SelectedArtMainCirclet> selectedArtMainCirclets;
    public List<SelectedArtifactData> selectedArtifactDatas;

    public string Name => name;
    public WeaponType WeaponType => status.weaponType;
    public abstract Dictionary<string, string> CalcDmg(Data data);
    protected Dictionary<AttackType, float[]> damageMultiplierDic = new();


    protected ExpectedDamage GetExpectedDamageFromStatus(Data data, ElementalReaction elementalReaction = null)
    {
        var multipliers = damageMultiplierDic[status.attackType];

        var ed = ExpectedDamage.Sum(data, status.attackType, multipliers, referenceStatus: status.referenceStatus, elementalReaction: elementalReaction);
        return ed;
    }


    [ContextMenu("Load CSV")]
    private void LoadCSV()
    {
        SelectedDataSetter.instance.SetDatas(this);
    }

    [ContextMenu("Calc")]
    public async void Calc()
    {
        damageMultiplierDic.Add(AttackType.Normal, damageMultiplier.normalAttacks);
        damageMultiplierDic.Add(AttackType.Charged, damageMultiplier.chargedAttacks);
        damageMultiplierDic.Add(AttackType.Plugged, damageMultiplier.pluggedAttacks);
        damageMultiplierDic.Add(AttackType.Skill, damageMultiplier.skills);
        damageMultiplierDic.Add(AttackType.Burst, damageMultiplier.bursts);


        List<Data> datas = await SelectedDataGetter.instance.GetDatas(this);
        Calculator.Calc(datas, this);
    }

}

[Serializable]
public class DamageMultiplier
{
    public float[] normalAttacks = { };
    public float[] chargedAttacks = { };
    public float[] pluggedAttacks = { };
    public float[] skills = { };
    public float[] bursts = { };

}

[Serializable]
public class ArtifactConfig
{
    [Range(1.0f, 1.6f)]
    public float fixedSubScore = 1.0f;
    public bool isUseRegisteredArtifacts = false;
    public bool isOnly4Set = true;
}


[Serializable]
public class Status
{
    public WeaponType weaponType = WeaponType.Catalyst;
    public ElementType elementType = ElementType.Electro;
    public ReferenceStatus referenceStatus = ReferenceStatus.Atk;
    public AttackType attackType = AttackType.Normal;
    // 基礎ステータス Lv90
    public float baseAtk;
    public float baseDef;
    public int baseHp;
    public bool hasSelfHarm = false;
    public bool notUseShimenawa = false;
    public bool isFront = true;
    public bool isNightSoul = false;
    public bool isRequiredShields = false;
    public bool canElementalApplication = true;
    public HealerType healerType = HealerType.None;

    public readonly float defaultCritRate = 0.05f;
    public readonly float defaultCritDmg = 0.5f;
}

public enum HealerType
{
    None,
    Selectable,
    Healer,
}


[Serializable]
public class Ascend
{
    public float critRate = 0.192f;
    public float critDmg = 0;
    public float dmgBonus = 0;
    public float atkPer = 0;
    public float energyRecharge = 0;
    public float hpPer = 0;
    public float defPer = 0;
    public float heal_bonus = 0;
    public float elemental_mastery = 0;

}

public enum WeaponType
{
    None = -1,
    Sword = 0,
    Claymore = 1,
    Bow = 2,
    Catalyst = 3,
    Polearm = 4,
}

public enum ElementType
{
    None = -1,
    Pyro = 0,
    Hydro = 1,
    Electro = 2,
    Cryo = 3,
    Geo = 4,
    Anemo = 5,
    Dendro = 6,
    Physics = 7,
}
