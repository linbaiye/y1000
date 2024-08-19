
namespace y1000.Source.Input
{
    public abstract class AbstractPredictableInput : IPredictableInput
    {
        protected AbstractPredictableInput(long s)
        {
            Sequence = s;
        }

        public long Sequence { get; }
        
        public abstract InputType Type { get; }
    }
}