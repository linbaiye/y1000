using System.Collections.Generic;
using System.Linq;
using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Networking.Server;

public class MerchantMenuMessage : AbstractNpcInteractionMessage
{
    private MerchantMenuMessage(long id, string name, Texture2D avatar, string text, List<MerchantItem> items, bool sell) : base(id, name, avatar, text)
    {
        Sell = sell;
        Items = items;
    }
    
    public List<MerchantItem> Items { get; }
    
    public bool Sell { get; }

    public override void Accept(IUiMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static MerchantMenuMessage Parse(MerchantMenuPacket packet, ISpriteRepository repository)
    {
        var offsetTexture = repository.LoadNpcWithPrefix(packet.Shape).Get(packet.AvatarIdx);
        var list = packet.Items.Select(i => new MerchantItem(i.Name, i.Icon, i.Color, i.Price, i.CanStack)).ToList();
        return new MerchantMenuMessage(packet.Id, packet.ViewName, offsetTexture.Texture, packet.Text, list, packet.Sell);
    }
}