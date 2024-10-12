using System.Collections.Generic;
using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Sprite;

namespace y1000.Source.Networking.Server;

public class NpcInteractionMenuMessage : AbstractNpcInteractionMessage
{
    private NpcInteractionMenuMessage(long id,
        string name,
        Texture2D avatar,
        IEnumerable<string> menuItems, string text) : base(id, name, avatar, text)
    {
        MenuItems = new List<string>(menuItems);
    }

    public List<string> MenuItems { get; }
    
    public override void Accept(IUiMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static NpcInteractionMenuMessage Parse(NpcInteractionMenuPacket packet,
        ISpriteRepository spriteRepository)
    {
         var atz = spriteRepository.LoadNpcWithPrefix(packet.Shape);
        return new NpcInteractionMenuMessage(packet.Id, packet.ViewName, atz.Get(packet.AvatarIdx).Texture, packet.Interactions, packet.Text);
    }
}