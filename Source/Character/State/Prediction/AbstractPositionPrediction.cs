using Godot;
using NLog;
using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character.State.Prediction
{
    public abstract class AbstractPositionPrediction<T> : AbstractPrediction where T : AbstractPositionMessage
    {

        private readonly Vector2I _currentCoordinate;

        private readonly Direction _direction;
        
        protected abstract ILogger Logger { get; }

        protected AbstractPositionPrediction(IPredictableInput input, Vector2I currentCoordinate, Direction direction, bool clearPrevious) : base(input, clearPrevious)
        {
            _currentCoordinate = currentCoordinate;
            _direction = direction;
        }

        public override bool Predicted(IPredictableResponse response)
        {
            if (response is not MoveEventResponse message)
            {
                return false;
            }
            if (message.Sequence != Input.Sequence)
            {
                Logger.Debug("Sequences are not equal, predict {0}, response {1}.", Input.Sequence, message.Sequence);
                return false;
            }
            if (message.PositionMessage is not T t)
            {
                Logger.Debug("Not expected type {0}.", message.PositionMessage);
                return false;
            }
            if (t.Direction != _direction)
            {
                Logger.Debug("Direction mismatch, client direction {0}, server direction {1}, id {2}.", _direction, t.Direction, Input.Sequence);
                return false;
            }
            if (!_currentCoordinate.Equals(t.Coordinate))
            {
                Logger.Debug("Position mismatch, client coordinate {0}, server coordinate {1}, id {2}.", _currentCoordinate, t.Coordinate, Input.Sequence);
                return false;
            }
            return true;
        }
    }
}