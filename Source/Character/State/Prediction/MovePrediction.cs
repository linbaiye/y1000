using Godot;
using NLog;
using y1000.code;
using y1000.code.networking.message;
using y1000.Source.Input;

namespace y1000.Source.Character.State.Prediction
{
    public class MovePrediction : AbstractPositionPrediction<MoveMessage>
    {
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
        public MovePrediction(IInput input, Vector2I currentCoordinate, Direction direction) : this(input, currentCoordinate, direction, false)
        {
        }

        public MovePrediction(IInput input, Vector2I currentCoordinate, Direction direction, bool clearPrevious) : base(input, currentCoordinate, direction, clearPrevious)
        {

        }

        public static MovePrediction Overflow(IInput input, Vector2I currentCoordinate, Direction direction)
        {
            return new MovePrediction(input, currentCoordinate, direction, true);
        }

        protected override ILogger Logger => LOGGER;
    }
}