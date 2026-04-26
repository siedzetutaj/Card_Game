
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

    [Header("Graphics")]
    [SerializeField] private Image _playerImage;

    //Properties
    private RectTransform _contentParent { get => Generator.MapParent; }


    [ContextMenu("Start Game")]
    public void StartGame()
    {
        if (State != MapState.Disabled)
            return;

        Generator.GenerateMap(Random.Range(0,100000)); //zmienic w przyszlosci

        PlayerPosID = new Vector2Int(0,-1);
        _playerImage.transform.localPosition = new Vector3(0, -(_contentParent.rect.height / 2f + 100f)); //resetuje pozycje grafiki gracza

        ChangeState(MapState.ChoosingEvent);

    }

    public void ChangeState(MapState newState)
    {
        MapState lastState = State;
        State = newState;

        switch (lastState)
        {
            case MapState.Disabled:
                break;
            case MapState.ChoosingEvent:
                UpdateButtonRow(false);
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
                break;
            case MapState.MovingPlayer:
                break;
            case MapState.PlayingEvent:
                break;
        }   
    }

    public void MovePlayer(MapButton mb)
    {
        PlayerPosID = mb.PosID;
        ChangeState(MapState.MovingPlayer);        
        Tween.LocalPosition(_playerImage.transform, mb.transform.parent.localPosition + mb.transform.localPosition, 1f).OnComplete(ActivateEvent);
    }

    public void ActivateEvent()
    {
        //tutaj aktywacja eventu czy cos
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
}
