using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class BaseCharacterSO : ScriptableObject, ICalcDmg
{
    public Status status;
    public Ascend ascend;
    public string Name => name;
    public string WeaponType => status.WeaponTypeName;
    public bool isSub = false;

    public abstract Dictionary<string, string> CalcDmg(Data data);
    public List<SelectedWeapon> selectedWeapons;
    public List<SelectedMember> selectedMembers;
    public List<SelectedArtSetData> selectedArtSets;


    [ContextMenu("Load CSV")]
    private async void LoadCSV()
    {
        var WeaponDatas = await CSVManager.DeserializeAsync<WeaponData>("Weapon");

        var selectedWeapon = WeaponDatas
        .Where(weaponData => weaponData.type == WeaponType)
        .Select(weaponData => new SelectedWeapon()
        {
            WeaponData = weaponData,
            name = weaponData.name,
        }).ToList();

        this.selectedWeapons = AddDifference(this.selectedWeapons, selectedWeapon);

        var MemberDatas = await CSVManager.DeserializeAsync<MemberData>("Members");
        var selectedMember = MemberDatas.Select(member => new SelectedMember()
        {
            Member = member,
            name = member.name,
            weapon = member.weapon,
            art_set = member.art_set,
            option = member.option,
        }).ToList();

        this.selectedMembers = AddDifference(this.selectedMembers, selectedMember);


        var ArtSetDatas = await CSVManager.DeserializeAsync<ArtSetData>("ArtSet");
        var selectedArtSet = ArtSetDatas.Select(artSetData => new SelectedArtSetData()
        {
            ArtSetData = artSetData,
            name = artSetData.name,
            set = artSetData.set,
            option = artSetData.option,
        }).ToList();
        this.selectedArtSets = AddDifference(this.selectedArtSets, selectedArtSet);

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

    [ContextMenu("Calc")]
    public async void Calc()
    {

        // Calculator.GetDatas();
        List<Data> datas = await GetDatas();

        var results = await Calculator.GetResultsAsync(datas, this);
        var texts = Calculator.ResultsToList(results);
        FileWriter.Save(Name, texts);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    async public UniTask<List<Data>> GetDatas()
    {
        Debug.Log("組み合わせ作成開始");

        var weaponDatas = GetWeaponDatas();
        var partyDatas = GetPartyDatas();
        var artifactGroups = await GetArtifactGroups();

        List<Data> datas = new();

        foreach (var weapon in weaponDatas)
        {
            foreach (var partyData in partyDatas)
            {
                foreach (var artifactGroup in artifactGroups)
                {

                    Data data = new()
                    {
                        weapon = weapon,
                        artMainData = artifactGroup.artMainData,
                        artSetData = artifactGroup.artSetData,
                        partyData = partyData,
                        artSub = artifactGroup.artSubData,
                        status = status,
                        ascend = ascend,
                    };

                    if (data.IsSkip() == false) datas.Add(data);
                }
            }

        }
        return datas;
    }

    WeaponData[] GetWeaponDatas()
    {
        Debug.Log("selectedWeapon: " + selectedWeapons.Count);
        foreach (var item in selectedWeapons)
        {
            Debug.Log(item.WeaponData);
        }

        var weaponDatas = selectedWeapons
                .Where(s => s.isUse)
                .Select(s => s.WeaponData)
                .Where(weaponData => weaponData.type == WeaponType)
                .ToArray();

        foreach (var item in weaponDatas)
        {
            Debug.Log(item.skip);
        }
        Debug.Log("weaponDatas: " + weaponDatas.Length);

        return weaponDatas;
    }

    PartyData[] GetPartyDatas()
    {
        var memberDatas = selectedMembers
                   .Where(s => s.isUse)
                   .Select(s => s.Member)
                   .ToArray();

        foreach (var item in memberDatas)
        {
            if (string.IsNullOrEmpty(item.name))
            {
                Debug.LogError(item.name);
            }
            Debug.Log(item.skip);

        }


        var partyDatas = Party.GetPartyDatas(status.elementType, memberDatas);

        Debug.Log("partyDatas: " + partyDatas.Length);

        return partyDatas;
    }


    async UniTask<List<Artifact.ArtifactGroup>> GetArtifactGroups()
    {
        List<Artifact.ArtifactGroup> artifactGroups;
        if (isSub)
        {
            var artSetDatas_notSkipped = selectedArtSets
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
            var artSetDatas = selectedArtSets
                .Where(s => s.isUse)
                .Select(s => s.ArtSetData)
                .ToArray();

            foreach (var item in artSetDatas)
            {
                if (string.IsNullOrEmpty(item.name))
                {
                    Debug.LogError(item.name);
                }
            }

            artifactGroups = Artifact.GetArtifactGroups(artSetDatas);
        }

        Debug.Log("artifactGroups: " + artifactGroups.Count);

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


[Serializable]
public class SelectedWeapon : ISelected
{
    public bool isUse;
    public string name;
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
    public ArtSetData ArtSetData;
    public string Id => name + set + option;
    public bool IsUse { get => isUse; set => isUse = value; }
}

public interface ISelected
{
    public string Id { get; }
    public bool IsUse { get; set; }

}