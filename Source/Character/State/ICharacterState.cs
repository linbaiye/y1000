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
    public interface ICharacterState
    {
        void OnMouseRightClicked(Character character, MouseRightClick rightClick);

        IPrediction Predict(Character character, MouseRightClick rightClick);

        void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease);
        
        IPrediction Predict(Character character, MouseRightRelease rightClick);

        void Process(Character character, long deltaMillis);

        OffsetTexture BodyOffsetTexture(Character character);

        bool RespondsTo(IInput input);

    }
}