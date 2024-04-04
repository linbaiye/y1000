using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character.state;
using y1000.code.player;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;

namespace y1000.Source.Character.State
{
    public interface ICharacterState
    {
        IClientEvent OnMouseRightClicked(Character character, MouseRightClick rightClick);

        IPrediction Predict(Character character, MouseRightClick rightClick);

        IClientEvent OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease);
        
        IPrediction Predict(Character character, MouseRightRelease rightClick);

        IClientEvent OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion);
        
        IPrediction Predict(Character character, RightMousePressedMotion mousePressedMotion);

        void Process(Character character, long deltaMillis);

        OffsetTexture BodyOffsetTexture(Character character);

        bool CanHandle(IInput input);

    }
}