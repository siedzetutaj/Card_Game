using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MapConnection
{
    public MapButton Button;
    public RectTransform ConnectionGraphic;

    public MapConnection(MapButton newButton, RectTransform conGraphic)
    {
        Button = newButton;
        ConnectionGraphic = conGraphic;
    }
}

public class MapButton : MonoBehaviour
{
    //public MapEvent Event; zamiast przypisywania eventa przy generacji lepiej przypisac typ i losowac encounter w trakcie runa
    public MapEventType EventType;
    public Vector2Int PosID;
    public HashSet<int> PossibleConnections = new();
    public List<MapConnection> Connections = new();
    [SerializeField] private Image _buttonImage;

    [Header("Podczas Gry")]
    [SerializeField] private bool _canBeMovedTo = false;

    public void UpdateButton(MapEventType newType, Sprite newSprite) //w przyszlosci mozna zmieniac grafiki, np ze gracz ukonczyl poziom
    {
        EventType = newType;
        _buttonImage.sprite = newSprite;
    }
    
    public void ChangeCanBeMovedTo(bool isOn)
    {
        _canBeMovedTo = isOn;
        //tutaj mozna jakies animacje dodac
    }

    public void MoveTo()
    {
        if (!_canBeMovedTo || MapController.Instance.State != MapState.ChoosingEvent)
            return;

        MapController.Instance.MovePlayer(this);
    }
}
