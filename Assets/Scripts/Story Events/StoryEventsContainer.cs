using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Story Events/Events Container")]
public class StoryEventsContainer : ScriptableObject
{
    //To do tego jakby frakcje mialy rozne historie
    [field: SerializeField] public List<StoryEvent> Stories { get; private set; } = new List<StoryEvent>();
}
