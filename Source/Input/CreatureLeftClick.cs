using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Entity;

namespace y1000.Source.Input;

public class CreatureLeftClick: IInput
{
    private readonly IEntity _entity;
    public CreatureLeftClick(IEntity entity)
    {
        _entity = entity;
    }


    public T Entity<T>() where T : IEntity
    {
        return (T)_entity;
    }
    
    public InputType Type => InputType.LEFT_CLICK;

    public InputPacket ToPacket()
    {
        throw new System.NotImplementedException();
    }
}