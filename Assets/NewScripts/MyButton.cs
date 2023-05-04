using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool buttonPressed;

    public bool buttonJustPressed;

    private void Update()
    {
        buttonJustPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        buttonJustPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}