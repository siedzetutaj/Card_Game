using UnityEngine;

public class MapGenerator : MonoBehaviourSingleton<MapGenerator>
{
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private MapButton _mapButtonPrefab;


    public void GenerateMap()
    {
        //
    }

}
