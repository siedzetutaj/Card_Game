using System.Collections.Generic;
using UnityEngine;

public class EnemieResourceManager : MonoBehaviourSingleton<EnemieResourceManager>
{
    [SerializeField] private List<ResourceHandler> _resourceHandlers = new();

    private void OnEnable()
    {
        FindResource(ResourceType.food)?.SetAmount(8);
        GameLogicManager.Instance.OnEndTurn += RefreshFood;
    }

    public ResourceHandler FindResource(ResourceType type)
    {
        foreach (var resource in _resourceHandlers)
        {
            if (resource.ResourceType == type)
            {
                return resource;
            }
        }
        return null;
    }
    private void RefreshFood()
    {
        FindResource(ResourceType.food)?.SetAmount(8);
    }
}
