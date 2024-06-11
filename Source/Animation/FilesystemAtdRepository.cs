using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using y1000.Source.Creature;

namespace y1000.Source.Animation;

public class FilesystemAtdRepository : IAtdRepository
{
    public static readonly FilesystemAtdRepository Instance = new();
    
    private static readonly string DIR_PATH = "D:/work/atd/";
    
    private static readonly IDictionary<string, AtdStructure> CACHE = new Dictionary<string, AtdStructure>();
    
    private static readonly IDictionary<CreatureState, string> MONSTER_ACTION_MAP = new Dictionary<CreatureState, string>()
    {
        { CreatureState.WALK, "MOVE" },
        { CreatureState.IDLE, "TURNNING" },
        { CreatureState.HURT, "STRUCTED" },
        { CreatureState.ATTACK, "HIT1" },
        { CreatureState.DIE, "DIE" },
        { CreatureState.FROZEN, "TURN" },
    };

    
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
    

    private static List<string> ConvertToStrings(byte[] allBytes)
    {
        List<string> list = new List<string>();
        int cnt = allBytes.Length / 255;
        for (int i = 0; i < cnt; i++)
        {
            var buffer = allBytes.Skip(i * 255).ToArray();
            var convert = Convert(buffer);
            if (convert != null)
            {
                list.Add(convert);
            }
        }
        return list;
    }
    
    private List<AtdAction> ConvertToActions(List<string> list)
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


    private AtdStructure Load(string path, IDictionary<CreatureState, string> actionMap)
    {
        if (!path.EndsWith(".atd"))
        {
            path += ".atd";
        }
        if (CACHE.TryGetValue(path, out var structure))
        {
            return structure;
        }
        var allbytes = File.ReadAllBytes(path);
        var atdStrings = ConvertToStrings(allbytes);
        var actions = ConvertToActions(atdStrings);
        IDictionary<string, List<AtdAction>> dictionary = new Dictionary<string, List<AtdAction>>();
        foreach (var action in actions)
        {
            if (!dictionary.ContainsKey(action.Action))
            {
                dictionary.Add(action.Action, new List<AtdAction>());
            }

            if (dictionary.TryGetValue(action.Action, out var list))
            {
                list.Add(action);
            }
        }
        var atdStructure = new AtdStructure(dictionary, actionMap);
        CACHE.Add(path, atdStructure);
        return atdStructure;
    }

    public AtdStructure LoadMonster(string fileName)
    {
        return Load(DIR_PATH + fileName, MONSTER_ACTION_MAP);
    }
}