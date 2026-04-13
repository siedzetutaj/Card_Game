using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Map Events/Shop")]
public class ShopMapEvent : MapEvent
{
    public override MapEventType Type => MapEventType.Shop;
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }
}
