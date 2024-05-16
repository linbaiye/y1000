using Godot;
using NLog;
using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character.State.Prediction
{
    public class MovePrediction : AbstractPositionPrediction<AbstractPositionMessage>
    {
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

        public MovePrediction(IPredictableInput input, Vector2I currentCoordinate, Direction direction, bool clearPrevious = false) : base(input, currentCoordinate, direction, clearPrevious)
        {

        }

        protected override ILogger Logger => LOGGER;
    }
}