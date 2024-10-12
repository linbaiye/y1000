using System.Collections.Generic;
using System.Linq;
using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature.Monster;
using y1000.Source.Sprite;

namespace y1000.Source.Networking.Server;

public class MerchantMenuMessage : AbstractNpcInteractionMessage
{
    private MerchantMenuMessage(long id, string name, Texture2D avatar, string text, List<Merchant.Item> items, bool sell) : base(id, name, avatar, text)
    {
        Sell = sell;
        Items = items;
    }
    
    public List<Merchant.Item> Items { get; }
    
    public bool Sell { get; }

    public override void Accept(IUiMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static MerchantMenuMessage Parse(MerchantMenuPacket packet, ISpriteRepository repository)
    {
        var offsetTexture = repository.LoadNpcWithPrefix(packet.Shape).Get(packet.AvatarIdx);
        var list = packet.Items.Select(i => new Merchant.Item(i.Name, i.Price, i.Icon, i.Color)).ToList();
        return new MerchantMenuMessage(packet.Id, packet.ViewName, offsetTexture.Texture, packet.Text, list, packet.Sell);
    }
}