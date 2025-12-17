
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public Sprite UnitSprite;
    public int UnitFoodCost;
    public int UnitAmount;
    public int UnitHealth;
    public int UnitDamage;
    [Description("the lower the faster")]
    public int UnitAttackSpeed;
    public int UnitAttackRange;
    public int UnitSpeed;
    public int TargetAmount;
    //[Range(0,1)]
   // public int SpawnPosionIndex;
    public UnitData(UnitData unitData)
    {
        UnitSprite = unitData.UnitSprite;
        UnitFoodCost = unitData.UnitFoodCost;
        UnitAmount = unitData.UnitAmount;
        UnitHealth = unitData.UnitHealth;
        UnitDamage = unitData.UnitDamage;
        UnitAttackSpeed = unitData.UnitAttackSpeed;
        UnitAttackRange = unitData.UnitAttackRange;
        UnitSpeed = unitData.UnitSpeed;
        TargetAmount = unitData.TargetAmount;
       // SpawnPosionIndex = unitData.SpawnPosionIndex;
    }
}
