using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public static class Party
{
    public static PartyData[] GetPartyDatas(PartyData[] originPartyDatas, ElementType characterElementType)
    {
        Debug.Log("パーティ計算開始");

        var partyMenbersList = new List<SortedSet<PartyData>>();
        foreach (var member1 in originPartyDatas)
        {
            if (member1.skip == 1) continue;

            foreach (var member2 in originPartyDatas)
            {
                if (member2.skip == 1) continue;

                foreach (var member3 in originPartyDatas)
                {
                    if (member3.skip == 1) continue;

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
            PartyData partyData = AddInstances(partyMenbers.ToArray());
            string[] combinedNames = partyMenbers.Select(partyData => partyData.CombinedName).ToArray();
            partyData.name = string.Join("+", combinedNames);
            partyData.SetElementalResonance(characterElementType);

            partyDatas.Add(partyData);
            // Debug.Log(JsonConvert.SerializeObject(partyData, Formatting.Indented));
        }

        return partyDatas.ToArray();
    }

    public static T AddInstances<T>(T[] instances) where T : new()
    {
        Type type = typeof(T);
        T newInstance = new(); // デフォルトの値をセットする

        foreach (FieldInfo fieldInfo in type.GetFields())
        {
            if (fieldInfo.FieldType == typeof(int))
            {
                int sum = 0;
                foreach (T instance in instances)
                {
                    sum += (int)fieldInfo.GetValue(instance);
                }
                fieldInfo.SetValue(newInstance, sum);
            }
            if (fieldInfo.FieldType == typeof(float))
            {
                float sum = 0;
                foreach (T instance in instances)
                {
                    sum += (float)fieldInfo.GetValue(instance);
                }
                fieldInfo.SetValue(newInstance, sum);
            }
            if (fieldInfo.FieldType == typeof(bool))
            {
                bool flag = false;
                foreach (T instance in instances)
                {
                    flag = (bool)fieldInfo.GetValue(instance);
                    if (flag) break;
                }
                fieldInfo.SetValue(newInstance, flag);
            }
            if (fieldInfo.FieldType == typeof(string))
            {
                string name = "";
                foreach (T instance in instances)
                {
                    name += (string)fieldInfo.GetValue(instance) + "+";
                }
                fieldInfo.SetValue(newInstance, name.TrimEnd('+'));
            }
        }
        return newInstance;
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
