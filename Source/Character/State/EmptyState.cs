using System;
using y1000.Source.Animation;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;
using y1000.Source.Player;
using IPlayer = y1000.Source.Player.IPlayer;

namespace y1000.Source.Character.State
{
    public class EmptyState : ICharacterState
    {
        public static readonly EmptyState Instance = new EmptyState();

        public OffsetTexture BodyOffsetTexture(CharacterImpl character)
        {
            throw new NotImplementedException();
        }

        public void OnMouseRightClicked(CharacterImpl character, MouseRightClick rightClick)
        {
            throw new NotImplementedException();
        }

        public void OnMouseRightReleased(CharacterImpl character, MouseRightRelease mouseRightRelease)
        {
            throw new NotImplementedException();
        }

        public IPrediction Predict(CharacterImpl character, MouseRightClick rightClick)
        {
            throw new NotImplementedException();
        }

        public IPrediction Predict(CharacterImpl character, MouseRightRelease rightClick)
        {
            throw new NotImplementedException();
        }

        public void Process(CharacterImpl character, double delta)
        {
            throw new NotImplementedException();
        }

        public void Process(CharacterImpl character, long deltaMillis)
        {
            throw new NotImplementedException();
        }

        public bool CanHandle(IPredictableInput input)
        {
            throw new NotImplementedException();
        }

        public ICharacterState AfterHurt()
        {
            throw new NotImplementedException();
        }

        public void OnWrappedPlayerStateChanged(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void OnWrappedPlayerStateChanged(IPlayerState newState)
        {
            throw new NotImplementedException();
        }

        public void Update(CharacterImpl character, long delta)
        {
            throw new NotImplementedException();
        }

        public IPlayerState WrappedState => throw new NotImplementedException();

        public void OnMousePressedMotion(CharacterImpl character, RightMousePressedMotion mousePressedMotion)
        {
            throw new NotImplementedException();
        }

        public IPrediction Predict(CharacterImpl character, RightMousePressedMotion mousePressedMotion)
        {
            throw new NotImplementedException();
        }

        public OffsetTexture BodyOffsetTexture(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Update(Player.PlayerImpl player, long delta)
        {
            throw new NotImplementedException();
        }
    }
}