
using System;
using System.Collections.Generic;
using y1000.Source.Character.Event;
using y1000.Source.Character.State;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;
using y1000.Source.Util;

namespace y1000.Source.KungFu.Attack;

public class QuangFa : AbstractLevelKungFu, IAttackKungFu
{

    private static readonly Random RANDOM = new();
    
    public QuangFa(float level, string name) : base(level)
    {
        Name = name;
    }

    public override string Name { get; }

    private static readonly ISet<string> NAMES = new HashSet<string>()
    {
        "无名拳法",
    };
    
    
    public static bool Knows(string name)
    {
        return NAMES.Contains(name);
    }

    private bool UseBlow50()
    {
        return Level < 50 || RANDOM.Next() % 2 == 1;
    }


    public void Attack(Character.Character character, AttackInput input)
    {
        bool below50 = UseBlow50();
        character.Direction = character.Coordinate.GetDirection(input.Entity.Coordinate);
        character.EmitEvent(new AttackPrediction(input), new AttackEntityEvent(input, below50));
        var characterAttackState = CharacterAttackState.Quanfa(character.IsMale, input.Entity, below50);
        character.ChangeState(characterAttackState);
    }
}