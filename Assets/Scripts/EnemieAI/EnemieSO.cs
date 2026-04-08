using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemieSO", menuName = "Scriptable Objects/Enemies/EnemieSO")]
public class EnemieSO : ScriptableObject
{
    public int Health;
    public int StartingPower;
    [Tooltip("It scales with hp, when Enemy will have 0 HP the amount of power below will be added")]
    public float AdditionalPower;
    public List<EnemieBuildingSO> BuildingsToSpawn = new();
    //Trzeba przerobiæ budynki ¿eby spawonowa³y enemiesów
}

