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
        void OnMouseRightClicked(Character character, MouseRightClick rightClick);

        void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease);
        
        void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion);
        
        void Process(Character character, long deltaMillis);

        OffsetTexture BodyOffsetTexture(Character character);

        bool CanHandle(IInput input);

    }
}