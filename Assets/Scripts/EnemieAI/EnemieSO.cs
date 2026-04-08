using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemieSO", menuName = "Scriptable Objects/Enemies/EnemieSO")]
public class EnemieSO : ScriptableObject
{
    public int Health;
    public int Power;
    public List<EnemieBuildingSO> BuildingsToSpawn = new();
    //Trzeba przerobiæ budynki ¿eby spawonowa³y enemiesów
}

