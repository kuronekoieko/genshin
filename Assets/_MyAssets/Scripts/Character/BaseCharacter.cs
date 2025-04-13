using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour, ICalcDmg
{
    public Status status;
    public Ascend ascend;
    public string Name => gameObject.name;
    public string WeaponType => status.WeaponTypeName;
    bool isSub = false;
    public abstract Dictionary<string, string> CalcDmg(Data data);
}

