using System.Collections.Generic;
using y1000.Source.Map;
using y1000.Source.Networking;

namespace y1000.Source.Creature.Monster;

public partial class Merchant : Monster
{
    public List<string> SellItems { get; set; } = new List<string>();

    public List<string> BuyItems { get; set; } = new List<string>();


    public int AvatarSpriteNumber { get; private set; } = 0;
    
    public void InitializeMerchant(
        NpcInterpolation npcInterpolation, IMap map,
        List<string> sell, List<string> buy, int avNumber)
    {
        Initialize(this, npcInterpolation, map);
        SellItems = sell;
        BuyItems = buy;
        AvatarSpriteNumber = avNumber;
    }
}