using System.Collections.Generic;
using Godot;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class PlayerBodyAnimation : AbstractPlayerBodyAnimation<PlayerBodyAnimation>
{
    private static readonly Vector2 DEFAULT_OFFSET = new(16, -12);

    private static readonly ISet<CreatureState> PLAYER_STATES = new HashSet<CreatureState>()
    {
        CreatureState.IDLE,

        CreatureState.WALK,

        CreatureState.RUN,
        
        CreatureState.STANDUP,

        CreatureState.HURT,

        CreatureState.DIE,

        CreatureState.ENFIGHT_WALK,

        CreatureState.BOW,

        CreatureState.SIT,
        
        CreatureState.FLY,
        
        CreatureState.COOLDOWN,
        
        CreatureState.HELLO,
        
        CreatureState.FIST,
        
        CreatureState.KICK,
        
        CreatureState.SWORD,
        
        CreatureState.SWORD2H,
        
        CreatureState.BLADE,
        
        CreatureState.BLADE2H,
        
        CreatureState.AXE,
        
        CreatureState.SPEAR,
        
        CreatureState.THROW,
    };

    private PlayerBodyAnimation()
    {
    }


    public static readonly PlayerBodyAnimation Male = ForMale();
    
    public static readonly PlayerBodyAnimation Female = ForFemale();

    private static PlayerBodyAnimation Config(string prefix)
    {
        var playerAnimation = new PlayerBodyAnimation();
        AtzSprite N00 = SpriteRepository.LoadByName(prefix + "00", DEFAULT_OFFSET);
        AtzSprite N01 = SpriteRepository.LoadByName(prefix + "01", DEFAULT_OFFSET);
        AtzSprite N04 = SpriteRepository.LoadByName(prefix + "04", DEFAULT_OFFSET);
        AtzSprite N02 = SpriteRepository.LoadByName(prefix + "02", DEFAULT_OFFSET);
        AtzSprite N03 = SpriteRepository.LoadByName(prefix + "03", DEFAULT_OFFSET);
        return Config(playerAnimation, N00, N01, N02, N03, N04);
    }

    private static PlayerBodyAnimation ForFemale()
    {
        return Config("A");
    }

    private static PlayerBodyAnimation ForMale()
    {
        return Config("N");
    }
}