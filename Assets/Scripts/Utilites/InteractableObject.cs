using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        gameObject.GetComponent<ClickableObject>().OnClick += OnObjectClicked;
        gameObject.GetComponent<ClickableObject>().OnMouseEnter += OnObjectMouseEnter;
        gameObject.GetComponent<ClickableObject>().OnMouseExit += OnObjectMouseExit;
    }
    protected virtual void OnDisable()
    {
        gameObject.GetComponent<ClickableObject>().OnClick -= OnObjectClicked;
        gameObject.GetComponent<ClickableObject>().OnMouseEnter -= OnObjectMouseEnter;
        gameObject.GetComponent<ClickableObject>().OnMouseExit -= OnObjectMouseExit;
    }
    protected virtual void OnObjectClicked()
    {

    }
    protected virtual void OnObjectMouseEnter()
    {

    }
    protected virtual void OnObjectMouseExit()
    {

    }
}
