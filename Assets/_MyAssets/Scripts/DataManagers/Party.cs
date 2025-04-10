using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public static class Party
{
    public static MemberData[] GetMemberDatas(ElementType characterElementType)
    {
        Debug.Log("パーティ計算開始");

        var partyMenbersList = new List<SortedSet<MemberData>>();
        foreach (var member1 in CSVManager.MemberDatas)
        {
            foreach (var member2 in CSVManager.MemberDatas)
            {
                foreach (var member3 in CSVManager.MemberDatas)
                {
                    // SortedSetだとGetHashCode()がうごかないため、先にHashSetにする
                    var partyMemberHashSet = new HashSet<MemberData>
                    {
                        member1,
                        member2,
                        member3
                    };
                    var partyMemberSortedSet = new SortedSet<MemberData>(partyMemberHashSet);

                    //  Debug.Log(string.Join("+", partyMenbers.Select(memberData => memberData.CombinedName).ToArray()));

                    string newName = string.Join("+", partyMemberSortedSet.Select(memberData => memberData.CombinedName).ToArray());

                    if (IsDuplicate(partyMenbersList, newName)) continue;
                    // Debug.Log(newName);

                    partyMenbersList.Add(partyMemberSortedSet);
                }
            }
        }

        MemberData firstMemberData = new() { name = "なし" };
        firstMemberData.SetElementalResonance(characterElementType);

        SortedSet<MemberData> memberDatas = new()
        {
            firstMemberData
        };


        foreach (var partyMenbers in partyMenbersList)
        {
            MemberData memberData = Utils.AddInstances(partyMenbers.ToArray());
            string[] combinedNames = partyMenbers.Select(memberData => memberData.CombinedName).ToArray();
            memberData.name = string.Join("+", combinedNames);
            memberData.SetElementalResonance(characterElementType);
            memberData.members = partyMenbers.ToList();
            memberDatas.Add(memberData);
            // Debug.Log(JsonConvert.SerializeObject(memberData, Formatting.Indented));
        }

        return memberDatas.ToArray();
    }



    public static bool IsDuplicate(List<SortedSet<MemberData>> partyMenbersList, string newName)
    {
        foreach (var item in partyMenbersList)
        {
            string oldName = string.Join("+", item.Select(memberData => memberData.CombinedName).ToArray());
            if (newName == oldName)
            {
                return true;
            }
        }
        return false;
    }
}
