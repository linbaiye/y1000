using Godot;
using NLog;
using y1000.code;
using y1000.code.networking.message;
using y1000.Source.Input;

namespace y1000.Source.Character.State.Prediction
{
    public class TurnPrediction : AbstractPositionPrediction<TurnMessage>
    {
        private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
        
        public TurnPrediction(IInput input, Vector2I currentCoordinate, Direction direction) : this(input, currentCoordinate, direction, false)
        {
        }

        public TurnPrediction(IInput input, Vector2I currentCoordinate, Direction direction, bool c) : base(input, currentCoordinate, direction, c)
        {
        }

        public static TurnPrediction Overflow(IInput input, Vector2I currentCoordinate, Direction direction)
        {
            return new TurnPrediction(input, currentCoordinate, direction, true);
        }

        protected override ILogger Logger => LOGGER;
    }
}