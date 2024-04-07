using Source.Networking.Protobuf;
using y1000.code.networking.message;

namespace y1000.Source.Input
{
    public interface IInput
    {
        long Sequence { get; }

        InputType Type { get; }

        InputPacket ToPacket();
    }
}