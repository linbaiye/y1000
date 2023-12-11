using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using y1000.code.character.state;
using y1000.code.networking.message;
using y1000.code.util;

namespace y1000.code.character
{
    public class StateSnapshotManager
    {
        private long ackedSequence;

        private readonly IDictionary<long, IStateSnapshot> inputPredictions ;


        public StateSnapshotManager()
        {
            ackedSequence = 0;
            inputPredictions = new Dictionary<long, IStateSnapshot>();
        }

        public void SaveState(IInput input, IStateSnapshot predicted)
        {
            if (ackedSequence >= input.Sequence)
            {
                return;
            }
            if (!inputPredictions.ContainsKey(input.Sequence))
            {
                inputPredictions.Add(input.Sequence, predicted);
            }
        }


        public bool TryAck(IUpdateCharacterStateMessage message)
        {
            if (message.Sequence <=  ackedSequence)
            {
                return true;
            }
            if (inputPredictions.TryGetValue(message.Sequence, out IStateSnapshot? stateSnapshot))
            {
                inputPredictions.Remove(message.Sequence);
                ackedSequence = message.Sequence;
                var ret = stateSnapshot.Match(message);
                if (!ret)
                {
                    LOG.Debug("Snapshot: " + stateSnapshot + ", message " + message);
                }
                return ret;
            }
            return false;
        }

        public void Reset(long newStart)
        {
            foreach (long v in inputPredictions.Keys)
            {
                if (v < newStart)
                {
                    inputPredictions.Remove(v);
                }
            }
            ackedSequence = newStart;
        }
        

        public void Replace(IInput original, IInput input, IStateSnapshot newSnapshot)
        {
            inputPredictions.Remove(original.Sequence);
            inputPredictions.Add(input.Sequence, newSnapshot);
        }
    }
}