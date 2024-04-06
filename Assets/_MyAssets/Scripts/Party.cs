using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public static class Party
{
    public static PartyData[] GetPartyDatas(ElementType characterElementType)
    {
        Debug.Log("パーティ計算開始");

        var partyMenbersList = new List<SortedSet<PartyData>>();
        foreach (var member1 in CSVManager.PartyDatas)
        {
            foreach (var member2 in CSVManager.PartyDatas)
            {
                foreach (var member3 in CSVManager.PartyDatas)
                {
                    // SortedSetだとGetHashCode()がうごかないため、先にHashSetにする
                    var partyMemberHashSet = new HashSet<PartyData>
                    {
                        member1,
                        member2,
                        member3
                    };
                    var partyMemberSortedSet = new SortedSet<PartyData>(partyMemberHashSet);

                    //  Debug.Log(string.Join("+", partyMenbers.Select(partyData => partyData.CombinedName).ToArray()));

                    string newName = string.Join("+", partyMemberSortedSet.Select(partyData => partyData.CombinedName).ToArray());

                    if (IsDuplicate(partyMenbersList, newName)) continue;
                    // Debug.Log(newName);

                    partyMenbersList.Add(partyMemberSortedSet);
                }
            }
        }



        SortedSet<PartyData> partyDatas = new()
        {
            new PartyData { name = "なし" }
        };
        foreach (var partyMenbers in partyMenbersList)
        {
            PartyData partyData = Utils.AddInstances(partyMenbers.ToArray());
            string[] combinedNames = partyMenbers.Select(partyData => partyData.CombinedName).ToArray();
            partyData.name = string.Join("+", combinedNames);
            partyData.SetElementalResonance(characterElementType);

            partyDatas.Add(partyData);
            // Debug.Log(JsonConvert.SerializeObject(partyData, Formatting.Indented));
        }

        return partyDatas.ToArray();
    }



    public static bool IsDuplicate(List<SortedSet<PartyData>> partyMenbersList, string newName)
    {
        foreach (var item in partyMenbersList)
        {
            string oldName = string.Join("+", item.Select(partyData => partyData.CombinedName).ToArray());
            if (newName == oldName)
            {
                return true;
            }
        }
        return false;
    }
}
