using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviourSingleton<HandController>
{
    [SerializeField] private List<CardHandler> cards = new (); 
    [SerializeField] private float radius = 3f;
    [SerializeField] private float angleRange = 30f;
    [SerializeField] private Vector3 cardOffset = new Vector3(0, 0, 0); 
    [SerializeField] private float animationSpeed = 5f; 

    void Update()
    {
        ArrangeCards();
    }

    void ArrangeCards()
    {
        int count = cards.Count;
        if (count == 0) return;

        float middleIndex = (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            Transform card = cards[i].transform;
            if (card == null) continue;

            float t = (i - middleIndex) / middleIndex; 
            if (count == 1) t = 0;

            float angle = -t * angleRange;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = -Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            Vector3 targetPosition = new Vector3(x, y, 0) + cardOffset;

            card.localPosition = Vector3.Lerp(card.localPosition, targetPosition, Time.deltaTime * animationSpeed);
            card.localRotation = Quaternion.Lerp(card.localRotation, targetRotation, Time.deltaTime * animationSpeed);
        }
    }

    public void AddCard(CardHandler card)
    {
        card.transform.SetParent(transform);
        cards.Add(card);
    }

    public void RemoveCard(CardHandler card)
    {
        cards.Remove(card);
    }
}
