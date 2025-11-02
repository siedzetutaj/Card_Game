using UnityEngine;

public class ResourceWithLimitHandler : ResourceHandler
{
    private int _limit;
    public int Limit
    {
        get => _limit;
        set
        {
            _limit = value;
            _amountText.text = $"{_amount.ToString()}/{_limit.ToString()}";
        }
    }
    public override void SetAmount(int amount)
    {
        _amount = amount;
        if(_amount > _limit) _amount = _limit;
        _amountText.text =$"{_amount.ToString()}/{_limit.ToString()}";

    }
}
