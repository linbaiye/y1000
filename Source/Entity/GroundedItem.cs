using Godot;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Event;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.Entity;

public partial class GroundedItem : AbstractEntity, IBody, IEntity
{
	private OffsetTexture _texture;

	private void Init(long id, Vector2I coordinate, string entityName, int number, OffsetTexture bodyOffsetTexture)
	{
		Init(id, coordinate, number > 0 ? entityName + ":" + number : entityName);
		_texture = bodyOffsetTexture;
		Position = coordinate.ToPosition();
	}
	
	private EventMediator? EventMediator { get; set; }

	protected override void MyEvent(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton button && (button.DoubleClick || button.IsPressed()) &&
		    (button.ButtonMask & MouseButtonMask.Left) != 0)
			EventMediator?.NotifyServer(new PickItemEvent((int)Id));
	}

	public override void _Ready()
	{
		base._Ready();
		var bodySprite = GetNode<BodySprite>("Body");
		bodySprite.Label.Position = BodyOffsetTexture.Offset;
	}

	public void Handle(IEntityMessage message)
	{
		if (message is RemoveEntityMessage)
		{
			QueueFree();
		}
	}

	public override OffsetTexture BodyOffsetTexture => _texture;
}
