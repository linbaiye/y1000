using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.Source.Input
{
    public class DoubleClickInput : AbstractPredictableInput
    {
        public DoubleClickInput(long s) : base(s)
        {
        }

        public override InputType Type => InputType.ATTACK;
    }
}