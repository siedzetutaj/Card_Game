using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Map Events/Fight")]
public class FightMapEvent : MapEvent
{
    public override MapEventType Type => MapEventType.Fight;
    
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}
