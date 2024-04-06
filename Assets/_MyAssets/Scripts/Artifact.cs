using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Artifact
{

    public static ArtifactData[] GetArtifactDatas(bool isSub)
    {
        if (isSub) return null;
        var artMainDatas = Artifacts_Main.GetArtMainDatas();
        var artSetDatas = CSVManager.artSetDatas;

        var artifactDatas = new List<ArtifactData>();




        return artifactDatas.ToArray();

    }

}
