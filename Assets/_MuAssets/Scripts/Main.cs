using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    async void Start()
    {
        await CSVManager.InitializeAsync();

        foreach (var item in CSVManager.weaponDatas)
        {
            Debug.Log(JsonUtility.ToJson(item, true));
            // Debug.Log(item.name);
        }
    }


}
