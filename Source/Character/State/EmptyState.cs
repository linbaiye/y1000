using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character.state.input;
using y1000.code.character.state.Prediction;

namespace y1000.Source.Character.State
{
    public class EmptyState : ICharacterState
    {
        public static readonly EmptyState Instance = new EmptyState();
        public void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            throw new NotImplementedException();
        }

        public IPrediction Predict(Character character, MouseRightClick rightClick)
        {
            throw new NotImplementedException();
        }

        public void Process(Character character, double delta)
        {
            throw new NotImplementedException();
        }
    }
}