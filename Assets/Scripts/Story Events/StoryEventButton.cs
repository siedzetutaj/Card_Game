using TMPro;
using UnityEngine;

public class StoryEventButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textHolder;

    private int _nextPartIndex = 0;
    private StoryEventChoice _currentChoice = null;

    public void UpdateInfo(StoryEventChoice newChoice, int newPartIndex)
    {
        _nextPartIndex = newPartIndex;
        _textHolder.text = "";

        for (int i = 0; i < newChoice.Parts.Count; i++)
            _textHolder.text += $"{string.Format(newChoice.Parts[i].GetText(), newChoice.Parts[i].Value)}. ";
        
        _currentChoice = newChoice;
    }   

    public void ActivateButton()
    {
        bool canHandleNextStoryEvent = true;

        if (_currentChoice != null)
        {       
            for (int i = 0; i < _currentChoice.Parts.Count; i++)
            {
                _currentChoice.Parts[i].Activate(_nextPartIndex, out bool waitForSpecialEvent);

                if (waitForSpecialEvent) //special event, np: wybieranie kart, wiec story event musi poczekac
                    canHandleNextStoryEvent = false;
            }              
        }

        if (canHandleNextStoryEvent)
            StoryEventManager.Instance.HandleNextStoryEvent(_nextPartIndex);  
    }
}
