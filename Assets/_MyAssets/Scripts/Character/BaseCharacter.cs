using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    public Status status;
    public Ascend ascend;
    public string Name => gameObject.name;
    public string WeaponType => status.WeaponTypeName;

    public abstract Dictionary<string, string> CalcDmg(Data data);
    public A[] weaponDatas;
    public B[] partyDatas;

    public C[] artifactGroups;


    [ContextMenu("Method")]
    private async Task Method()
    {
        await CSVManager.InitializeAsync();
        weaponDatas = CSVManager.WeaponDatas
                 .Where(weaponData => weaponData.type == WeaponType)
                 .Select(weaponData => new A()
                 {
                     weaponData = weaponData,
                     name = weaponData.name,
                 })
                 .ToArray();


        partyDatas = Party.GetPartyDatas(status.elementType).Select(partyData => new B()
        {
            partyData = partyData,
            name = partyData.name,
        })
                 .ToArray();

        artifactGroups = Artifact.GetArtifactGroups(false).Select(artifactGroup => new C()
        {
            artifactGroup = artifactGroup,
            name = artifactGroup.artSetData.name,
        }).ToArray();
    }

}

[Serializable]
public class A
{
    public bool isUse;
    public string name;
    public WeaponData weaponData { get; set; }
}

[Serializable]
public class B
{
    public bool isUse;
    public string name;
    public PartyData partyData { get; set; }
}

[Serializable]
public class C
{
    public bool isUse;
    public string name;
    public Artifact.ArtifactGroup artifactGroup { get; set; }
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