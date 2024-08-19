using System.Collections.Generic;

namespace y1000.Source.Animation;

/// <summary>
/// Each AtdStruct represents 8 directions of animations for a given state/action.
/// </summary>
public class AtdAction
{
    public AtdAction(string action, string direction, int frame, int frameTime, List<AtdFrameDescriptor> frameDescriptors)
    {
        Action = action;
        Direction = direction;
        Frame = frame;
        FrameTime = frameTime;
        FrameDescriptors = frameDescriptors;
    }

    public string Action { get; }
    public string Direction { get; }
    public int Frame { get; }
    public int FrameTime { get; }
    public List<AtdFrameDescriptor> FrameDescriptors { get; }

    public int ActionTime => FrameTime * Frame;

    public override string ToString()
    {
        return
            $"{nameof(Action)}: {Action}, {nameof(Direction)}: {Direction}, {nameof(Frame)}: {Frame}, {nameof(FrameTime)}: {FrameTime}";
    }
}