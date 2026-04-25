using UnityEngine;

public enum MapEventType { Fight = 0, Card = 1, Shop = 2, Story = 3 };

public abstract class MapEvent : ScriptableObject
{
    public abstract void Activate();
    public abstract MapEventType Type { get; }
}
