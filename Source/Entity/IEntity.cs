using Godot;
using y1000.code.networking.message;
using y1000.Source.Networking.Server;

namespace y1000.Source.Entity
{
    public interface IEntity
    {
        string EntityName { get; }

        long Id { get; }

        Vector2I Coordinate { get; }

        void Delete() { }

        void Handle(IEntityMessage message)
        {
        }
    }
}