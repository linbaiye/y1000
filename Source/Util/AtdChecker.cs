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


    private static void Print(AtdAction atdAction)
    {
        var descriptors = atdAction.FrameDescriptors;
        Logger.Debug("State {3}, frame time {0}, number {1}, total time {2}, start {4}, end {5}.", atdAction.FrameTime, atdAction.Frame, atdAction.ActionTime, atdAction.Action, descriptors[0].Number, 
            descriptors[atdAction.Frame - 1].Number);
    }

    public static void Dump()
    {
        var atd = FilesystemAtdRepository.Instance.LoadByName("29");
        Print(atd.FindFirst(CreatureState.WALK));
        Print(atd.FindFirst(CreatureState.IDLE));
        Print(atd.FindFirst(CreatureState.FROZEN));
    }
}