using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Godot;
using NLog;
using y1000.code;
using y1000.code.networking.message;
using y1000.code.world;
using y1000.Source.Character.Event;
using y1000.Source.Character.State;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.Magic.Foot;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Player;
using ICharacterState = y1000.Source.Character.State.ICharacterState;
namespace y1000.Source.Character
{
	public partial class Character : Node2D, IPlayer
	{
		private ICharacterState _state = EmptyState.Instance;

		private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

		public IFootMagic? FootMagic { get; set; }

		public event EventHandler<AbstractCharacterEventArgs>? WhenCharacterUpdated;

		
		public void ChangeState(ICharacterState state)
		{
			WrappedPlayer().ChangeState(state.WrappedState);
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

		 public Vector2I Coordinate => WrappedPlayer().Coordinate;

		public bool CanHandle(IPredictableInput input)
		{
			return _state.CanHandle(input);
		}


		public void Rewind(AbstractPositionMessage positionMessage)
		{
			var player = WrappedPlayer();
			player.SetPosition(positionMessage);
			LOGGER.Debug("Rewind to coordinate {0}, direction {1}.", player.Coordinate, player.Direction);
			ChangeState(CharacterIdleState.Create(IsMale));
		}

		public void EmitMovementEvent(IPrediction prediction, IClientEvent movementEvent)
		{
			WhenCharacterUpdated?.Invoke(this, new CharacterStateEventArgs(prediction, movementEvent));
		}

		public bool CanMoveOneUnit(Direction direction)
		{
			return WrappedPlayer().Map.Movable(Coordinate.Move(direction));
		}


		private void OnPlayerAnimationFinished(object? sender, CreatureAnimationDoneEventArgs args)
		{
			_state.OnWrappedPlayerAnimationFinished(this);
		}

		public void HandleInput(IPredictableInput input)
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

		public static Character LoggedIn(LoginMessage message, IMap map)
		{
			PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/character.tscn");
			var character = scene.Instantiate<Character>();
			var state = PlayerIdleState.StartFrom(message.Male, 0);
			var player = character.WrappedPlayer();
			player.Init(message.Male, PlayerIdleState.StartFrom(message.Male, 0),  Direction.DOWN, message.Coordinate, message.Id, map);
			player.StateAnimationEventHandler += character.OnPlayerAnimationFinished;
			character.FootMagic = IFootMagic.ByName(UnnamedFootMagic.Name, 85.10f);
			character.ChangeState(CharacterIdleState.Wrap(state));
			return character;
		}

	}
}
