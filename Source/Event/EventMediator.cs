using System;
using System.Collections.Generic;
using Godot;
using y1000.Source.Control;
using y1000.Source.Control.Bottom;
using y1000.Source.Control.RightSide;
using y1000.Source.Input;
using y1000.Source.Networking.Server;

namespace y1000.Source.Event;

public class EventMediator
{
    private BottomControl? _bottomControl;
    
    private DropItemUI? _dropItemUi;

    private Action<IClientEvent>? _clientEventSender;

    private Action<DragInventorySlotEvent>? _dragItemHandler;

    private UIController? _uiController;
    
    
    public void SetComponent(Action<IClientEvent> sender)
    {
        _clientEventSender = sender;
    }
    
    public void SetComponent(Action<DragInventorySlotEvent> handler)
    {
        _dragItemHandler = handler;
    }
    
    public void SetComponent(BottomControl control)
    {
        _bottomControl = control;
    }

    public void SetComponent(DropItemUI dropItemUi)
    {
        _dropItemUi = dropItemUi;
    }

    public void SetComponent(UIController uiController)
    {
        _uiController = uiController;
    }


    public void NotifyServer(IClientEvent clientEvent)
    {
        _clientEventSender?.Invoke(clientEvent);
    }

    public void NotifyTextArea(string message)
    {
        _bottomControl?.DisplayMessage(message);
    }

    public void NotifyDragItemEvent(DragInventorySlotEvent slotEvent, Vector2 mousePosition, Vector2I characterCoordinate)
    {
        _dropItemUi?.OnDropItem(slotEvent, mousePosition, characterCoordinate);
    }

    public void NotifyUiEvent(IUiEvent e)
    {
        if (e is TextEvent textEvent)
        {
            _bottomControl?.DisplayMessage(textEvent);
        }
        else if (e is DragInventorySlotEvent dragInventorySlotEvent)
        {
            _dragItemHandler?.Invoke(dragInventorySlotEvent);
        }
        else if (e is ClickInventorySlotEvent slotEvent)
        {
            _uiController?.OnClickInventorySlotEvent(slotEvent);
        }
        else if (e is ItemAttributeEvent attributeEvent)
        {
            _uiController?.DisplayItemAttribute(attributeEvent.Item, attributeEvent.Description);
        }
        else if (e is KungFuAttributeEvent kungFuAttributeEvent)
        {
            _uiController?.DisplayKungFuAttribute(kungFuAttributeEvent.KungFu, kungFuAttributeEvent.Description);
        }
    }
}