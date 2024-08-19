using System;
using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Creature;

namespace y1000.Source.Animation;

public class AtdStructure
{
    private readonly IDictionary<string, List<AtdAction>> _structs;

    private readonly IDictionary<CreatureState, string> _actionMap;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    public AtdStructure(IDictionary<string, List<AtdAction>> structs, IDictionary<CreatureState, string> actionmap)
    {
        _structs = structs;
        _actionMap = actionmap;
    }

    private static readonly IDictionary<CreatureState, string> MONSTER_ACTION_MAP = new Dictionary<CreatureState, string>()
    {
        { CreatureState.WALK, "MOVE" },
        { CreatureState.IDLE, "TURNNING" },
        { CreatureState.HURT, "STRUCTED" },
        { CreatureState.ATTACK, "HIT1" },
        { CreatureState.DIE, "DIE" },
        { CreatureState.FROZEN, "TURN" },
    };

    private static readonly IDictionary<CreatureState, string> PLAYER_ACTION_MAP = new Dictionary<CreatureState, string>()
    {
        { CreatureState.WALK, "MOVE" },
        { CreatureState.ENFIGHT_WALK, "MOVE1" },
        { CreatureState.RUN, "MOVE4" },
        { CreatureState.FLY, "MOVE5" },
        { CreatureState.IDLE, "TURN" },
        { CreatureState.COOLDOWN, "TURN1" },
        { CreatureState.STANDUP, "STANDUP" },
        { CreatureState.SIT, "SEATDOWN" },
        { CreatureState.HURT, "STRUCTED" },
        { CreatureState.DIE, "DIE" },
        { CreatureState.HELLO, "HELLO" },
        { CreatureState.FIST, "HIT" },
        { CreatureState.KICK, "HIT1" },
        { CreatureState.BLADE, "HIT2" },
        { CreatureState.SWORD, "HIT2" },
        { CreatureState.THROW, "HIT2" },
        { CreatureState.AXE, "HIT3" },
        { CreatureState.SPEAR, "HIT3" },
        { CreatureState.BOW, "HIT4" },
        { CreatureState.BLADE2H, "HIT7" },
        { CreatureState.SWORD2H, "HIT8" },
    };

    private static readonly IDictionary<Direction, string> DIRECTION_MAP = new Dictionary<Direction, string>()
    {
        { Direction.UP, "DR_0" },
        { Direction.UP_RIGHT, "DR_1" },
        { Direction.RIGHT, "DR_2" },
        { Direction.DOWN_RIGHT, "DR_3" },
        { Direction.DOWN, "DR_4" },
        { Direction.DOWN_LEFT, "DR_5" },
        { Direction.LEFT, "DR_6" },
        { Direction.UP_LEFT, "DR_7" },
    };

    private static string? Convert(byte[] bytes)
    {
        for (int i = 0; i < bytes.Length; i++)
        {
            var b = bytes[i];
            var l = 0x0f & b;
            var h = 0xf0 & b;
            bytes[i] = (byte)((h >> 4) + (l << 4));
        }
        int len = bytes[0] & 0xff;
        if (len <= 0)
        {
            return null;
        }
        return System.Text.Encoding.UTF8.GetString(bytes, 1, len);
    }

    public List<AtdAction> Find(CreatureState state)
    {
        if (!_actionMap.TryGetValue(state, out var stateStr))
        {
            throw new NotImplementedException("No state string mapping found: " + state);
        }
        if (!_structs.TryGetValue(stateStr, out var list))
        {
            throw new NotImplementedException("No state mapping found: " + stateStr);
        }
        return list;
    }

    public bool HasState(CreatureState state)
    {
        return _actionMap.TryGetValue(state, out var stateString) && _structs.ContainsKey(stateString);
    }

    public AtdAction FindFirst(CreatureState state)
    {
        return Find(state)[0];
    }

    private static List<AtdFrameDescriptor> ToArray(string[] strings)
    {
        List<AtdFrameDescriptor> result = new List<AtdFrameDescriptor>();
        for (int i = 'A', index = 5; i <= 'P' && index + 2 < strings.Length; i++, index += 3)
        {
            var frame = strings[index];
            if (string.IsNullOrEmpty(frame))
            {
                continue;
            }
            var px = strings[index + 1];
            var py  = strings[index + 2];
            result.Add(new AtdFrameDescriptor(frame.ToInt(), string.IsNullOrEmpty(px) ? 0 : px.ToInt(), string.IsNullOrEmpty(py) ? 0 : py.ToInt()));
        }

        return result;
    }

    private static List<AtdAction> Convert(List<string> list)
    {
        List<AtdAction> result = new List<AtdAction>();
        foreach (var str in list)
        {
            var strings = str.Split(",");
            if (string.IsNullOrEmpty(strings[0]) || "Name".Equals(strings[0]))
            {
                continue;
            }
            result.Add(new AtdAction(strings[1], strings[2], strings[3].ToInt(), strings[4].ToInt(), ToArray(strings)));
        }
        return result;
    }
    

    private static List<string> ReadStrings(FileAccess fileAccess)
    {
        List<string> list = new List<string>();
        int cnt = (int)fileAccess.GetLength() / 255;
        for (int i = 0; i < cnt; i++)
        {
            var buffer = fileAccess.GetBuffer(255);
            if (buffer == null)
            {
                break;
            }
            var convert = Convert(buffer);
            if (convert != null)
            {
                list.Add(convert);
            }
        }
        return list;
    }

    private static AtdStructure Load(string path, IDictionary<CreatureState, string> actionMap)
    {
        FileAccess fileAccess = FileAccess.Open( path, FileAccess.ModeFlags.Read);
        if (fileAccess == null)
        {
            throw new ArgumentException("Invalid atd:" + path);
        }
        var readStrings = ReadStrings(fileAccess);
        var atdStructs = Convert(readStrings);
        IDictionary<string, List<AtdAction>> dictionary = new Dictionary<string, List<AtdAction>>();
        foreach (var atdStruct in atdStructs)
        {
            if (!dictionary.ContainsKey(atdStruct.Action))
            {
                dictionary.Add(atdStruct.Action, new List<AtdAction>());
            }

            if (dictionary.TryGetValue(atdStruct.Action, out var list))
            {
                list.Add(atdStruct);
            }
        }
        fileAccess.Dispose();
        return new AtdStructure(dictionary, actionMap);
    }

    
    public static AtdStructure LoadPlayer(string atdName)
    {
        return Load( "res://sprite/char/" + atdName, PLAYER_ACTION_MAP);
    }
}