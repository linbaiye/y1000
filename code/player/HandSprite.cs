namespace y1000.code.player;

public partial class HandSprite : AbstractEquipmentSprite
{
    protected override OffsetTexture? GetOffsetTexture(IPlayer p)
    {
        return p.WeaponTexture;
    }
}