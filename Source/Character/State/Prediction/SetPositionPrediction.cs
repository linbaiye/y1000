using Godot;
using NLog;
using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.Networking;

namespace y1000.Source.Character.State.Prediction
{
    public class SetPositionPrediction : AbstractPositionPrediction<SetPositionMessage>
    {
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
        
        public SetPositionPrediction(IPredictableInput input, Vector2I currentCoordinate, Direction direction) : this(input, currentCoordinate, direction, false)
        {
        }

        public SetPositionPrediction(IPredictableInput input, Vector2I currentCoordinate, Direction direction, bool clear) : base(input, currentCoordinate, direction, clear)
        {
        }

        public static SetPositionPrediction Overflow(IPredictableInput input, Vector2I currentCoordinate, Direction direction)
        {
            return new SetPositionPrediction(input, currentCoordinate, direction, true);
        }

        protected override ILogger Logger => LOGGER;
    }
}