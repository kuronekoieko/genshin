using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SelectedDataSetter
{
    BaseCharacterSO baseCharacterSO;
    public static SelectedDataSetter instance = new();

    public async void SetDatas(BaseCharacterSO baseCharacterSO)
    {
        this.baseCharacterSO = baseCharacterSO;


        await CSVManager.InitializeAsync();

        var WeaponDatas = CSVManager.WeaponDatas;

        var selectedWeapon = WeaponDatas
        .Where(weaponData => weaponData.WeaponType == baseCharacterSO.WeaponType)
        .Select(weaponData =>
        {
            return new SelectedWeapon()
            {
                WeaponData = weaponData,
                name = weaponData.name,
                refinement = weaponData.refinement,
                option = weaponData.option,
            };
        }).ToList();

        baseCharacterSO.selectedWeapons = AddDifference(baseCharacterSO.selectedWeapons, selectedWeapon);

        var MemberDatas = CSVManager.MemberDatas;
        var selectedMember = MemberDatas.Select(member =>
        {
            return new SelectedMember()
            {
                Member = member,
                name = member.name,
                weapon = member.weapon,
                art_set = member.art_set,
                constellationName = member.ConstellationName,
                option = member.option,
            };
        }).ToList();

        baseCharacterSO.selectedMembers = AddDifference(baseCharacterSO.selectedMembers, selectedMember);


        var ArtSetDatas = CSVManager.ArtSetDatas;
        var selectedArtSet = ArtSetDatas.Select(artSetData =>
        {
            return new SelectedArtSetData()
            {
                ArtSetData = artSetData,
                name = artSetData.name,
                set = artSetData.set,
                option = artSetData.option,
            };
        }).ToList();
        baseCharacterSO.selectedArtSets = AddDifference(baseCharacterSO.selectedArtSets, selectedArtSet);


        var artMainHeader = new ArtMainHeader();

        var selectedArtMainSands = artMainHeader.sands.Select(name =>
        {
            var instance = new SelectedArtMainSand()
            {
                name = name,
                isUse = IsUseReferenceStatus(name)
            };
            if (name == "元チャ") instance.isUse = false;
            return instance;
        }).ToList();
        baseCharacterSO.selectedArtMainSands = AddDifference(baseCharacterSO.selectedArtMainSands, selectedArtMainSands);

        var selectedArtMainGoblets = artMainHeader.goblets.Select(name =>
        {
            var instance = new SelectedArtMainGoblet()
            {
                name = name,
                isUse = IsUseReferenceStatus(name)
            };

            if (IsElementBonus(name, out ElementType elementType))
            {
                instance.isUse = elementType == baseCharacterSO.status.elementType;
            }

            return instance;
        }).ToList();

        baseCharacterSO.selectedArtMainGoblets = AddDifference(baseCharacterSO.selectedArtMainGoblets, selectedArtMainGoblets);


        var selectedArtMainCirclets = artMainHeader.circlets.Select(name =>
        {
            var instance = new SelectedArtMainCirclet()
            {
                name = name,
                isUse = IsUseReferenceStatus(name)
            };

            if (name == "治癒効果") instance.isUse = false;
            return instance;
        }).ToList();

        baseCharacterSO.selectedArtMainCirclets = AddDifference(baseCharacterSO.selectedArtMainCirclets, selectedArtMainCirclets);


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("完了");
    }

    bool IsElementBonus(string name, out ElementType elementType)
    {
        string elementTypeNameShort = name.Replace("バフ", "");
        elementType = Utils.GetElementType(elementTypeNameShort);
        return elementType != ElementType.None;
    }

    bool IsUseReferenceStatus(string name)
    {
        ReferenceStatus referenceStatus = Utils.GetReferenceStatus(name);
        if (referenceStatus == ReferenceStatus.None) return true;
        if (referenceStatus == ReferenceStatus.Em) return true;
        if (referenceStatus == baseCharacterSO.status.referenceStatus) return true;
        return false;
    }

    List<T> AddDifference<T>(List<T> existingList, List<T> newList) where T : ISelected
    {
        List<T> tmpList = new();

        // 重複と空id削除
        var existingArray = existingList.DistinctBy(item => item.Id).ToArray();

        foreach (var existingItem in existingArray)
        {
            // Debug.Log("a: " + existingItem.Id);
            if (string.IsNullOrEmpty(existingItem.Id) == false)
            {
                tmpList.Add(existingItem);
            }
        }

        // 上書きor追加
        foreach (var newItem in newList)
        {
            if (tmpList.ContainsBy(item => item.Id == newItem.Id, out var index))
            {
                newItem.IsUse = tmpList[index].IsUse;
                tmpList[index] = newItem;
            }
            else
            {
                tmpList.Add(newItem);
            }
        }

        tmpList = tmpList.OrderBy(item => item.Id).ToList();

        return tmpList;
    }

}

public static class Extension
{

    public static bool ContainsBy<T>(this List<T> self, Func<T, bool> contains, out int index) where T : ISelected
    {
        index = -1;

        for (int i = 0; i < self.Count; i++)
        {
            if (contains(self[i]))
            {
                index = i;
                return true;
            }
        }
        return false;
    }

}