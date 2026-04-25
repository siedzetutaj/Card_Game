using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AdditionalCardsPoolSO", menuName = "Scriptable Objects/Cards/AdditionalCardsPoolSO")]
public class AdditionalCardsPoolSO : ScriptableObject
{
    public List<CardSO> AdditionalCardsPool = new List<CardSO>();
}
