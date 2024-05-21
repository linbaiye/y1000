using y1000.Source.Creature;
using y1000.Source.Input;

namespace y1000.Source.KungFu.Attack;

public class EmptyAttackKungFu : IAttackKungFu
{
    private EmptyAttackKungFu()
    {
        
    }

    public static readonly EmptyAttackKungFu Instance = new EmptyAttackKungFu();
    public string Name => "";
    public int Level => 0;
    public void Attack(Character.CharacterImpl character, AttackInput input)
    {
        throw new System.NotImplementedException();
    }

    public CreatureState RandomAttackState()
    {
        throw new System.NotImplementedException();
    }
}