using UnityEngine;
using UnityEngine.UI;

public class ChangeTimeScaleDebug : MonoBehaviour
{
    public void ChangeTimeScale(Slider timeScaleSlider)
    {
        Time.timeScale = timeScaleSlider.value;
    }
}
