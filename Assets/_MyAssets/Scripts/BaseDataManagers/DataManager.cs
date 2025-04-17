using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IekoLibrary;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;


public class DataManager
{

    public static async Task<List<Data>> GetDatas(BaseCharacterSO character, WeaponData[] weaponDatas, PartyData[] partyDatas, List<ArtifactGroup> artifactGroups)
    {
        return await GetDatas(character.status, character.ascend, weaponDatas, partyDatas, artifactGroups);
    }

    static async UniTask<List<Data>> GetDatas(Status status, Ascend ascend, WeaponData[] weaponDatas, PartyData[] partyDatas, List<ArtifactGroup> artifactGroups)
    {
        Debug.Log("組み合わせ作成開始");
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        int progress = 0;
        int max = weaponDatas.Length * partyDatas.Length * artifactGroups.Count;


        List<Data> datas = new();

        foreach (var weapon in weaponDatas)
        {
            foreach (var partyData in partyDatas)
            {
                foreach (var artifactGroup in artifactGroups)
                {
                    BaseDataSet baseDataSet = new(weapon, artifactGroup, partyData, status, ascend);

                    if (DataSkip.IsSkip(baseDataSet) == false)
                    {
                        Data data = new(baseDataSet);
                        datas.Add(data);
                    }

                    progress++;
                    if (sw.ElapsedMilliseconds % 1000 == 0)
                    {
                        await UniTask.DelayFrame(1);
                        //await UniTask.DelayFrame(1);
                        int per = (int)((float)progress / (float)max * 100f);

                        Debug.Log("progress: " + progress + "/" + max + " " + per + "%");
                    }
                }
            }
        }

        sw.Stop();
        Debug.Log("処理時間 " + sw.ElapsedMilliseconds / 1000f + "s");


        return datas;
    }


}
