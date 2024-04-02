using y1000.code.networking.message;

namespace y1000.Source.Input
{
    public interface IInput : I2ServerGameMessage
    {
        long Sequence { get; }

        InputType Type { get; }
    }
}