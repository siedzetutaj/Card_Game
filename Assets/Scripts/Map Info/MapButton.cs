using UnityEngine;

public class MapButton : MonoBehaviour
{
    [field: SerializeField] public Vector2Int Pos { get; private set; }
    [field: SerializeField] public MapButton Left { get; private set; }
    [field: SerializeField] public MapButton Middle { get; private set; }
    [field: SerializeField] public MapButton Right { get; private set; }
}
