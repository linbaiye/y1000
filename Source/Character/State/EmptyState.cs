using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character.state;
using y1000.code.character.state.input;
using y1000.code.character.state.Prediction;
using y1000.code.player;

namespace y1000.Source.Character.State
{
    public class EmptyState : ICharacterState
    {
        public static readonly EmptyState Instance = new EmptyState();

        public OffsetTexture BodyOffsetTexture(Character character)
        {
            throw new NotImplementedException();
        }

        public void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            throw new NotImplementedException();
        }

        public void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease)
        {
            throw new NotImplementedException();
        }

        public IPrediction Predict(Character character, MouseRightClick rightClick)
        {
            throw new NotImplementedException();
        }

        public IPrediction Predict(Character character, MouseRightRelease rightClick)
        {
            throw new NotImplementedException();
        }

        public void Process(Character character, double delta)
        {
            throw new NotImplementedException();
        }

        public void Process(Character character, long deltaMillis)
        {
            throw new NotImplementedException();
        }

        public bool CanHandle(IInput input)
        {
            throw new NotImplementedException();
        }

        public void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion)
        {
            throw new NotImplementedException();
        }

        public IPrediction Predict(Character character, RightMousePressedMotion mousePressedMotion)
        {
            throw new NotImplementedException();
        }
    }
}