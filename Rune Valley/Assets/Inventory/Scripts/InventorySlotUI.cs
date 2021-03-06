﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[SelectionBase]
public class InventorySlotUI : Selectable, IPointerDownHandler, IPointerUpHandler{

    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            leftClick.Invoke();
        } else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            middleClick.Invoke();
        } else if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightClick.Invoke();
        }

        this.DoStateTransition(SelectionState.Pressed, true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        this.DoStateTransition(SelectionState.Highlighted, true);
        base.OnPointerUp(eventData);
    }
}
