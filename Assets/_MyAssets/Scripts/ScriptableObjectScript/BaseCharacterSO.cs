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
    public bool isSub = false;
    public List<SelectedWeapon> selectedWeapons;
    public List<SelectedMember> selectedMembers;
    public List<SelectedArtSetData> selectedArtSets;


    public string Name => name;
    public WeaponType WeaponType => status.weaponType;
    public abstract Dictionary<string, string> CalcDmg(Data data);

    private void OnEnable()
    {
        // Debug.Log("OnEnable: " + Name);
        // CharacterSOManager characterSOManager = new(this);
        // await characterSOManager.LoadCSV();
    }

    [ContextMenu("Load CSV")]
    private async void LoadCSV()
    {
        CharacterSOManager characterSOManager = new(this);
        await characterSOManager.LoadCSV();
    }

    [ContextMenu("Calc")]
    public async void Calc()
    {
        CharacterSOManager characterSOManager = new(this);
        await characterSOManager.Calc();
    }

    [ContextMenu("Test")]
    public void Test()
    {/*
        var MemberDatas = await CSVManager.DeserializeAsync<MemberData>("Members");

        foreach (var item in MemberDatas)
        {
            Debug.Log(item.element_type_name);
            Debug.Log(item.ElementType);

        }*/
    }
}


[Serializable]
public class Status
{
    public WeaponType weaponType = WeaponType.Catalyst;
    public ElementType elementType = ElementType.Electro;
    // 基礎ステータス Lv90
    public float baseAtk;
    public float baseDef;
    public int baseHp;
    public bool hasSelfHarm = false;
    public bool notUseShimenawa = false;
    public bool isFront = true;
    public bool isNightSoul = false;


    public readonly float defaultCritRate = 0.05f;
    public readonly float defaultCritDmg = 0.5f;
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

[Serializable]
public class SelectedWeapon : ISelected
{
    public bool isUse;
    public string name;
    public bool isRequired;

    public WeaponData WeaponData;
    public string Id => WeaponData != null ? WeaponData.name : "";
    public bool IsUse { get => isUse; set => isUse = value; }
}

[Serializable]
public class SelectedMember : ISelected
{
    public bool isUse;
    public string name;
    public string weapon = "";
    public string art_set = "";
    public string option = "";
    public bool isRequired;
    public MemberData Member;
    public string Id => Member.CombinedName;
    public bool IsUse { get => isUse; set => isUse = value; }
}

[Serializable]
public class SelectedArtSetData : ISelected
{
    public bool isUse;
    public string name;
    public int set;
    public string option;
    public bool isRequired;

    public ArtSetData ArtSetData;
    public string Id => name + set + option;
    public bool IsUse { get => isUse; set => isUse = value; }
}

public interface ISelected
{
    public string Id { get; }
    public bool IsUse { get; set; }
}