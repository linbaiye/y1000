using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using NLog;
using NLog.Fluent;
using y1000.code.networking.message;

namespace y1000.code.character.state.Prediction
{
    public abstract class AbstractPositionPrediction<T> : AbstractPrediction where T : AbstractPositionMessage
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly Vector2I _currentCoordinate;

        private readonly Direction _direction;

        protected AbstractPositionPrediction(IInput input, Vector2I currentCoordinate, Direction direction) : base(input)
        {
            _currentCoordinate = currentCoordinate;
            _direction = direction;
        }

        public override bool SyncWith(InputResponseMessage message)
        {
            if (message.Sequence != Input.Sequence)
            {
                logger.Debug("Sequences are not equal, predict {0}, response {1}.", Input.Sequence, message.Sequence);
                return false;
            }
            if (message.PositionMessage is not T t)
            {
                logger.Debug("Not expected type");
                return false;
            }
            var ret = t.Direction == _direction && _currentCoordinate.Equals(t.Coordinate);
            if (!ret)
            {
                logger.Debug("Current direction {0}, coor {1}, seq {2}, message {3}.", _direction, _currentCoordinate, Input.Sequence,
                 message);
            }
            return ret;
        }

    }
}