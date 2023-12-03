using System.Collections.Generic;
using y1000.code.character.state;
using y1000.code.networking.message;

namespace y1000.code.character
{
    public class StateSnapshotManager
    {
        private readonly Queue<IInput> inputs;

        private readonly Queue<IStateSnapshot> predictedStates;

        private const int MAX_SIZE = 1000;

        private long startSequence;

        public StateSnapshotManager()
        {
            inputs = new();
            predictedStates = new();
            startSequence = 0;
        }

        public void EnqueueState(IInput input, IStateSnapshot predicted)
        {
            inputs.Enqueue(input);
            predictedStates.Enqueue(predicted);
        }

        public bool DequeueAndMatchState(IUpdateCharacterStateMessage message)
        {
            if (message.Sequence < startSequence)
            {
                // Ignore delayed messages.
                return true;
            }
            while (true)
            {
                if (!inputs.TryDequeue(out IInput? input))
                {
                    return false;
                }
                if (!predictedStates.TryDequeue(out IStateSnapshot? result))
                {
                    throw new System.Exception("Mismatched queue size.");
                }
                if (input.Sequence == message.Sequence)
                {
                    return result.Match(message);
                }
            }
        }

        public void Reset(long newStart)
        {
            startSequence = newStart;
        }
    }
}