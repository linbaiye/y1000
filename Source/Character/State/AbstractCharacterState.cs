using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character.state;
using y1000.code.creatures;
using y1000.code.player;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State
{
    public abstract class AbstractCharacterState 
    {

        protected AbstractCharacterState(SpriteManager spriteManager)
        {
        }
    }
}