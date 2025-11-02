using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    protected int _amount;
    public int Amount
    {
        get => _amount;
        set => SetAmount(value);
    }
    [SerializeField] protected TextMeshProUGUI _amountText;
    [SerializeField] protected ResourceType _resourceType;
    
    public ResourceType ResourceType
    {
        get => _resourceType;
    }

    public virtual void SetAmount(int amount)
    {
        _amount = amount;
        _amountText.text = _amount.ToString();  
    }  
    public int GetAmount()
    {
        return _amount;
    }
    public bool IsEnoughtResources(int amount)
    {
        if (amount <= _amount) 
        {
            return true;
        }
        return false;
    }
}
