using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character.state.input;
using y1000.code.character.state.Prediction;
using y1000.code.player;

namespace y1000.Source.Character.State
{
    public interface ICharacterState
    {
        void OnMouseRightClicked(Character character, MouseRightClick rightClick);

        IPrediction Predict(Character character, MouseRightClick rightClick);

        void Process(Character character, double delta);

        OffsetTexture BodyOffsetTexture(Character character);
    }
}