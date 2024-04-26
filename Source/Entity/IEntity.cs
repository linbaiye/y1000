using y1000.code.networking.message;

namespace y1000.Source.Entity
{
    public interface IEntity
    {
        string EntityName { get; }

        long Id { get; }

        void Delete() { }

        void Handle(IEntityMessage message)
        {
        }
    }
}