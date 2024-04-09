using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character.state;
using y1000.code.creatures;
using y1000.code.player;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;

namespace y1000.Source.Character.State
{
    public abstract class AbstractCharacterState : ICharacterState
    {

        private readonly SpriteManager _spriteManager;

        protected long ElpasedMillis;

        protected AbstractCharacterState(SpriteManager spriteManager)
        {
            _spriteManager = spriteManager;
            ElpasedMillis = 0;
        }

        public OffsetTexture BodyOffsetTexture(Character character)
        {
            return _spriteManager.Texture(character.Direction, ElpasedMillis);
        }

        protected SpriteManager SpriteManager => _spriteManager;

        public abstract void OnMouseRightClicked(Character character, MouseRightClick rightClick);

        public abstract void Process(Character character, long deltaMillis);

        public abstract bool CanHandle(IInput input);

        public abstract void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease);

        public abstract void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion);
    }
}