using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArtifactData : BaseData
{
    public string id;
    public string part;
    public string art_set_name;
    public string art_main;

    public string DisplayName => $"{art_set_name}({SubName})";

    public string MainName
    {
        get
        {
            List<string> infos = new();
            if (string.IsNullOrEmpty(part) == false) infos.Add(part);
            if (string.IsNullOrEmpty(art_set_name) == false) infos.Add(art_set_name);
            if (string.IsNullOrEmpty(art_main) == false) infos.Add(art_main);

            string mainName = string.Join("、", infos);

            return $"{mainName}";
        }
    }

    public string SubName
    {
        get
        {
            List<string> infos = new();

            if (crit_rate > 0) infos.Add($"crit_rate:{crit_rate}");
            if (crit_dmg > 0) infos.Add($"crit_dmg:{crit_dmg}");
            if (atk_rate > 0) infos.Add($"atk_rate:{atk_rate}");
            if (atk > 0) infos.Add($"atk:{atk}");
            if (def_rate > 0) infos.Add($"def_rate:{def_rate}");
            if (hp > 0) infos.Add($"hp:{hp}");
            if (hp_rate > 0) infos.Add($"hp_rate:{hp_rate}");
            if (energy_recharge > 0) infos.Add($"energy_recharge:{energy_recharge}");
            if (elemental_mastery > 0) infos.Add($"elemental_mastery:{elemental_mastery}");

            string combinedInfo = string.Join("+", infos);
            return combinedInfo;
        }
    }
}
