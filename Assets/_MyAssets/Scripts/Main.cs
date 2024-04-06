using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;


public class Main : MonoBehaviour
{
    [SerializeField] BaseCharacter character;
    [SerializeField] bool isSub;


    async void Start()
    {
        await CSVManager.InitializeAsync();

        var texts = await Calc();

        FileWriter.Save(character.Name, texts);
    }





    async UniTask<List<string>> Calc()
    {
        List<Data> datas = GetDatas();

        var results = await GetResultsAsync(datas);

        Debug.Log("計算終了");

        var sortKey = results[0].Keys.ToArray()[4];

        results = results
            .OrderByDescending(result =>
            {

                // float.TryParse(result["通常期待値"], out float val);
                float.TryParse(result[sortKey], out float val);
                return val;
            })
            .Take(100)
            .ToList();

        List<string> texts = new();
        string header = string.Join(",", results[0].Keys.ToArray());
        texts.Add(header);
        foreach (var result in results)
        {
            string line = string.Join(",", result.Values.ToArray());
            texts.Add(line);
        }

        return texts;
    }


    List<Data> GetDatas()
    {
        Debug.Log("組み合わせ作成開始");

        List<Data> datas = new();

        var weaponDatas = CSVManager.weaponDatas
            .Where(weaponData => weaponData.skip != 1)
            .Where(weaponData => weaponData.type == character.WeaponType)
            .ToArray();
        var partyDatas = Party.GetPartyDatas(CSVManager.partyDatas, character.status.elementType);

        var artifactGroups = Artifact.GetArtifactGroups(isSub);


        foreach (var weapon in weaponDatas)
        {
            foreach (var partyData in partyDatas)
            {
                foreach (var artifactGroup in artifactGroups)
                {
                    Data data = GetData(weapon, partyData, artifactGroup);
                    if (data != null) datas.Add(data);
                }
            }

        }
        return datas;
    }


    Data GetData(WeaponData weapon, PartyData partyData, Artifact.ArtifactGroup artifactGroup)
    {
        if (artifactGroup.artSetData.name == "しめ縄4" && character.status.notUseShimenawa) return null;

        bool isGakudan = character.status.weaponType == WeaponType.Catalyst || character.status.weaponType == WeaponType.Bow;

        if (artifactGroup.artSetData.name == "楽団4" && isGakudan == false)
        {
            return null;
        }
        if (artifactGroup.artSetData.name == "剣闘士4" && isGakudan == true)
        {
            return null;
        }

        bool hasSelfHarm = character.status.hasSelfHarm || partyData.has_self_harm;

        if ((artifactGroup.artSetData.name == "ファントム4" ||
             artifactGroup.artSetData.name == "花海4" ||
             artifactGroup.artSetData.name == "辰砂4") &&
            !hasSelfHarm)
        {
            return null;
        }
        if (artifactGroup.artSetData.name == "劇団4(控え)" && character.status.isFront)
        {
            return null;
        }
        if (artifactGroup.artSetData.name == "劇団4(表)" && !character.status.isFront)
        {
            return null;
        }
        if (artifactGroup.artSetData.name == "深林4" && character.status.elementType != ElementType.Dendro)
        {
            return null;
        }

        bool isFrozen = partyData.cryo_count > 0 && partyData.hydro_count > 0;

        if (artifactGroup.artSetData.name == "氷風4(凍結)" && isFrozen == false)
        {
            return null;
        }
        if (artifactGroup.artSetData.name == "氷風4(凍結無し)" && partyData.cryo_count == 0)
        {
            return null;
        }
        if (artifactGroup.artSetData.name == "雷4" && partyData.electro_count == 0)
        {
            return null;
        }

        if (artifactGroup.artMainData.physics_bonus > 0 && character.status.elementType != ElementType.Physics)
        {
            return null;
        }
        if (artifactGroup.artMainData.dmg_bonus > 0 && character.status.elementType == ElementType.Physics)
        {
            return null;
        }
        // TODO:残響


        Data data = new()
        {
            weapon = weapon,
            artMain = artifactGroup.artMainData,
            artSets = artifactGroup.artSetData,
            partyData = partyData,
            artSub = artifactGroup.artSubData,
            status = character.status,
            ascend = character.ascend,
        };
        return data;
    }

    async Task<List<Dictionary<string, string>>> GetResultsAsync(List<Data> datas)
    {
        Debug.Log("ダメージ計算開始");

        List<Dictionary<string, string>> results = new();

        int progress = 0;
        int max = datas.Count;

        foreach (var data in datas)
        {
            Dictionary<string, string> result = character.CalcDmg(data);
            if (result != null) results.Add(result);

            progress++;
            if (progress % 200000 == 0)
            {
                await UniTask.DelayFrame(1);
                Debug.Log("progress: " + progress + "/" + max);
            }
        }

        return results;
    }

}
