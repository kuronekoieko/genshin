using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class Main : MonoBehaviour
{

    void Start()
    {

    }




    public Type FindTypeByName(string typeName)
    {
        return Assembly.GetExecutingAssembly()
                       .GetTypes()
                       .FirstOrDefault(t => t.Name == typeName || t.FullName == typeName);
    }


    void CreateInstance(BaseCharacterSO instance, string name)
    {
        string directory = "Assets/_MyAssets/ScriptableObjectScript/";
        string filePath = $"Assets/_MyAssets/ScriptableObjectScript/{name}.asset";
        // Debug.Log(directory);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        AssetDatabase.CreateAsset(instance, filePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Created {instance.name}: " + filePath);
    }
}
