
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

public class MapController : MonoBehaviourSingleton<MapController>
{
    [field: Header("Map Generator")]
    [field: SerializeField] public MapGenerator Generator { get; private set; }
    
    [Header("Graphics")]
    [SerializeField] private Image _playerImage;

    public void ChangePos()
    {
        
    }

    public void MovePlayer(MapButton mb)
    {
        Tween.Custom(0f, 1f, 1, onValueChange: (float t) =>
        {
            if (mb.transform != null)
            {
                _playerImage.transform.position = Vector3.Lerp(_playerImage.transform.position, 
                    mb.transform.position, t);
            }
        });
    }
}
