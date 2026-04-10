using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapRow
{
    public List<MapButton> Buttons = new();
}

public class MapGenerator : MonoBehaviourSingleton<MapGenerator>
{
    [Header("Generation Info")]
    [SerializeField] private int _maxRowCount;
    [SerializeField] private int _maxColumnCount;
    [SerializeField] private float _heightDis;
    [SerializeField] private float _widthDis;
    [SerializeField] private Transform _mapParent;
    [SerializeField] private Transform _spawnTF;
    [SerializeField] private MapButton _buttonPrefab;
    //ale pieknosc
    [SerializeField] private List<MapRow> _rows = new();


    public void GenerateMap(int genSeed)
    {
        ResetMap();

        Random.InitState(genSeed);

        int buttonsInRowCounter = 0; //przydaloby sie ze w rzedzie musza zawsze co najmniej 2 przyciski
                
        /*for (int y = 1; y < _rowCount; y++)
        {
            for (int x = 0; x < _columnCount; x++)
            {
                MapButton newButton = Instantiate(_buttonPrefab, _spawnTF.position, 
                    Quaternion.identity, _mapParent);
                
                newButton.transform.position += new Vector3(0, _heightDis * y);
            } 
            
        }*/
    }

    //walne to chyba rekurencyjnie
    private void CreateColumn(int curRow)
    {
        if (curRow >= _maxRowCount)
            return;

        //nie mozemy miec w rzedzie mniej niz dwoch 
        //int buttonsInRow = _buttonListte
    }

    private void ResetMap()
    {
        //wszystko jest ladnie czyszczone
        for (int r = 0; r < _rows.Count; r++)
        {
            for (int c = 0; c < _rows[r].Buttons.Count; c++)
            {
                if (_rows[r].Buttons[c] == null)
                    continue;
                
                Destroy(_rows[r].Buttons[c].gameObject);
            }
            _rows[r].Buttons.Clear();
        }
        _rows.Clear();

        //dodajemy z powrotem
        for (int r = 0; r < _maxRowCount; r++)
        {
            _rows.Add(new MapRow());
            for (int c = 0; c < _maxColumnCount; c++)
                _rows[r].Buttons.Add(null);
        }
    }

}
