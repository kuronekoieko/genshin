using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour, ICalcDmg
{
    public Status status;
    public Ascend ascend;
    public string Name => gameObject.name;
    public string WeaponType => status.WeaponTypeName;
    bool isSub = false;

    public abstract Dictionary<string, string> CalcDmg(Data data);
    public List<SelectedWeapon> selectedWeapon;
    public List<SelectedMember> selectedMember;
    public List<SelectedArtSetData> selectedArtSet;


    [ContextMenu("Load CSV")]
    private async void LoadCSV()
    {
        var WeaponDatas = await CSVManager.DeserializeAsync<WeaponData>("Weapon");

        selectedWeapon = WeaponDatas
        .Where(weaponData => weaponData.type == WeaponType)
        .Select(weaponData => new SelectedWeapon()
        {
            WeaponData = weaponData,
            name = weaponData.name,
        }).ToList();

        var MemberDatas = await CSVManager.DeserializeAsync<MemberData>("Members");
        selectedMember = MemberDatas.Select(member => new SelectedMember()
        {
            Member = member,
            name = member.name,
            weapon = member.weapon,
            art_set = member.art_set,
            option = member.option,
        }).ToList();

        var ArtSetDatas = await CSVManager.DeserializeAsync<ArtSetData>("ArtSet");
        selectedArtSet = ArtSetDatas.Select(artSetData => new SelectedArtSetData()
        {
            ArtSetData = artSetData,
            name = artSetData.name,
            set = artSetData.set,
            option = artSetData.option,
        }).ToList();

        Debug.Log("完了");
    }

    [ContextMenu("Calc")]
    public async void Calc()
    {
        /*
        var weaponDatas = selectedWeapon
            .Where(s => s.isUse)
            .Select(s => s.WeaponData)
            .ToArray();
        foreach (var artSetData in weaponDatas)
        {
            Debug.Log(artSetData.name);
        }
        var memberDatas = selectedMember
            .Where(s => s.isUse)
            .Select(s => s.Member)
            .ToArray();

        foreach (var artSetData in memberDatas)
        {
            Debug.Log(artSetData.name);
        }
        var artSetDatas_notSkipped = selectedArtSet
            .Select(s => s.ArtSetData)
            .ToArray();

        foreach (var artSetData in artSetDatas_notSkipped)
        {
            Debug.Log(artSetData.name);
        }

        var artSetDatas = selectedArtSet
            .Where(s => s.isUse)
            .Select(s => s.ArtSetData)
            .ToArray();

        foreach (var artSetData in artSetDatas)
        {
            Debug.Log(artSetData.name);
        }

        var ArtifactDatas = await CSVManager.DeserializeAsync<ArtifactData>("Artifacts");
        ArtifactDatas = ArtifactDatas.Where(data => data.skip != 1).ToArray();
        //  var ArtSetDatas_notSkipped = await CSVManager.DeserializeAsync<ArtSetData>("ArtSet");

        var partyDatas = Party.GetPartyDatas(this.status.elementType, CSVManager.MemberDatas);

        List<Data> datas = Calculator.GetDatas(this, weaponDatas, memberDatas, artSetDatas, partyDatas);

        var results = await Calculator.GetResultsAsync(datas, this);
        var texts = Calculator.ResultsToList(results);*/
    }


}


public interface ICalcDmg
{
    public abstract Dictionary<string, string> CalcDmg(Data data);

}



[System.Flags]
public enum ReferenceStatus
{
    Atk,
    Def,
    Hp,
    Em,
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
    public string WeaponTypeName
    {
        get
        {
            return weaponType switch
            {
                WeaponType.Sword => "片手剣",
                WeaponType.Claymore => "両手剣",
                WeaponType.Bow => "弓",
                WeaponType.Catalyst => "法器",
                WeaponType.Polearm => "槍",
                _ => "",
            };
        }
    }
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
    Sword,
    Claymore,
    Bow,
    Catalyst,
    Polearm,
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