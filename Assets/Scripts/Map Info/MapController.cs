
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;


public enum MapState
{
    Disabled = 0, ChoosingEvent = 1, MovingPlayer = 2, PlayingEvent = 3,
}

public class MapController : MonoBehaviourSingleton<MapController>
{
    [field: Header("Map Generator")]
    [field: SerializeField] public MapGenerator Generator { get; private set; }

    
    [field: Header("Main Info")]
    [field: SerializeField] public MapState State { get; private set; }
    [field: SerializeField] public Vector2Int PlayerPosID { get; private set; }
    [field: SerializeField] public GameObject MapCanvas { get; private set; }

    [Header("Graphics")]
    [SerializeField] private Image _playerImage;

    [Header("Testing")]
    [SerializeField] private bool _startGame = false;

    //Properties
    private RectTransform _contentParent { get => Generator.MapParent; }

    private void Start()
    {
        if (_startGame)
            StartGame();
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        if (State != MapState.Disabled)
            return;

        Generator.GenerateMap(Random.Range(0,100000)); //zmienic w przyszlosci

        PlayerPosID = new Vector2Int(0,-1);
        Debug.Log( new Vector3(0, -(Generator.BaseHeight / 2f)));
        _playerImage.rectTransform.anchoredPosition = new Vector3(0, -(100f + Generator.BaseHeight / 2f));//resetuje pozycje grafiki gracza
        ChangeState(MapState.ChoosingEvent);

    }

    public void ChangeState(MapState newState)
    {
        MapState lastState = State;
        State = newState;
        Time.timeScale = 1.0f; //ja bym sobie odpuscil z zerowaniem timescale xd. tutaj szybki fix

        switch (lastState)
        {
            case MapState.Disabled:
                break;
            case MapState.ChoosingEvent:
                UpdateButtonRow(false); //deaktywuje przyciski        
                break;
            case MapState.MovingPlayer:
                break;
            case MapState.PlayingEvent: 
                break;
        }

        switch (newState)
        {
            case MapState.Disabled:
                break;
            case MapState.ChoosingEvent:
                UpdateButtonRow(true);
                MapCanvas.SetActive(true);
                break;
            case MapState.MovingPlayer:
                break;
            case MapState.PlayingEvent:
                MapCanvas.SetActive(false);
                break;
        }   
    }

    public void MovePlayer(MapButton mb)
    {
        Debug.Log("Moving Player");
        PlayerPosID = mb.PosID;
        ChangeState(MapState.MovingPlayer);        
        Tween.LocalPosition(_playerImage.transform, mb.transform.parent.localPosition + 
            mb.transform.localPosition, 1f, useUnscaledTime: true)
            .OnComplete(ActivateEvent);
    }

    public void ActivateEvent()
    {
        Debug.Log("Activating Event");
        ChangeState(MapState.PlayingEvent);

        MapButton mb = GetCurrentMapButton();

        //tutaj testowo zostawie zeby zawsze byla walka
        TurnManager.Instance.StartGameManually();

        switch (mb.EventType)
        {
            case MapEventType.Fight:
                //TurnManager.Instance.StartGameManually();
                break;
            case MapEventType.Card:
                break;
            case MapEventType.Shop:
                break;
            case MapEventType.Story:
                break;
        }
    }

    public void UpdateButtonRow(bool isOn)
    {
        if (!isOn) //chcesz je wylaczyc
        {
            foreach(MapButton mb in Generator.Rows[PlayerPosID.y].Buttons) //resetuje rzad na ktorym jest gracz
                mb.ChangeCanBeMovedTo(false);
        }
        else if (PlayerPosID.y == -1) //start gry 
        {
            foreach(MapButton mb in Generator.Rows[0].Buttons) //resetuje rzad na ktorym jest gracz
                mb.ChangeCanBeMovedTo(true);
        }
        else //normalne na prawda
        {
            MapButton mb = Generator.GetMapButton(PlayerPosID);
            if (mb == null)
                return;
            
            foreach (MapConnection mc in mb.Connections)
                mc.Button.ChangeCanBeMovedTo(true);
               
        }
    }

    private MapButton GetCurrentMapButton()
    {
        return Generator.GetMapButton(PlayerPosID);
    }
}
