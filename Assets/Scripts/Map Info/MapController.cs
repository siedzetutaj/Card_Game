using UnityEngine;

public class MapController : MonoBehaviour
{
    [field: Header("Map Generator")]
    [field: SerializeField] public MapGenerator Generator { get; private set; }
}
