using Godot;
using NLog;
using y1000.code;
using y1000.code.networking.message;
using y1000.Source.Input;

namespace y1000.Source.Character.State.Prediction
{
    public class SetPositionPrediction : AbstractPositionPrediction<SetPositionMessage>
    {
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
        
        public SetPositionPrediction(IInput input, Vector2I currentCoordinate, Direction direction) : this(input, currentCoordinate, direction, false)
        {
        }

        public SetPositionPrediction(IInput input, Vector2I currentCoordinate, Direction direction, bool clear) : base(input, currentCoordinate, direction, clear)
        {
        }

        public static SetPositionPrediction Overflow(IInput input, Vector2I currentCoordinate, Direction direction)
        {
            return new SetPositionPrediction(input, currentCoordinate, direction, true);
        }

        protected override ILogger Logger => LOGGER;
    }
}