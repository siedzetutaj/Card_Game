using System.Collections.Generic;
using UnityEngine;

public enum StoryEventChoiceType
{
    Text = 0, TakeDamage = 1, Heal = 2, GetCard = 3, //...
}

[System.Serializable]
public class StoryEventChoicePart
{
    [field: SerializeField] public StoryEventChoiceType Type { get; private set; }
    [SerializeField] private string _text;
    [field: SerializeField] public float Value { get; private set; }

    public string GetText()
    {
        return _text; //todo: rozne kolory tekstu
    }

    public void Activate(int nextPartIndex, out bool waitForSpecialEvent)
    {
        //wole jednego switcha niz kilka SO, bo nie bedzie pewnie az tyle typów wyboru
        waitForSpecialEvent = false;

        switch (Type)
        {
            case StoryEventChoiceType.Text:
                break;
            case StoryEventChoiceType.TakeDamage:
                break;
            case StoryEventChoiceType.Heal:
                break;
            case StoryEventChoiceType.GetCard:
                waitForSpecialEvent = true;
                RewardsManager.Instance.ShowCardRewards(() => StoryEventManager.Instance.HandleNextStoryEvent(nextPartIndex));
                break;
        }
    }
}


[System.Serializable]
public class StoryEventChoice
{
    [Tooltip("Wszystko poza zakresem będzie uznane jako koniec Story Eventu")]
    [field: SerializeField] public int NextPartIndex { get; private set; }
    [field: SerializeField] public List<StoryEventChoicePart> Parts { get; private set; } = new(); 
}

[System.Serializable]
public class StoryEventPart
{
    [field: TextArea(3,10)]
    [field: SerializeField] public string Text { get; private set; }
    [field: SerializeField] public Sprite BGSprite { get; private set; }
    [field: SerializeField] public List<StoryEventChoice> Choices { get; private set; } = new();
}

[CreateAssetMenu(menuName = "Scriptable Objects/Story Events/Event")]
public class StoryEvent : ScriptableObject
{
    [field: SerializeField] public List<StoryEventPart> Parts { get; private set; } = new();
}
