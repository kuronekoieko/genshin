using System;
using UnityEngine;

public interface ISelected
{
    public string Id { get; }
    public bool IsUse { get; set; }
}

[Serializable]
public class SelectedWeapon : ISelected
{
    public bool isUse;
    public string name;
    public int refinement = 1;
    public string option = "";
    public WeaponData WeaponData;
    public string Id => WeaponData != null ? WeaponData.Id : "";
    public bool IsUse { get => isUse; set => isUse = value; }
}

[Serializable]
public class SelectedMember : ISelected
{
    public bool isUse;
    public string name;
    public string weapon = "";
    public string art_set = "";
    public string constellationName = "";
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

[Serializable]
public class SelectedArtMainSand : ISelected
{
    public bool isUse = true;
    public string name;
    public string Id => name;
    public bool IsUse { get => isUse; set => isUse = value; }
}

[Serializable]
public class SelectedArtMainGoblet : ISelected
{
    public bool isUse = true;
    public string name;
    public string Id => name;
    public bool IsUse { get => isUse; set => isUse = value; }
}


[Serializable]
public class SelectedArtMainCirclet : ISelected
{
    public bool isUse = true;
    public string name;
    public string Id => name;
    public bool IsUse { get => isUse; set => isUse = value; }
}


