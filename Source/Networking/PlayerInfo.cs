using Source.Networking.Protobuf;

namespace y1000.Source.Networking;

public class PlayerInfo
{
    public long Id { get; private set; }
    
    public bool Male { get; private set; }

    public string Name { get; private set; } = "";
    
    public string? WeaponName { get; private set; }
    
    public string? ChestName { get; private set; }
    
    public string? HairName { get; private set; }
    
    public string? HatName { get; private set; }
    
    public string? WristName { get; private set; }
    
    public string? BootName { get; private set; }
    
    public string? ClothingName { get; private set; }
    
    public string? TrouserName { get; private set; }

    public int HairColor { get; set; }
    public int HatColor { get; set; }
    public int ChestColor { get; set; }
    public int TrouserColor { get; set; }
    public int ClothingColor { get; set; }
    public int BootColor { get; set; }
    public int WristColor { get; set; }

    public static PlayerInfo FromPacket(PlayerInfoPacket packet)
    {
        return new PlayerInfo()
        {
            Id = packet.Id,
            Male = packet.Male,
            Name = packet.Name,
            WeaponName = packet.HasWeaponName ? packet.WeaponName : null,
            ChestName = packet.HasChestName ? packet.ChestName : null,
            HairName = packet.HasHairName ? packet.HairName : null,
            HatName = packet.HasHatName ? packet.HatName : null,
            WristName = packet.HasWristName ? packet.WristName: null,
            BootName = packet.HasBootName? packet.BootName: null,
            ClothingName= packet.HasClothingName? packet.ClothingName: null,
            TrouserName= packet.HasTrouserName? packet.TrouserName: null,
            HairColor = packet.HairColor,
            HatColor = packet.HatColor,
            ChestColor = packet.ChestColor,
            TrouserColor = packet.TrouserColor,
            ClothingColor = packet.ClothingColor,
            BootColor = packet.BootColor,
            WristColor = packet.WristColor,
        };
    }
    
}