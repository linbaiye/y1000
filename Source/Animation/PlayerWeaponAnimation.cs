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
        return ConfigureState(CreatureState.FLY, AtdReader, nonattack)
            .ConfigureState(CreatureState.WALK, AtdReader, nonattack)
            .ConfigureState(CreatureState.RUN, AtdReader, nonattack)
            .ConfigureState(CreatureState.ENFIGHT_WALK, AtdReader, nonattack)
            .ConfigureState(CreatureState.HURT, AtdReader, nonattack)
            .ConfigureState(CreatureState.COOLDOWN, AtdReader, nonattack)
            .ConfigureState(CreatureState.IDLE, AtdReader, nonattack)
            .ConfigureState(CreatureState.SIT, AtdReader, nonattack)
            .ConfigureState(CreatureState.STANDUP, AtdReader, nonattack)
            .ConfigureState(CreatureState.DIE, AtdReader, nonattack)
            ;
    }

    private static PlayerWeaponAnimation Load(string name, string attackName, CreatureState state1, CreatureState? state2 = null)
    {
        AtzSprite nonattack = FilesystemSpriteRepository.Instance.LoadByName(name, new Vector2(16, -12));
        //AtzSprite attack = AtzSprite.LoadOffsetWeaponSprites(attackName);
        AtzSprite attack = FilesystemSpriteRepository.Instance.LoadByName(attackName, new Vector2(16, -12));
        var playerAnimation = new PlayerWeaponAnimation(state1, state2);
        playerAnimation.ConfigureNoneAttack(nonattack)
                .ConfigureState(state1, AtdReader, attack);
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