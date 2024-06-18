using System;
using System.Collections.Generic;
using Godot;
using y1000.Source.Control;
using y1000.Source.Control.Bottom;
using y1000.Source.Control.RightSide;
using y1000.Source.Input;

namespace y1000.Source.Event;

public class EventMediator
{
    private BottomControl? _bottomControl;
    
    private RightControl? _rightControl;
    
    private DropItemUI? _dropItemUi;

    private Action<IClientEvent>? _clientEventSender;

    private Action<DragInventorySlotEvent>? _dragItemHandler;
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

    public void RegisterEventHandler()
    {
        
    }

    public void SetComponent(DropItemUI dropItemUi)
    {
        _dropItemUi = dropItemUi;
    }

    public void SetComponent(RightControl rightControl)
    {
        _rightControl = rightControl;
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
    }
}