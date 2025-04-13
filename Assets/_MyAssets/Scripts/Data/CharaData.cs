public class CharaData : BaseData
{
    public Data Data { get; private set; }
    public CharaData(Data data)
    {
        Data = data;

        heal_bonus = data.heal_bonus();
        hp_rate = data.hpPerSum();
        energy_recharge = 1 + data.energy_recharge();
        elemental_mastery = data.elemental_mastery();
        def_rate = data.def_rate();
        atk_rate = data.atk_rate();
        dmg_bonus = data.dmg_bonus();
        pyro_bonus = data.pyro_bonus;
        hydro_bonus = data.hydro_bonus;
        electro_bonus = data.electro_bonus;
        cryo_bonus = data.cryo_bonus;
        geo_bonus = data.geo_bonus;
        anemo_bonus = data.anemo_bonus;
        dendro_bonus = data.dendro_bonus;
        physics_bonus = data.physics_bonus;
        normal_atk_bonus = data.normal_atk_bonus;
        charged_atk_bonus = data.charged_atk_bonus;
        plugged_atk_bonus = data.plugged_atk_bonus;
        skill_bonus = data.skill_bonus;
        burst_bonus = data.burst_bonus;
        atk_speed = data.atk_speed;
        crit_rate = data.crit_rate();
        crit_rate_normal_atk = data.crit_rate_normal_atk;
        crit_rate_charged_atk = data.crit_rate_charged_atk;
        crit_rate_plugged_atk = data.crit_rate_plugged_atk;
        crit_rate_skill = data.crit_rate_skill;
        crit_rate_burst = data.crit_rate_burst;
        crit_dmg = data.crit_dmg();
        crit_dmg_plugged = data.crit_dmg_plugged;
        crit_dmg_burst = data.crit_dmg_burst;
        add = data.add;
        add_normal_atk = data.add_normal_atk;
        add_charged_atk = data.add_charged_atk;
        add_plugged_atk = data.add_plugged_atk;
        add_skill = data.add_skill;
        add_burst = data.add_burst;
        res = data.res;


        GetCharaData(data);
    }

    public float ElementalDmgBonus(ElementType elementType)
    {
        return elementType switch
        {
            ElementType.Pyro => pyro_bonus,
            ElementType.Hydro => hydro_bonus,
            ElementType.Electro => electro_bonus,
            ElementType.Cryo => cryo_bonus,
            ElementType.Geo => geo_bonus,
            ElementType.Anemo => anemo_bonus,
            ElementType.Dendro => dendro_bonus,
            ElementType.Physics => physics_bonus,
            _ => 0,
        };
    }

    public float GetElementalRes(ElementType elementType)
    {
        return elementType switch
        {
            ElementType.Pyro => pyro_res,
            ElementType.Hydro => hydro_res,
            ElementType.Electro => electro_res,
            ElementType.Cryo => cryo_res,
            ElementType.Geo => geo_res,
            ElementType.Anemo => anemo_res,
            ElementType.Dendro => dendro_res,
            ElementType.Physics => physics_res,
            _ => 0,
        };
    }

    void GetCharaData(Data data)
    {
        // CharaData charaData = new(data);

        hp = data.status.baseHp * (1 + hp_rate) + data.hp;
        def = data.status.baseDef * (1 + def_rate) + data.def;

        var dmgAdd_sekikaku = def * data.weapon.sekikaku;
        add_normal_atk += dmgAdd_sekikaku;
        add_charged_atk += dmgAdd_sekikaku;

        var dmgAdd_cinnabar = def * data.weapon.cinnabar;
        add_skill += dmgAdd_cinnabar;

        var homa_atkAdd = hp * data.weapon.homa;
        var sekisa_atkAdd = elemental_mastery * data.weapon.sekisha;
        var kusanagi_atkAdd = (energy_recharge - 1) * data.weapon.kusanagi;

        atk
            = data.base_atk() * (1 + atk_rate)
            + data.atk
            + homa_atkAdd
            + sekisa_atkAdd
            + kusanagi_atkAdd;

    }


    public (float, Crit) ExpectedDmgSum(
    AttackType attackType,
    float[] atkRates,
    ElementType elementType = ElementType.None,
    ReferenceStatus referenceStatus = ReferenceStatus.Atk,
    float er_multi = 1,
    float er_add = 0)
    {
        if (elementType == ElementType.None) elementType = Data.status.elementType;
        ExpectedDamage expectedDamage = new(attackType, elementType, this, Data.artSub);

        float result = expectedDamage.GetExpectedDamageSum(referenceStatus, atkRates, er_multi, er_add);

        return (result, expectedDamage.Crit);
    }

    public (float, Crit) ExpectedDmg(
        AttackType attackType,
        float atkRate,
        ElementType elementType = ElementType.None,
        ReferenceStatus referenceStatus = ReferenceStatus.Atk,
        float er_multi = 1,
        float er_add = 0)
    {
        if (elementType == ElementType.None) elementType = Data.status.elementType;
        ExpectedDamage expectedDamage = new(attackType, elementType, this, Data.artSub);

        float result = expectedDamage.GetExpectedDamage(referenceStatus, atkRate, er_multi, er_add);

        return (result, expectedDamage.Crit);
    }
}