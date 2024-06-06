using System;
using Godot;
using y1000.Source.Creature;
using y1000.Source.Item;
using y1000.Source.KungFu.Attack;
using y1000.Source.Sprite;
using y1000.Source.Util;

namespace y1000.Source.Animation;

public class WeaponEffectAnimation : AbstractPlayerAnimation<WeaponEffectAnimation>
{
    private static readonly MagicSdbReader MAGIC_SDB_READER = MagicSdbReader.Instance;
    public WeaponEffectAnimation(CreatureState state1, CreatureState? state2)
    {
        State1 = state1;
        State2 = state2;
    }

    private CreatureState State1 { get; }
    private CreatureState? State2 { get; }

    private static WeaponEffectAnimation Load(string name, CreatureState state1, CreatureState? state2 = null)
    {
        AtzSprite attack = FilesystemSpriteRepository.Instance.LoadByName(name, new Vector2(16, -12));
        var playerAnimation = new WeaponEffectAnimation(state1, state2);
        playerAnimation.ConfigureState(state1, AtdReader, attack);
        if (state2 != null)
        {
            playerAnimation.ConfigureState(state2.Value, AtdReader, attack);
        }

        return playerAnimation;
    }

    public bool Compatible(CreatureState state)
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
                return State1 == state || State2 == state;
            default:
                return true;
        }
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
    

    public static WeaponEffectAnimation LoadFor(PlayerWeapon weapon)
    {
        return weapon.AttackKungFuType switch
        {
            AttackKungFuType.QUANFA=>  Load("_/_11" + MAGIC_SDB_READER.GetEffect("无名拳法"), CreatureState.FIST, CreatureState.KICK),
            //AttackKungFuType.SWORD => Load("_/_22" + MAGIC_SDB_READER.GetEffect("飞龙剑法"), CreatureState.SWORD, CreatureState.SWORD2H),
            AttackKungFuType.SWORD => Load("_/_12" + MAGIC_SDB_READER.GetEffect("无名剑法"), CreatureState.SWORD, CreatureState.SWORD2H),
            AttackKungFuType.BLADE =>  Load("_/_13" + MAGIC_SDB_READER.GetEffect("无名刀法"), CreatureState.BLADE, CreatureState.BLADE2H),
            AttackKungFuType.AXE =>  Load("_/_14" + MAGIC_SDB_READER.GetEffect("无名槌法"), CreatureState.AXE),
            AttackKungFuType.SPEAR =>  Load("_/_15" + MAGIC_SDB_READER.GetEffect("无名枪术"), CreatureState.SPEAR),
            AttackKungFuType.BOW =>  Load("_/_16" + MAGIC_SDB_READER.GetEffect("无名弓术"), CreatureState.BOW),
            /*AttackKungFuType.AXE =>  Load("_/_14" + MAGIC_SDB_READER.GetEffect("无名槌法"), CreatureState.SWORD, CreatureState.SWORD2H),
            AttackKungFuType.BOW => Load(),
            AttackKungFuType.SPEAR => Load(weapon.NonAttackAnimation, weapon.AttackAnimation, CreatureState.SPEAR),*/
            _ => throw new NotImplementedException()
        };
    }
}