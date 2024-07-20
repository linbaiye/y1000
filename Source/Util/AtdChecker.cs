using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Util;

public class AtdChecker
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private static void CheckState(string name, CreatureState state, AtdStructure atdStructure)
    {
        if (!atdStructure.HasState(state))
        {
            return;
        }
        var atdActions = atdStructure.Find(state);
        foreach (var atdAction in atdActions)
        {
            if (atdAction.Frame != atdAction.FrameDescriptors.Count)
            {
                Logger.Error("{0} has error, state {1}, frame count {2}, list length {3}.", name, state, atdAction.Frame,atdAction.FrameDescriptors.Count);
            }
        }
    }

    public static void Check()
    {
        for (int i = 1; i <= 139; i++)
        {
            var atd = FilesystemAtdRepository.Instance.LoadByName(i.ToString());
            CheckState(i.ToString(), CreatureState.WALK, atd);
            CheckState(i.ToString(), CreatureState.DIE, atd);
            CheckState(i.ToString(), CreatureState.IDLE, atd);
            CheckState(i.ToString(), CreatureState.HURT, atd);
            CheckState(i.ToString(), CreatureState.ATTACK, atd);
        }
        
    }
}