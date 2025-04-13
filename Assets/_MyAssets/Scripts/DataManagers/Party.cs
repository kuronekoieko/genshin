using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Party
{
    public static PartyData[] GetPartyDatas(ElementType characterElementType, MemberData[] memberDatas)
    {
        Debug.Log("パーティ計算開始");

        MemberData[] requiredMembers = memberDatas.Where(memberData => memberData.isRequired).ToArray();

        var membersList = new List<SortedSet<MemberData>>();
        foreach (var member1 in memberDatas)
        {
            foreach (var member2 in memberDatas)
            {
                foreach (var member3 in memberDatas)
                {
                    // SortedSetだとGetHashCode()がうごかないため、先にHashSetにする
                    var partyMemberHashSet = new HashSet<MemberData>
                    {
                        member1,
                        member2,
                        member3
                    };
                    var partyMemberSortedSet = new SortedSet<MemberData>(partyMemberHashSet);

                    string newName = string.Join("+", partyMemberSortedSet.Select(memberData => memberData.CombinedName).ToArray());

                    if (IsDuplicate(membersList, newName)) continue;
                    if (IsContainsRequired(requiredMembers, partyMemberSortedSet))
                        // Debug.Log(newName);

                        membersList.Add(partyMemberSortedSet);
                }
            }
        }

        PartyData firstPartyData = new(new MemberData[0], characterElementType);

        SortedSet<PartyData> partyDatas = new()
        {
             firstPartyData
        };


        foreach (SortedSet<MemberData> members in membersList)
        {
            PartyData partyData = new(members.ToArray(), characterElementType);
            partyDatas.Add(partyData);
            // Debug.Log(JsonConvert.SerializeObject(partyData, Formatting.Indented));
        }

        return partyDatas.ToArray();
    }

    static bool IsContainsRequired(MemberData[] requiredMembers, SortedSet<MemberData> partyMemberSortedSet)
    {
        if (requiredMembers.Length == 0) return true;

        foreach (var requiredMember in requiredMembers)
        {
            if (partyMemberSortedSet.Contains(requiredMember))
            {
                return true;
            }
        }
        return false;
    }



    public static bool IsDuplicate(List<SortedSet<MemberData>> partyMenbersList, string newName)
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
