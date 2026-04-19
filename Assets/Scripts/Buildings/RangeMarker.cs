using UnityEngine;
using UnityEngine.UI;

public class RangeMarker : MonoBehaviour
{
    [SerializeField] private RectTransform _rangeCircleRT;
    [SerializeField] private Image _rangeCircleImage;

    public void SetRange(float range)
    {
        _rangeCircleRT.sizeDelta = new Vector2(range, range);
    }   
    public void EnableVisiblity() 
    {
        _rangeCircleImage.enabled = true;
    }
    public void DisableVisiblity()
    {
        _rangeCircleImage.enabled = false;
    }
}
