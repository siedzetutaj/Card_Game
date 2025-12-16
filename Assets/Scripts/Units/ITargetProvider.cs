using System.Collections.Generic;
using UnityEngine;

public interface ITargetProvider
{
    List<UnitHandler> GetTargets(UnitHandler requester);
}
