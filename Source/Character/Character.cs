using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Godot;
using y1000.code;
using y1000.code.networking.message;
using y1000.code.world;
using y1000.Source.Character.Event;
using y1000.Source.Character.State;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;
using y1000.Source.Player;
using ICharacterState = y1000.Source.Character.State.ICharacterState;
namespace y1000.Source.Character
{
	public partial class Character : Node2D, IPlayer
	{
		private ICharacterState _state = EmptyState.Instance;

		public IRealm Realm { get; set; } = IRealm.Empty;

		public event EventHandler? OnCharacterUpdated;
		
		public void ChangeState(ICharacterState state)
		{
			_state = state;
		}

		public bool IsMale => WrappedPlayer().IsMale;
		
		public string EntityName => WrappedPlayer().EntityName;
		
		public long Id => WrappedPlayer().Id;

		public Direction Direction
		{
			get => WrappedPlayer().Direction;
			set
			{
				{
					WrappedPlayer().Direction = value;
				}
			}
		}

		public Player.Player WrappedPlayer()
		{
			foreach (var child in GetChildren())
			{
				if (child is Player.Player player)
				{
					return player;
				}
			}
			throw new NotImplementedException();
		}

		 public override void _Process(double delta)
		 {
			 _state.Update(this, delta);
		 }

		 public Vector2I Coordinate => WrappedPlayer().Coordinate;

		public bool CanHandle(IInput input)
		{
			return _state.CanHandle(input);
		}

		public void Rewind(AbstractPositionMessage positionMessage)
		{
			var player = WrappedPlayer();
			player.Handle(positionMessage);
		}

		public void EmitMovementEvent(IPrediction prediction, IClientEvent movementEvent)
		{
			OnCharacterUpdated?.Invoke(this, new CharacterUpdatedEventArgs(prediction, movementEvent));
		}

		public bool CanMoveOneUnit(Direction direction)
		{
			return CanMoveTo(Coordinate.Move(direction));
		}


		public bool CanMoveTo(Vector2I point)
		{
			return Realm.CanMove(point);
		}


		private void OnPlayerEvent(object? sender, EventArgs e)
		{
			
		}

		public void HandleInput(IInput input)
		{
			if (!_state.CanHandle(input))
			{
				return;
			}
			switch (input.Type)
			{
				case InputType.MOUSE_RIGHT_CLICK:
					_state.OnMouseRightClicked(this, (MouseRightClick)input);
					break;
				case InputType.MOUSE_RIGHT_RELEASE:
					_state.OnMouseRightReleased(this, (MouseRightRelease)input);
					break;
				case InputType.MOUSE_RIGHT_MOTION:
					_state.OnMousePressedMotion(this, (RightMousePressedMotion)input);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static Character LoggedIn(LoginMessage message, IRealm realm)
		{
			PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/character.tscn");
			var character = scene.Instantiate<Character>();
			var state = PlayerIdleState.StartFrom(message.Male, 0);
			var player = character.WrappedPlayer();
			player.Init(message.Male, PlayerIdleState.StartFrom(message.Male, 0),  Direction.DOWN, message.Coordinate, message.Id);
			player.PlayerEventHandler += character.OnPlayerEvent;
			character.Realm = realm;
			player.ProcessMode = ProcessModeEnum.Disabled;
			character.ChangeState(CharacterIdleState.Wrap(state));
			return character;
		}

	}
}
