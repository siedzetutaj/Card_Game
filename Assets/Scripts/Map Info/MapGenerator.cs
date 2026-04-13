using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class MapRow
{
    public RectTransform Parent;
    public List<MapButton> Buttons = new();
}

public class MapGenerator : MonoBehaviourSingleton<MapGenerator>
{
    [Header("Generation Info")]
    [SerializeField] private int _maxRowCount;
    [SerializeField] private int _maxColumnCount;
    [SerializeField] private float _heightDis;
    [SerializeField] private float _widthDis;
    [SerializeField] private int _minButtonsPerRow = 2;
    [SerializeField] private int _additionalButtonChance = 5;
    [SerializeField] private int _testSeed;
    [SerializeField] private RectTransform _mapParent;
    [SerializeField] private List<MapRow> _rows = new();
    [Header("Prefabs")]
    [SerializeField] private RectTransform _mapRowParentPrefab;
    [SerializeField] private MapButton _buttonPrefab;
    [SerializeField] private RectTransform _connectionPrefab;

    [ContextMenu("Generate Test Map")]
    public void GenerateTest()
    {
        GenerateMap(_testSeed);
    }

    public void GenerateMap(int genSeed)
    {
        ResetMap();

        Random.InitState(genSeed);

        _mapParent.sizeDelta = new Vector2(_mapParent.sizeDelta.x, _heightDis * (_maxRowCount + 1));

        //pierwszy rzad tworzymy oddzielnie
        CreateFirstRow();

        for (int i = 1; i < _maxRowCount; i++)
            CreateRow(i);
    }

    private void CreateFirstRow()
    {
        SpawnRowParent(0);
        List<int> buttonsToSpawn = GetRandomButtonList(Random.Range(_minButtonsPerRow, _maxColumnCount - 1));
        buttonsToSpawn.Sort();
        SpawnButtons(0, buttonsToSpawn);
    }

    private void CreateRow(int curRow)
    {
        if (curRow >= _maxRowCount)
            return;

        SpawnRowParent(curRow);

        List<MapButton> mb = _rows[curRow - 1].Buttons;

        List<int> buttonsToSpawn = new();
        HashSet<int> uniqueButtons = new();

        //bazowe
        for (int i = 0; i < mb.Count; i++)       
            AddRandomConnection(i, uniqueButtons, mb);

        //DODANIE SPECJALNYCH LOSOWO
        for (int i = 0; i < mb.Count; i++)
        {
            if (Random.Range(0,_additionalButtonChance) != 0) //idk czemu continue jak moge bez, ale je kocham!
                continue;

            AddRandomConnection(i, uniqueButtons, mb);
        }


        //SPRAWDZENIE CZY JEST WYSTARCZAJACO PRZYCISKOW, JESLI NIE TO DODAJ JAKIS (NIE ZAWSZE SIE UDA)
        if (uniqueButtons.Count < _minButtonsPerRow)
            AddRandomConnection(Random.Range(0, mb.Count), uniqueButtons, mb);

        //przepisuje choc nie trzeba ale tam kto sie bedzie czepial
        buttonsToSpawn = uniqueButtons.ToList();
        buttonsToSpawn.Sort();

        SpawnButtons(curRow, buttonsToSpawn);
        ConnectButtons(curRow);
    }

    private void AddRandomConnection(int i, HashSet<int> uniqueButtons, List<MapButton> mb)
    {
        int randPos = mb[i].Pos.x + Random.Range(-1, 2);
        randPos = Mathf.Clamp(randPos, 0, _maxColumnCount - 1);

        uniqueButtons.Add(randPos);
        mb[i].PossibleConnections.Add(randPos);
    }

    private void SpawnRowParent(int curRow)
    {
        _rows[curRow].Parent = Instantiate(_mapRowParentPrefab, _mapParent);
        _rows[curRow].Parent.localPosition = new Vector3(0, -_mapParent.sizeDelta.y + (curRow + 1) * _heightDis);
        _rows[curRow].Parent.offsetMin = new Vector2(0, _rows[curRow].Parent.offsetMin.y);
        _rows[curRow].Parent.offsetMax = new Vector2(0, _rows[curRow].Parent.offsetMax.y);
    }

    private void SpawnButtons(int curRow, List<int> buttonsToSpawn)
    {
        for(int i = 0; i < buttonsToSpawn.Count; i++)
        {
            MapButton newButton = Instantiate(_buttonPrefab, _rows[curRow].Parent);
            float newX = (-((_maxColumnCount - 1) / 2.0f) + buttonsToSpawn[i]) * _widthDis;

            newButton.transform.localPosition = new Vector3(newX, 0);
            
            newButton.Pos = new Vector2Int(buttonsToSpawn[i], curRow);
            _rows[curRow].Buttons.Add(newButton);        
        } 
    }

    //laczy przyciski do poprzedniego rzedu
    private void ConnectButtons(int curRow)
    {
        List<MapButton> mb = _rows[curRow - 1].Buttons;

        for (int i = 0; i < mb.Count; i++)
        {
            foreach (int x in mb[i].PossibleConnections)
            {
                MapButton newButton = _rows[curRow].Buttons.Find(a => a.Pos.x == x);

                if (newButton == null)
                    continue;

                mb[i].Connections.Add(new MapConnection(newButton, SpawnConnectionGraphic(mb[i], newButton)));
            }
        }          
    }

    private RectTransform SpawnConnectionGraphic(MapButton mb1, MapButton mb2)
    {
        RectTransform newCon = Instantiate(_connectionPrefab, mb2.transform);
        newCon.position = (mb1.transform.position + mb2.transform.position) / 2.0f;

        Vector2 dir = (mb2.transform.position - mb1.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        newCon.localEulerAngles = new Vector3(0,0, angle);

        return newCon;
    }

    //bierze losowe kolumny czy coś
    private List<int> GetRandomButtonList(int howMany)
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

            if (_rows[r].Parent != null)
                Destroy(_rows[r].Parent.gameObject);

            _rows[r].Buttons.Clear();
        }

        //gc tego nienawidzi!
        _rows.Clear();
        
        //dodajemy z powrotem
        for (int r = 0; r < _maxRowCount; r++)
            _rows.Add(new MapRow());
    }

    private int RandomDirection() => Random.Range(0,2) * 2 - 1;
}
