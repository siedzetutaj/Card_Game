using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action OnClick;
    public Action OnMouseEnter;
    public Action OnMouseExit;


    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke();
    }
}
