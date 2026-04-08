using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviourSingleton<MapGenerator>
{
    [Header("Generation Info")]
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private float _heightDis;
    [SerializeField] private float _widthDis;
    [SerializeField] private Transform _mapParent;
    [SerializeField] private Transform _spawnTF;
    [SerializeField] private MapButton _buttonPrefab;
    [SerializeField] private List<MapButton> _buttonList = new();


    public void GenerateMap(int genSeed)
    {
        ResetMap();

        Random.InitState(genSeed);

        int buttonsInRowCounter = 0; //przydaloby sie ze w rzedzie musza zawsze co najmniej 2 przyciski
        
        
        for (int y = 1; y < _rowCount; y++)
        {
            for (int x = 0; x < _columnCount; x++)
            {
                MapButton newButton = Instantiate(_buttonPrefab, _spawnTF.position, 
                    Quaternion.identity, _mapParent);
                
                newButton.transform.position += new Vector3(0, _heightDis * y);
            } 
            
        }
    }

    private void ResetMap()
    {
        _buttonList.ForEach(x => Destroy(x.gameObject));
        _buttonList.Clear();
    }

}
