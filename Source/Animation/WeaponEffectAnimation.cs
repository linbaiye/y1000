using System;
using Godot;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class WeaponEffectAnimation : AbstractPlayerAnimation<WeaponEffectAnimation>
{
    private WeaponEffectAnimation(CreatureState state1, CreatureState? state2)
    {
        State1 = state1;
        State2 = state2;
    }

    public int EffectId { get; private set; } = 0;

    private CreatureState State1 { get; }
    
    private CreatureState? State2 { get; }

    private static WeaponEffectAnimation Load(string name, CreatureState state1, CreatureState? state2 = null)
    {
        AtzSprite attack = FilesystemSpriteRepository.Instance.LoadByNumberAndOffset(name, new Vector2(16, -12));
        var playerAnimation = new WeaponEffectAnimation(state1, state2);
        playerAnimation.ConfigureState(state1, AtdStructure, attack);
        if (state2 != null)
        {
            playerAnimation.ConfigureState(state2.Value, AtdStructure, attack);
        }
        return playerAnimation;
    }

    
    private bool IsAttacking(CreatureState state)
    {
        switch (state)
        {
            case CreatureState.FIST:
            case CreatureState.KICK:
            case CreatureState.AXE:
            case CreatureState.SPEAR:
            case CreatureState.BLADE:
            case CreatureState.BLADE2H:
            case CreatureState.SWORD:
            case CreatureState.SWORD2H:
            case CreatureState.BOW:
            case CreatureState.THROW:
                return true;
            default:
                return false;
        }
    }

    public OffsetTexture? GetIfAttacking(CreatureState state, Direction direction, int elpased)
    {
        if (!IsAttacking(state))
        {
            return null;
        }
        return OffsetTexture(state, direction, elpased);
    }


    private static WeaponEffectAnimation Create(CreatureState state, int id)
    {
        return state switch 
        {
            CreatureState.FIST => Load("_/_" + id, CreatureState.FIST, CreatureState.KICK),
            CreatureState.KICK => Load("_/_" + id, CreatureState.FIST, CreatureState.KICK),
            CreatureState.SWORD => Load("_/_" + id, CreatureState.SWORD, CreatureState.SWORD2H),
            CreatureState.SWORD2H => Load("_/_" + id, CreatureState.SWORD, CreatureState.SWORD2H),
            CreatureState.BLADE => Load("_/_" + id, CreatureState.BLADE, CreatureState.BLADE2H),
            CreatureState.BLADE2H => Load("_/_" + id, CreatureState.BLADE, CreatureState.BLADE2H),
            CreatureState.AXE => Load("_/_" + id, CreatureState.AXE),
            CreatureState.SPEAR => Load("_/_" + id, CreatureState.SPEAR),
            CreatureState.BOW => Load("_/_" + id, CreatureState.BOW),
            CreatureState.THROW => Load("_/_" + id, CreatureState.THROW),
            _ => throw new NotImplementedException()
        };
    }


    public static WeaponEffectAnimation LoadFor(CreatureState state, int id)
    {
        var ani = Create(state, id);
        ani.EffectId = id;
        return ani;
    }
}