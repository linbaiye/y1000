using System;
using System.Collections.Generic;
using Godot;
using y1000.Source.Creature.Monster;
using y1000.Source.DynamicObject;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Sprite;
using y1000.Source.Util;

namespace y1000.Source.Entity;

public class EntityFactory
{

    private readonly ItemSdbReader _itemDb;
    
    private readonly IconReader _iconReader;

    private readonly EventMediator _eventMediator;
    public EntityFactory(EventMediator eventMediator)
    {
        _itemDb = ItemSdbReader.ItemSdb;
        _iconReader = IconReader.ItemIconReader;
        _eventMediator = eventMediator;
    }


    public OnGroundItem CreateOnGroundItem(ShowItemMessage message) 
    {
        var iconId = _itemDb.GetIconId(message.Name);
        var texture2D = _iconReader.Get(iconId);
        if (texture2D == null)
        {
            throw new NotImplementedException(message.Name + " does not have icon.");
        }
        return OnGroundItem.Create(message, texture2D, _eventMediator);
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
                items[0].Add(new Merchant.Item(s, _itemDb.GetPrice(s), _itemDb.GetIconId(s)));
            }
            else if (line.StartsWith("BUYITEM"))
            {
                var s = line.Split(":")[1];
                items[1].Add(new Merchant.Item(s, _itemDb.GetPrice(s), _itemDb.GetIconId(s)));
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

    public GameDynamicObject CreateObject(DynamicObjectInterpolation interpolation)
    {
        return null;
    }
}