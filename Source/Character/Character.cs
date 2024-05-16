using System;
using System.Collections.Generic;
using Godot;
using Godot.NativeInterop;
using NLog;
using y1000.Source.Character.Event;
using y1000.Source.Character.State;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.KungFu.Attack;
using y1000.Source.KungFu.Foot;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Player;
using y1000.Source.Util;
using ICharacterState = y1000.Source.Character.State.ICharacterState;

namespace y1000.Source.Character
{
	public partial class Character : Node2D, IPlayer, IServerMessageVisitor
	{
		private ICharacterState _state = EmptyState.Instance;

		private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

		public IFootKungFu? FootMagic { get; private set; }
		
		public IAttackKungFu? AttackKungFu { get; private set; }

		public event EventHandler<CharacterEventArgs>? WhenCharacterUpdated;

		private readonly Queue<IPredictableInput> _inputs = new();

		
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


		private void Rewind(MoveEventResponse response)
		{
			var player = WrappedPlayer();
			player.SetPosition(response.PositionMessage);
			LOGGER.Debug("Rewind to coordinate {0}, direction {1}.", player.Coordinate, player.Direction);
			ChangeState(CharacterIdleState.Idle());
		}
		
		public void Rewind(IPredictableResponse response)
		{
			if (response is MoveEventResponse moveEventResponse)
			{
				Rewind(moveEventResponse);
			}
		}

		public void EmitEvent(IPrediction prediction, IClientEvent movementEvent)
		{
			WhenCharacterUpdated?.Invoke(this, new CharacterEventArgs(prediction, movementEvent));
		}

		public bool CanMoveOneUnit(Direction direction)
		{
			return WrappedPlayer().Map.Movable(Coordinate.Move(direction));
		}


		private void OnPlayerAnimationFinished(object? sender, CreatureAnimationDoneEventArgs args)
		{
			_state.OnWrappedPlayerAnimationFinished(this);
		}


		public void Visit(RewindMessage rewindMessage)
		{
			WrappedPlayer().SetPosition(rewindMessage);
			//_state = 
		}

		public void Handle(IEntityMessage message)
		{
			message.Accept(this);
		}

		public void Visit(PlayerAttackMessage message)
		{
			Direction = message.Direction;
			ChangeState(CharacterAttackState.FromMessage(message));
		}

		public void Visit(HurtMessage hurtMessage)
		{
			ChangeState(CharacterHurtState.Hurt(_state));
		}

		public override void _PhysicsProcess(double delta)
		{
			if (_inputs.Count <= 0 || !_state.AcceptInput())
			{
				return;
			}
			while (_inputs.TryDequeue(out var result))
			{
				if (_state.IsValid(result))
				{
					DispatchInput(result);
				}
			}
		}

		private void DispatchInput(IPredictableInput input)
		{
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
				case InputType.ATTACK:
					_state.Attack(this, (AttackInput)input);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void EnqueueInput(IPredictableInput input)
		{
			_inputs.Enqueue(input);
		}

		public static Character LoggedIn(JoinedRealmMessage message, IMap map)
		{
			PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/character.tscn");
			var character = scene.Instantiate<Character>();
			var state = IPlayerState.Idle();
			var player = character.WrappedPlayer();
			player.Init(message.Male, state, Direction.DOWN, message.Coordinate, message.Id, map);
			player.StateAnimationEventHandler += character.OnPlayerAnimationFinished;
			character.FootMagic = message.FootKungFu;
			character.AttackKungFu = message.AttackKungFu;
			character.ChangeState(CharacterIdleState.Wrap(state));
			return character;
		}
	}
}
