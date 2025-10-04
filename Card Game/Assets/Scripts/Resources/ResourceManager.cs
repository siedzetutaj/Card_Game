using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviourSingleton<ResourceManager>
{
    [SerializeField] private  List<ResourceHandler> _resourceHandlers = new();

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
}
