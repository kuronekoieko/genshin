using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CharacterSOManager
{
    BaseCharacterSO baseCharacterSO;

    public CharacterSOManager(BaseCharacterSO baseCharacterSO)
    {
        this.baseCharacterSO = baseCharacterSO;
    }

    public async UniTask LoadCSV()
    {
        var WeaponDatas = await CSVManager.DeserializeAsync<WeaponData>("Weapon");

        var selectedWeapon = WeaponDatas
        .Where(weaponData => weaponData.WeaponType == baseCharacterSO.WeaponType)
        .Select(weaponData => new SelectedWeapon()
        {
            WeaponData = weaponData,
            name = weaponData.name,
        }).ToList();

        baseCharacterSO.selectedWeapons = AddDifference(baseCharacterSO.selectedWeapons, selectedWeapon);

        var MemberDatas = await CSVManager.DeserializeAsync<MemberData>("Members");
        var selectedMember = MemberDatas.Select(member => new SelectedMember()
        {
            Member = member,
            name = member.name,
            weapon = member.weapon,
            art_set = member.art_set,
            option = member.option,
        }).ToList();

        baseCharacterSO.selectedMembers = AddDifference(baseCharacterSO.selectedMembers, selectedMember);


        var ArtSetDatas = await CSVManager.DeserializeAsync<ArtSetData>("ArtSet");
        var selectedArtSet = ArtSetDatas.Select(artSetData => new SelectedArtSetData()
        {
            ArtSetData = artSetData,
            name = artSetData.name,
            set = artSetData.set,
            option = artSetData.option,
        }).ToList();
        baseCharacterSO.selectedArtSets = AddDifference(baseCharacterSO.selectedArtSets, selectedArtSet);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("完了");
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

        return tmpList;
    }

    public async UniTask Calc()
    {
        // Calculator.GetDatas();
        List<Data> datas = await GetDatas();

        var results = await Calculator.GetResultsAsync(datas, baseCharacterSO);
        var texts = Calculator.ResultsToList(results);
        FileWriter.Save(baseCharacterSO.Name, texts);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    async public UniTask<List<Data>> GetDatas()
    {
        Debug.Log("組み合わせ作成開始");

        var weaponDatas = GetWeaponDatas();
        var partyDatas = GetPartyDatas();
        var artifactGroups = await GetArtifactGroups();

        List<Data> datas = Calculator.GetDatas(baseCharacterSO, weaponDatas, partyDatas, artifactGroups);
        return datas;
    }

    WeaponData[] GetWeaponDatas()
    {
        // Debug.Log("selectedWeapon: " + baseCharacterSO.selectedWeapons.Count);
        foreach (var item in baseCharacterSO.selectedWeapons)
        {
            // Debug.Log(item.WeaponData);
        }

        var weaponDatas = baseCharacterSO.selectedWeapons
                .Where(s => s.isUse)
                .Select(s => s.WeaponData)
                .Where(weaponData => weaponData.WeaponType == baseCharacterSO.WeaponType)
                .ToArray();

        foreach (var item in weaponDatas)
        {
            // Debug.Log(item.skip);
        }
        // Debug.Log("weaponDatas: " + weaponDatas.Length);

        return weaponDatas;
    }

    PartyData[] GetPartyDatas()
    {
        var memberDatas = baseCharacterSO.selectedMembers
                   .Where(s => s.isUse)
                   .Select(s =>
                    {
                        s.Member.isRequired = s.isRequired;
                        return s.Member;
                    })
                   .ToArray();

        foreach (var item in memberDatas)
        {
            if (string.IsNullOrEmpty(item.name))
            {
                Debug.LogError(item.name);
            }
            // Debug.Log(item.ElementType);
            // Debug.Log(item.name + " " + item.isRequired);
        }


        var partyDatas = Party.GetPartyDatas(baseCharacterSO.status.elementType, memberDatas);

        // Debug.Log("partyDatas: " + partyDatas.Length);

        return partyDatas;
    }


    async UniTask<List<Artifact.ArtifactGroup>> GetArtifactGroups()
    {
        List<Artifact.ArtifactGroup> artifactGroups;
        if (baseCharacterSO.isSub)
        {
            var artSetDatas_notSkipped = baseCharacterSO.selectedArtSets
            .Select(s => s.ArtSetData)
            .ToArray();

            foreach (var item in artSetDatas_notSkipped)
            {
                if (string.IsNullOrEmpty(item.name))
                {
                    Debug.LogError(item.name);
                }
            }

            var artifactDatas = await CSVManager.DeserializeAsync<ArtifactData>("Artifacts");
            foreach (var item in artifactDatas)
            {
                if (string.IsNullOrEmpty(item.name))
                {
                    Debug.LogError(item.name);
                }
                Debug.Log(item.skip);
            }
            artifactDatas = artifactDatas.Where(data => data.skip != 1).ToArray();

            artifactGroups = Artifact.GetSubArtifactGroups(artSetDatas_notSkipped, artifactDatas);
        }
        else
        {
            var artSetDatas = baseCharacterSO.selectedArtSets
                .Where(s => s.isUse)
                .Select(s =>
                {
                    s.ArtSetData.isRequired = s.isRequired;
                    return s.ArtSetData;
                })
                .ToArray();

            foreach (var item in artSetDatas)
            {
                if (string.IsNullOrEmpty(item.name))
                {
                    Debug.LogError(item.name);
                }
            }

            artifactGroups = Artifact.GetArtifactGroups(artSetDatas, isTest: true);
        }

        // Debug.Log("artifactGroups: " + artifactGroups.Count);

        return artifactGroups;
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