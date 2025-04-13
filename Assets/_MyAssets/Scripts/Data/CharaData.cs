public class CharaData : BaseData
{
    public Data Data { get; private set; }
    public CharaData(Data data)
    {
        Data = data;

        Utils.CopyBaseFields<BaseData>(data, this);

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
            = data.BaseAtk * (1 + atk_rate)
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