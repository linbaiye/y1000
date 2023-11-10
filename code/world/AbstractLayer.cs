using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code;
using y1000.code.world;

public abstract partial class AbstractLayer : Node2D
{
    protected void Layout(string layer)
    {
        var gameMap = GameMap.Load("res://assets/maps/prison/prison.map");
        gameMap?.ForeachCell((cell, x, y) => Layout("object".Equals(layer) ? cell.ObjectId : cell.RoofId, x, y, layer));
    }

    private void Layout(int objectId, int x, int y, string layer)
    {
		var dirpath = "res://assets/maps/prison/" + layer + "/" + objectId;
		var imagePath = dirpath + "/image.png";
		if (!Godot.FileAccess.FileExists(imagePath))
		{
			return;
		}
		if (ResourceLoader.Load(imagePath) is not Texture2D texture)
		{
			return;
		}
		using (var fileAccess = Godot.FileAccess.Open(dirpath + "/struct.json", Godot.FileAccess.ModeFlags.Read))
		{
			var jsonString = fileAccess.GetAsText();
			Object2Json json = Object2Json.FromJsonString(jsonString);
			int xPos = x * VectorUtil.TILE_SIZE_X;
			int yPos = y * VectorUtil.TILE_SIZE_Y;
			Sprite2D objectSprite = new Sprite2D() { Texture = texture, Centered = false, Position = new Vector2(xPos, yPos), Offset = new Vector2(json.X, json.Y), YSortEnabled = true};
			AddChild(objectSprite);
		}
	}
}