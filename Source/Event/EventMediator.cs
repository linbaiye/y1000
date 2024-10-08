using System;
using y1000.Source.Control;
using y1000.Source.Control.Bottom;
using y1000.Source.Input;
using y1000.Source.Networking.Server;

namespace y1000.Source.Event;

public class EventMediator
{
    private BottomControl? _bottomControl;
    
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

    public void NotifyUiEvent(IUiEvent e)
    {
        if (e is DragInventorySlotEvent dragInventorySlotEvent)
        {
            _dragItemHandler?.Invoke(dragInventorySlotEvent);
        }
        else if (e is ItemAttributeEvent attributeEvent)
        {
            _uiController?.DisplayItemAttribute(attributeEvent);
        }
        else if (e is KungFuAttributeEvent kungFuAttributeEvent)
        {
            _uiController?.DisplayKungFuAttribute(kungFuAttributeEvent.KungFu, kungFuAttributeEvent.Description);
        }
        else if (e is PlayerAttributeMessage message)
        {
            _uiController?.DisplayCharacterAttributes(message.FormatAttributes());
        }
        else if (e is OpenKungFuFormEvent)
        {
            _uiController?.OpenKungFuForm();
        }
    }
}