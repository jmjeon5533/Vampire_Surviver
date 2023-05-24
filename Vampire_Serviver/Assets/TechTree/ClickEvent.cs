using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ClickEvent : MonoBehaviour, IPointerDownHandler
{   
    private Action clickAction;

    public void OnPointerDown(PointerEventData eventData)
    {
        clickAction?.Invoke();
    }
    
    public void Init(Action action)
    {
        clickAction = action;
    }
}
