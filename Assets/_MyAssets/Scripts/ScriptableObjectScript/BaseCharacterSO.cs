using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public abstract class BaseCharacterSO : ScriptableObject
{
    public Status status;
    public Ascend ascend;
    public string Name => name;
    public string WeaponType => status.WeaponTypeName;
    bool isSub = false;

    public abstract Dictionary<string, string> CalcDmg(Data data);
    public SelectedWeapon[] selectedWeapon;
    public SelectedMember[] selectedMember;
    public SelectedArtSetData[] selectedArtSet;


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
        }).ToArray();

        var MemberDatas = await CSVManager.DeserializeAsync<MemberData>("Members");
        selectedMember = MemberDatas.Select(member => new SelectedMember()
        {
            Member = member,
            name = member.name,
            weapon = member.weapon,
            art_set = member.art_set,
            option = member.option,
        }).ToArray();

        var ArtSetDatas = await CSVManager.DeserializeAsync<ArtSetData>("ArtSet");
        selectedArtSet = ArtSetDatas.Select(artSetData => new SelectedArtSetData()
        {
            ArtSetData = artSetData,
            name = artSetData.name,
            set = artSetData.set,
            option = artSetData.option,
        }).ToArray();

        Debug.Log("完了");
    }

    [ContextMenu("Calc")]
    public async void Calc()
    {
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
        var weaponDatas = selectedWeapon
           .Where(s => s.isUse)
           .Select(s => s.WeaponData)
           .ToArray();

        var memberDatas = selectedMember
            .Where(s => s.isUse)
            .Select(s => s.Member)
            .ToArray();

        var artSetDatas_notSkipped = selectedArtSet
            .Select(s => s.ArtSetData)
            .ToArray();

        var artSetDatas = selectedArtSet
            .Where(s => s.isUse)
            .Select(s => s.ArtSetData)
            .ToArray();

        var artifactDatas = await CSVManager.DeserializeAsync<ArtifactData>("Artifacts");

        artifactDatas = artifactDatas.Where(data => data.skip != 1).ToArray();

        List<Data> datas = new();


        weaponDatas = weaponDatas
            .Where(weaponData => weaponData.type == WeaponType)
            .ToArray();
        var partyDatas = Party.GetPartyDatas(status.elementType, memberDatas);

        var artifactGroups = Artifact.GetArtifactGroups(isSub, artSetDatas, artSetDatas_notSkipped, artifactDatas);


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

}
