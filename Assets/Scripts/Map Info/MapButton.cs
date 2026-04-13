using System.Collections.Generic;
using UnityEngine;

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
    [field: SerializeField] public Vector2Int Pos { get; set; }
    public HashSet<int> PossibleConnections = new();
    public List<MapConnection> Connections = new();
}
