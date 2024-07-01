using System;
using Godot;
using y1000.Source.Creature;
using y1000.Source.Item;
using y1000.Source.KungFu.Attack;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class PlayerWeaponAnimation: AbstractPlayerAnimation<PlayerWeaponAnimation>
{
    public PlayerWeaponAnimation(CreatureState state1, CreatureState? state2)
    {
        State1 = state1;
        State2 = state2;
    }

    private CreatureState State1 { get; }
    private CreatureState? State2 { get; }

    private PlayerWeaponAnimation ConfigureNoneAttack(AtzSprite nonattack)
    {
        return ConfigureState(CreatureState.FLY, AtdStructure, nonattack)
            .ConfigureState(CreatureState.WALK, AtdStructure, nonattack)
            .ConfigureState(CreatureState.RUN, AtdStructure, nonattack)
            .ConfigureState(CreatureState.ENFIGHT_WALK, AtdStructure, nonattack)
            .ConfigureState(CreatureState.HURT, AtdStructure, nonattack)
            .ConfigureState(CreatureState.COOLDOWN, AtdStructure, nonattack)
            .ConfigureState(CreatureState.IDLE, AtdStructure, nonattack)
            .ConfigureState(CreatureState.SIT, AtdStructure, nonattack)
            .ConfigureState(CreatureState.STANDUP, AtdStructure, nonattack)
            .ConfigureState(CreatureState.DIE, AtdStructure, nonattack)
            ;
    }

    private static PlayerWeaponAnimation Load(string name, string attackName, CreatureState state1, CreatureState? state2 = null)
    {
        AtzSprite nonattack = FilesystemSpriteRepository.Instance.LoadByNumberAndOffset(name, new Vector2(16, -12));
        //AtzSprite attack = AtzSprite.LoadOffsetWeaponSprites(attackName);
        AtzSprite attack = FilesystemSpriteRepository.Instance.LoadByNumberAndOffset(attackName, new Vector2(16, -12));
        var playerAnimation = new PlayerWeaponAnimation(state1, state2);
        playerAnimation.ConfigureNoneAttack(nonattack)
                .ConfigureState(state1, AtdStructure, attack);
        if (state2 != null)
        {
            playerAnimation.ConfigureState(state2.Value, AtdStructure, attack);
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
    
    
    public static PlayerWeaponAnimation LoadSword(string name, string attackName)
    {
        return Load(name, attackName, CreatureState.SWORD, CreatureState.SWORD2H);
    }

    public static PlayerWeaponAnimation LoadBow(string name, string attackName)
    {
        return Load(name, attackName, CreatureState.BOW);
    }

    public static PlayerWeaponAnimation LoadBlade(string name, string attackName)
    {
        return Load(name, attackName, CreatureState.BLADE, CreatureState.BLADE2H);
    }
    

    public static PlayerWeaponAnimation LoadFor(PlayerWeapon weapon)
    {
        return weapon.AttackKungFuType switch
        {
            AttackKungFuType.SWORD => LoadSword(weapon.NonAttackAnimation, weapon.AttackAnimation),
            AttackKungFuType.BLADE => LoadBlade(weapon.NonAttackAnimation, weapon.AttackAnimation),
            AttackKungFuType.BOW => LoadBow(weapon.NonAttackAnimation, weapon.AttackAnimation),
            AttackKungFuType.AXE => Load(weapon.NonAttackAnimation, weapon.AttackAnimation, CreatureState.AXE),
            AttackKungFuType.SPEAR => Load(weapon.NonAttackAnimation, weapon.AttackAnimation, CreatureState.SPEAR),
            AttackKungFuType.QUANFA => Load(weapon.NonAttackAnimation, weapon.AttackAnimation, CreatureState.FIST, CreatureState.KICK),
            _ => throw new NotImplementedException()
        };
    }
        
}