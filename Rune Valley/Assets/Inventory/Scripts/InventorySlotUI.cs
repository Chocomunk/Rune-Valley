using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[SelectionBase]
public class InventorySlotUI : Selectable, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler{

    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;

    public void OnPointerClick(PointerEventData eventData)
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
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        this.DoStateTransition(SelectionState.Pressed, true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        this.DoStateTransition(SelectionState.Highlighted, false);
        base.OnPointerUp(eventData);
    }
}
