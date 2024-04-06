using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ArtSetData : BaseData
{

    public string option;
    public int set;

    public float zetsuen;


    // https://www.mum-meblog.com/entry/tyr-utility/csharp-hashset
    public override bool Equals(object obj)
    {
        ArtSetData other = obj as ArtSetData;
        if (other == null) return false;
        return this.name == other.name;
    }

    public override int GetHashCode()
    {
        // hashsetは通るが、sortedsetは通らない
        return name.GetHashCode();
    }
}
