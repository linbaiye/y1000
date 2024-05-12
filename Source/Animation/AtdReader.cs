using System;
using System.Collections.Generic;
using Godot;
using NLog;

namespace y1000.Source.Animation;

public class AtdReader
{
    private readonly IDictionary<string, List<AtdStruct>> structs;

    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private AtdReader(IDictionary<string, List<AtdStruct>> structs)
    {
        this.structs = structs;
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
    

    private static List<AtdFrameDescriptor> ToArray(string[] strings)
    {
        List<AtdFrameDescriptor> result = new List<AtdFrameDescriptor>();
        for (int i = 'A', index = 5; i <= 'P'; i++, index += 3)
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

    private static List<AtdStruct> Convert(List<string> list)
    {
        List<AtdStruct> result = new List<AtdStruct>();
        foreach (var str in list)
        {
            var strings = str.Split(",");
            if (string.IsNullOrEmpty(strings[0]) || "Name".Equals(strings[0]))
            {
                continue;
            }
            result.Add(new AtdStruct(strings[1], strings[2], strings[3].ToInt(), strings[4].ToInt(), ToArray(strings)));
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
    

    public static AtdReader? Load(string atdName)
    {
        FileAccess fileAccess = FileAccess.Open( "res://sprite/char/" + atdName, FileAccess.ModeFlags.Read);
        if (fileAccess == null)
        {
            throw new ArgumentException("Invalid atd:" + atdName);
        }
        var readStrings = ReadStrings(fileAccess);
        var atdStructs = Convert(readStrings);
        IDictionary<string, List<AtdStruct>> dictionary = new Dictionary<string, List<AtdStruct>>();
        foreach (var atdStruct in atdStructs)
        {
            Logger.Debug("Struct {0}.", atdStruct );
            foreach (var atdStructFrameDescriptor in atdStruct.FrameDescriptors)
            {
                Logger.Debug("Offset {0}", atdStructFrameDescriptor);
            }
            Logger.Debug("----------------------------------------------------------");
            if (!dictionary.ContainsKey(atdStruct.Action))
            {
                dictionary.Add(atdStruct.Action, new List<AtdStruct>());
            }

            if (dictionary.TryGetValue(atdStruct.Action, out var list))
            {
                list.Add(atdStruct);
            }
        }
        return new AtdReader(dictionary);
    }
}