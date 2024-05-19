using Godot;
using y1000.Source.Animation;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.Entity
{
    public interface IEntity
    {
        string EntityName { get; }

        long Id { get; }

        Vector2I Coordinate { get; }

        Vector2 OffsetPosition => Coordinate.ToPosition();
        
        void Delete() { }

        void Handle(IEntityMessage message)
        {
        }
    }
}