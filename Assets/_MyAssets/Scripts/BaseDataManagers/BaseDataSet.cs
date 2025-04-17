using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class BaseDataSet
{
    public WeaponData weaponData;
    public ArtMainData artMainData;
    public ArtSetData artSetData;
    public ArtSubData artSubData;
    public PartyData partyData;
    public Status status;
    public Ascend ascend;

    public BaseDataSet(
        WeaponData weaponData,
        ArtifactGroup artifactGroup,
        PartyData partyData,
        Status status,
        Ascend ascend)
    {
        this.weaponData = weaponData;
        this.artMainData = artifactGroup.artMainData;
        this.artSetData = artifactGroup.artSetData;
        this.artSubData = artifactGroup.artSubData;
        this.partyData = partyData;
        this.status = status;
        this.ascend = ascend;
    }

    public void SetInstance(Data data)
    {
        var baseDatas = new BaseData[] { weaponData, artMainData, artSetData, partyData, artSubData, };
        var baseData = FastInstanceAdder.AddInstances(baseDatas);
        FastFieldCopier.CopyBaseFields(baseData, data);
    }

}
