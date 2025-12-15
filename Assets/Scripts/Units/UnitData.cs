
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public Sprite UnitSprite;
    public int UnitFoodCost;
    public int UnitAmount;
    public int UnitHealth;
    public int UnitDamage;
    public int UnitAttackSpeed;
    public int UnitAttackRange;
    public int UnitSpeed;
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
       // SpawnPosionIndex = unitData.SpawnPosionIndex;
    }
}
