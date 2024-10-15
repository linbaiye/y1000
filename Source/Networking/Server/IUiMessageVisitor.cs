namespace y1000.Source.Networking.Server;

public interface IUiMessageVisitor
{
    void Visit(NpcInteractionMenuMessage message);

    void Visit(MerchantMenuMessage message);
}