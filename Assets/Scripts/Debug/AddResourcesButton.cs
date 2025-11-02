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
            money.SetAmount(money.GetAmount()+10);

        var population = _resourceManager.FindResource(ResourceType.population) as ResourceWithLimitHandler;
        if(population != null)
        {
            population.Limit += 10;
            population.SetAmount(population.GetAmount() + 10);
        }

    }
}
