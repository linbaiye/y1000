using Source.Networking.Protobuf;

namespace y1000.Source.Input
{
    public interface IPredictableInput : IInput
    {
        long Sequence { get; }
    }
}