using Godot;
using NLog;
using y1000.code;
using y1000.code.networking.message;
using y1000.Source.Input;

namespace y1000.Source.Character.State.Prediction
{
    public class MovePrediction : AbstractPositionPrediction<AbstractPositionMessage>
    {
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

        public MovePrediction(IInput input, Vector2I currentCoordinate, Direction direction, bool clearPrevious = false) : base(input, currentCoordinate, direction, clearPrevious)
        {

        }

        protected override ILogger Logger => LOGGER;
    }
}