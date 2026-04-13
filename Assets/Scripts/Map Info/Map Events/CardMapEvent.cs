using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Map Events/Card")]
public class CardMapEvent : MapEvent
{
    public override MapEventType Type => MapEventType.Card;
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}
