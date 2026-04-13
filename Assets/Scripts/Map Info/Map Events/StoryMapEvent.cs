using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Map Events/Story")]
public class StoryMapEvent : MapEvent
{
    public override MapEventType Type => MapEventType.Story;

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
    
}
