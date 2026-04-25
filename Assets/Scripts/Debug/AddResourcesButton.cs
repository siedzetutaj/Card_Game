using System.Xml.Schema;
using UnityEngine;

public class AddResourcesButton : MonoBehaviour
{
    private ResourceManager _resourceManager;
    private void OnEnable()
    {
        _resourceManager = ResourceManager.Instance;
    }
    public void OnButtonPressed()
    {
        var money = _resourceManager.FindResource(ResourceType.money);
        if(money != null)
            money.IncreaseAmount(10);

        var population = _resourceManager.FindResource(ResourceType.population) as ResourceWithLimitHandler;
        if(population != null)
        {
            population.Limit += 100;
            population.IncreaseAmount(100);
        }

        var food = _resourceManager.FindResource(ResourceType.food) as ResourceWithLimitHandler;
        if (food != null)
        {
            food.Limit = 10;
            food.SetAmount(10);
        }

    }
}
