using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Godot;
using NLog;
using y1000.Source.Creature;
using FileAccess = Godot.FileAccess;

namespace y1000.Source.Animation;

public class FilesystemAtdRepository : IAtdRepository
{
    public static readonly FilesystemAtdRepository Instance = new();
    
    //private static readonly string DIR_PATH = "D:/work/atd/";
    private static readonly string DIR_PATH = "res://assets/atd/";
    
    private static readonly IDictionary<string, AtdStructure> CACHE = new Dictionary<string, AtdStructure>();
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private static readonly IDictionary<CreatureState, string> MONSTER_ACTION_MAP = new Dictionary<CreatureState, string>()
    {
        { CreatureState.WALK, "MOVE" },
        { CreatureState.IDLE, "TURNNING" },
        { CreatureState.HURT, "STRUCTED" },
        { CreatureState.ATTACK, "HIT1" },
        { CreatureState.DIE, "DIE" },
        { CreatureState.FROZEN, "TURN" },
    };

    
    private static List<AtdFrameDescriptor> ToFrameArray(int total, string[] strings)
    {
        List<AtdFrameDescriptor> result = new List<AtdFrameDescriptor>();
        for (int i = 'A', index = 5, j = 0; i <= 'Z' && index + 2 < strings.Length && j < total; i++, index += 3, j++)
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
    

    
    private List<AtdAction> ConvertToActions(List<string> list)
    {
        List<AtdAction> result = new List<AtdAction>();

        foreach (var str in list)
        {
            var strings = Regex.Replace(str, @"\s+", "").Split(",");
            if (string.IsNullOrEmpty(strings[0]) || "Name".Equals(strings[0]))
            {
                continue;
            }
            string action = strings[1];
            string direction = strings[2];
            int frame = strings[3].ToInt();
            int frameTime = action.Equals("TURN") ? strings[4].ToInt() * 10 : strings[4].ToInt();
            result.Add(new AtdAction(action, direction, frame, frameTime, ToFrameArray(frame, strings)));
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
    

    private AtdStructure Load(string path, IDictionary<CreatureState, string> actionMap)
    {
        if (CACHE.TryGetValue(path, out var structure))
        {
            return structure;
        }

        FileAccess fileAccess = FileAccess.Open( path, FileAccess.ModeFlags.Read);
        if (fileAccess == null)
        {
            throw new ArgumentException("Invalid atd:" + path);
        }
        var readStrings = ReadStrings(fileAccess);
        var atdStructs = ConvertToActions(readStrings);
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
        var atdStructure = new AtdStructure(dictionary, actionMap);
        CACHE.Add(path, atdStructure);
        return atdStructure;
    }

    public AtdStructure LoadByName(string fileName)
    {
        return Load(DIR_PATH + (fileName.EndsWith(".atd") ? fileName : fileName + ".atd"), MONSTER_ACTION_MAP);
    }

    public bool HasFile(string fileName)
    {
        return FileAccess.FileExists(DIR_PATH + (fileName.EndsWith(".atd") ? fileName : fileName + ".atd"));
    }
}