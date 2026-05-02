using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryEventManager : MonoBehaviourSingleton<StoryEventManager>
{
    [Header("Main")]
    [SerializeField] private GameObject _storyEventCanvas;
    [SerializeField] private TextMeshProUGUI _textHolder;
    [SerializeField] private Image _BG;
    [SerializeField] private List<StoryEventButton> _buttons;
    [SerializeField] private List<StoryEvent> _availableStories = new();
    [SerializeField] private StoryEventsContainer _testingContainer;  //do usuniecia kiedys

    [Header("Parameters")]
    [SerializeField] private float _charWaitTime;


    //Other variables
    private StoryEventsContainer _currentContainer; 
    private StoryEvent _currentStoryEvent;

    private void Start()
    {
        ToggleUI(false);
        AddStories(_testingContainer);
    }

    public void AddStories(StoryEventsContainer newContainer)
    {
        _availableStories.Clear();
        _availableStories = new List<StoryEvent>(newContainer.Stories);
        _currentContainer = newContainer;
    }

    public StoryEvent GetRandomStory()
    {
        if (_availableStories.Count == 0)
            return null;
        
        int randIdx = Random.Range(0, _availableStories.Count);
        StoryEvent newStoryEvent = _availableStories[randIdx];
        _availableStories.RemoveAt(randIdx);

        if (_availableStories.Count == 0 && _currentContainer != null) //tak na wszelki wypadek
            AddStories(_currentContainer);

        return newStoryEvent;
    }

    public void ToggleUI(bool isOn)
    {
        _storyEventCanvas.gameObject.SetActive(isOn);
    }

    public void ActivateStory()
    {
        _currentStoryEvent = GetRandomStory();   
        ToggleUI(true);
        HandleNextStoryEvent(0);
    }
    
    public void HandleNextStoryEvent(int partIdx)
    {   
        if (partIdx == -1)
        {
            EndStory();
            return; 
        }

        StartCoroutine(WaitForStoryEvent(_currentStoryEvent, partIdx));
    }

    public void EndStory()
    {
        ToggleUI(false);
        MapController.Instance.ChangeState(MapState.ChoosingEvent);
    }

    private IEnumerator WaitForStoryEvent(StoryEvent se, int partIdx)
    {
        for (int i = 0; i < _buttons.Count; i++) //nie ma chyba sensu za kazdym nowe przyciski, czytaj nie chce mi sie XD
            _buttons[i].gameObject.SetActive(false);
        
        StoryEventPart newPart = se.Parts[partIdx];
        _BG.sprite = newPart.BGSprite;

        _textHolder.text = newPart.Text;
        _textHolder.maxVisibleCharacters = 0;
        var newWait = new WaitForSeconds(_charWaitTime);

        //update text
        for (int i = 0; i < newPart.Text.Length; i++)
        {
            _textHolder.maxVisibleCharacters++;
            yield return newWait;
        }

        //Show buttons
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (i >= newPart.Choices.Count)
                continue;

            bool doesEndStory = newPart.Choices[i].NextPartIndex < 0 || 
                newPart.Choices[i].NextPartIndex >= se.Parts.Count;

            _buttons[i].gameObject.SetActive(true);
            _buttons[i].UpdateInfo(newPart.Choices[i], doesEndStory ? -1 : newPart.Choices[i].NextPartIndex);
        }

        if (newPart.Choices.Count == 0)
        {
            yield return new WaitForSeconds(1.5f);
            EndStory();
        }
    }
}
