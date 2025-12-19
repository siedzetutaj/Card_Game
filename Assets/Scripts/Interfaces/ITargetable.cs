using UnityEngine;

public interface ITargetable
{
    Transform TargetTransform { get; }
    bool IsAlive { get; }
    int TargetAmount { get; set; }

    bool IsAlly(bool isPlayerUnit);
    void TakeDamage(int damage, IAttacker attacker);
    void OnDeath(IAttacker attacker);
}