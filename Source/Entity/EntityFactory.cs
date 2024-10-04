using System;
using System.Collections.Generic;
using Godot;
using y1000.Source.Animation;
using y1000.Source.Creature.Monster;
using y1000.Source.DynamicObject;
using y1000.Source.Event;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Sprite;
using y1000.Source.Util;

namespace y1000.Source.Entity;

public class EntityFactory
{

    private readonly ItemSdbReader _itemDb;
    
    private readonly IconReader _itemIconReader;

    private readonly EventMediator _eventMediator;

    private readonly ISpriteRepository _spriteRepository;
    
    public EntityFactory(EventMediator eventMediator, ISpriteRepository spriteRepository)
    {
        _itemDb = ItemSdbReader.ItemSdb;
        _itemIconReader = IconReader.ItemIconReader;
        _eventMediator = eventMediator;
        _spriteRepository = spriteRepository;
    }


    public GroundItem CreateOnGroundItem(ShowItemMessage message) 
    {
        var iconId = _itemDb.GetIconId(message.Name);
        var texture2D = _itemIconReader.Get(iconId);
        if (texture2D == null)
        {
            throw new NotImplementedException(message.Name + " does not have icon.");
        }
        return GroundItem.Create(message, texture2D, _eventMediator);
    }
    
    

    private Merchant CreateMerchant(NpcInterpolation interpolation, IMap map)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Merchant.tscn");
        var merchant = scene.Instantiate<Merchant>();
        //ISet<string>[] items = LoadItems("lbn");
        var fileAccess = FileAccess.Open("res://assets/sdb/npc/" + interpolation.TextFile, FileAccess.ModeFlags.Read);
        string? line;
        List<Merchant.Item>[] items = {new(), new() };
        int avNubmer = 0;
        while ((line = fileAccess.GetLine()) != null)
        {
            if (string.IsNullOrEmpty(line))
            {
                break;
            }
            if (line.StartsWith("SELLITEM"))
            {
                var s = line.Split(":")[1];
                items[0].Add(new Merchant.Item(s, _itemDb.GetPrice(s), _itemDb.GetIconId(s), _itemDb.GetColor(s)));
            }
            else if (line.StartsWith("BUYITEM"))
            {
                var s = line.Split(":")[1];
                items[1].Add(new Merchant.Item(s, _itemDb.GetPrice(s), _itemDb.GetIconId(s), _itemDb.GetColor(s)));
            }
            else if (line.StartsWith("SELLIMAGE"))
            {
                var s = line.Split(":")[1];
                avNubmer = int.Parse(s);
            }
        }
        fileAccess.Dispose();
        merchant.InitializeMerchant(interpolation, map, items[0], items[1], avNubmer);
        return merchant;
    }

    private Monster CreateMonster(NpcInterpolation interpolation, IMap map)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Monster.tscn");
        var monster = scene.Instantiate<Monster>();
        Monster.Initialize(monster, interpolation, map);
        return monster;
    }


    public Monster CreateNpc(NpcInterpolation interpolation, IMap map)
    {
        if (interpolation.NpcType == NpcType.MERCHANT)
        {
            return CreateMerchant(interpolation, map);
        }
        return CreateMonster(interpolation, map);
    }

    public Teleport CreateTeleport(TeleportInterpolation interpolation)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Teleport.tscn");
        var teleport = scene.Instantiate<Teleport>();
        var texture2D = _itemIconReader.Get(interpolation.Shape);
        if (texture2D == null)
        {
            throw new NotImplementedException("no shape id " + interpolation.Id);
        }
        teleport.Init(interpolation.Id, interpolation.Coordinate, interpolation.Name, 
            new OffsetTexture(texture2D, new Vector2I(-VectorUtil.TileSizeX / 2, 0 )));
        return teleport;
    }
    

    public GameDynamicObject CreateObject(DynamicObjectInterpolation interpolation, IMap map)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/DynamicObject.tscn");
        var obj = scene.Instantiate<GameDynamicObject>();
        if (interpolation.Type == DynamicObjectType.GUILD_STONE)
        {
            var texture2D = _itemIconReader.Get(interpolation.Shape.ToInt());
            if (texture2D == null)
                throw new ArgumentException("Shape " + interpolation.Shape + " invalid.");
            obj.Initialize(interpolation, map, new List<OffsetTexture>() {new(texture2D, new Vector2(8, -12))});
        }
        else
        {
            var atzSprite = _spriteRepository.LoadByNumberAndOffset("x" + interpolation.Shape, new Vector2I(16, 12));
            obj.Initialize(interpolation, map, atzSprite.GetAll());
        }
        return obj;
    }
}