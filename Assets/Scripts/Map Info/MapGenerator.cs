using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapRow
{
    public List<MapButton> Buttons = new();
    public RectTransform Parent;
}

public class MapGenerator : MonoBehaviourSingleton<MapGenerator>
{
    [Header("Generation Info")]
    [SerializeField] private int _maxRowCount;
    [SerializeField] private int _maxColumnCount;
    [SerializeField] private float _heightDis;
    [SerializeField] private float _widthDis;
    [SerializeField] private int _testSeed;
    [SerializeField] private RectTransform _mapParent;
    [SerializeField] private RectTransform _mapRowParentPrefab;
    [SerializeField] private MapButton _buttonPrefab;
    //ale pieknosc
    [SerializeField] private List<MapRow> _rows = new();

    [ContextMenu("Generate Test Map")]
    public void GenerateTest()
    {
        GenerateMap(_testSeed);
    }

    public void GenerateMap(int genSeed)
    {
        ResetMap();

        Random.InitState(genSeed);

        //przydaloby sie ze w rzedzie musza zawsze co najmniej 2 przyciski
        _mapParent.sizeDelta = new Vector2(_mapParent.sizeDelta.x, _heightDis * (_maxRowCount + 1));

        for (int i = 0; i < _maxRowCount; i++)
            CreateRow(i);
    }

    //walne to chyba rekurencyjnie
    private void CreateRow(int curRow)
    {
        if (curRow >= _maxRowCount)
            return;

        _rows[curRow].Parent = Instantiate(_mapRowParentPrefab, _mapParent);
        _rows[curRow].Parent.localPosition = new Vector3(0, -_mapParent.sizeDelta.y + (curRow + 1) * _heightDis);
        _rows[curRow].Parent.offsetMin = new Vector2(0, _rows[curRow].Parent.offsetMin.y);
        _rows[curRow].Parent.offsetMax = new Vector2(0, _rows[curRow].Parent.offsetMax.y);

        List<int> columnsToSpawn = GetRandomColumnList(Random.Range(2,5));
        columnsToSpawn.Sort();

        SolveRowConflicts(columnsToSpawn, curRow);


        for(int i = 0; i < columnsToSpawn.Count; i++)
        {
            MapButton newButton = Instantiate(_buttonPrefab, _rows[curRow].Parent);
            float newX = (-((_maxColumnCount - 1) / 2.0f) + columnsToSpawn[i]) * _widthDis;

            newButton.transform.localPosition = new Vector3(newX, 0);
            
            _rows[curRow].Buttons[columnsToSpawn[i]] = newButton;
        } 
    }

    private void SolveRowConflicts(List<int> columns, int curRow)
    {
        if (curRow == 0)
            return;
        
        for (int i = 0; i < _rows[curRow - 1].Buttons.Count; i++)
        {
            
        }
    }
    
    //bierze losowe kolumny czy coś
    private List<int> GetRandomColumnList(int howMany)
    {
        List<int> nrList = new List<int>();
        for (int i = 0; i < _maxColumnCount; i++)
            nrList.Add(i);
        
        for (int i = 0; i < howMany; i++)
        {
            int rnd = Random.Range(i, nrList.Count);
            int temp = nrList[i];
            nrList[i] = nrList[rnd];
            nrList[rnd] = temp;
        }
        
        return nrList.GetRange(0, howMany);
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

            Destroy(_rows[r].Parent.gameObject);
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
