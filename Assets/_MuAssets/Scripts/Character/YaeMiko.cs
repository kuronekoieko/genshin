using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YaeMiko
{
    // 基礎ステータス Lv90
    private float baseAtk = 340;
    private float baseDef = 569;
    private float baseCritRate = 0.05f;
    private float baseCritDmg = 0.5f;
    private float baseHp = 10372;

    //突破ステータス
    private float accendCritRate = 0.192f;
    private float accendCritDmg = 0;
    private float accendDmgBonus = 0;
    private float accendAtkPer = 0;
    private float accendEnergyRecharge = 0;
    private float accendHpPer = 0;
    private float accendDefPer = 0;

    // スキル Lv9
    private float[] skillPerArray = { 1.031f, 1.289f, 1.612f };

    // 固有天賦
    private float passive_dmgBonusPerEM = 0.15f * 0.01f;

   
}
