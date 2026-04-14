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
    public Vector2Int Pos;
    public HashSet<int> PossibleConnections = new();
    public List<MapConnection> Connections = new();
    [SerializeField] private Image _buttonImage;

    public void UpdateButton(MapEventType newType, Sprite newSprite) //w przyszlosci mozna zmieniac grafiki, np ze gracz ukonczyl poziom
    {
        EventType = newType;
        _buttonImage.sprite = newSprite;
    }
}
