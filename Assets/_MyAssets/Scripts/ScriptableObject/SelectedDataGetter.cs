using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class SelectedDataGetter
{
    BaseCharacterSO baseCharacterSO;

    public static SelectedDataGetter instance = new();


    async public UniTask<List<Data>> GetDatas(BaseCharacterSO baseCharacterSO)
    {
        this.baseCharacterSO = baseCharacterSO;

        Debug.Log("組み合わせ作成開始");
        await CSVManager.InitializeAsync();

        var weaponDatas = GetWeaponDatas();
        var partyDatas = GetPartyDatas();
        var artifactGroups = GetArtifactGroups();

        List<Data> datas = await DataManager.GetDatas(baseCharacterSO, weaponDatas, partyDatas, artifactGroups);
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


    List<ArtifactGroup> GetArtifactGroups()
    {
        List<ArtifactGroup> artifactGroups;
        if (baseCharacterSO.isSub)
        {
            artifactGroups = GetSubArtifactGroups();
        }
        else
        {
            artifactGroups = GetFixedScoreArtifactGroups();
        }

        foreach (var artifactGroup in artifactGroups)
        {
            // Debug.Log("artifactGroups: " + artifactGroup.artMainData.name);
        }

        //Debug.Log("artifactGroups: " + artifactGroups.Count);

        return artifactGroups;
    }

    List<ArtifactGroup> GetFixedScoreArtifactGroups()
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

        ArtMainHeader artMainHeader = new()
        {
            sands = baseCharacterSO.selectedArtMainSands.Where(s => s.isUse).Select(s => s.name).ToArray(),
            goblets = baseCharacterSO.selectedArtMainGoblets.Where(s => s.isUse).Select(s => s.name).ToArray(),
            circlets = baseCharacterSO.selectedArtMainCirclets.Where(s => s.isUse).Select(s => s.name).ToArray(),
        };

        return Artifact.GetFixedScoreArtifactGroups(artSetDatas, artMainHeader, baseCharacterSO.subScore);
    }

    List<ArtifactGroup> GetSubArtifactGroups()
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

        var artifactDatas = baseCharacterSO.selectedArtifactDatas.Select(s => s.artifactData).ToArray();
        foreach (var item in artifactDatas)
        {
            if (string.IsNullOrEmpty(item.name))
            {
                // Debug.LogError(item.name);
            }
        }
        return Artifact.GetSubArtifactGroups(artSetDatas_notSkipped, artifactDatas);
    }


}

