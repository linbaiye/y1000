using System.IO;
using System.IO.Compression;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Map;
using FileAccess = System.IO.FileAccess;

namespace y1000.Source.Util;

public class AtdChecker
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private static void CheckState(string name, CreatureState state, AtdStructure atdStructure)
    {
        if (!atdStructure.HasState(state))
        {
            Logger.Debug("{0} has no state {1}.", name, state);
            return;
        }
        var atdActions = atdStructure.Find(state);
        foreach (var atdAction in atdActions)
        {
            if (atdAction.Frame != atdAction.FrameDescriptors.Count)
            {
                Logger.Error("{0} has error, state {1}, frame count {2}, list length {3}.", name, state, atdAction.Frame,atdAction.FrameDescriptors.Count);
            }
        }
    }

    public static void TestZip()
    {
        // var zipArchive = ZipFile.Open("/Users/ab000785/learn/map/start.zip", ZipArchiveMode.Read);
        // var zipArchiveEntry = zipArchive.GetEntry("start/object/101/0.png");
        // if (zipArchiveEntry != null)
        // {
        //     var stream = zipArchiveEntry.Open();
        //     Logger.Debug("Readable {0}.", stream.CanRead);
        // }
        //new ZipFileMapObjectRepository().LoadObjects("start");
    }

    public static void ZipAll()
    {
        var directories = Directory.GetDirectories("/Users/ab000785/learn/map");
        foreach (var dir in directories)
        {
            Logger.Debug("Zip file {0}.", dir);
            if (File.Exists(dir + ".zip"))
                File.Delete(dir + ".zip");
            ZipFile.CreateFromDirectory(dir, dir + ".zip");
        }
    }
        

    public static void Check()
    {
        for (int i = 1; i <= 139; i++)
        {
            var atd = FilesystemAtdRepository.Instance.LoadByName(i.ToString());
            CheckState(i.ToString(), CreatureState.WALK, atd);
            CheckState(i.ToString(), CreatureState.DIE, atd);
            CheckState(i.ToString(), CreatureState.IDLE, atd);
            CheckState(i.ToString(), CreatureState.HURT, atd);
            CheckState(i.ToString(), CreatureState.ATTACK, atd);
        }
        
    }


    private static void Print(AtdAction atdAction)
    {
        var descriptors = atdAction.FrameDescriptors;
        Logger.Debug("State {3}, frame time {0}, number {1}, total time {2}, start {4}, end {5}.", atdAction.FrameTime, atdAction.Frame, atdAction.ActionTime, atdAction.Action, descriptors[0].Number, 
            descriptors[atdAction.Frame - 1].Number);
    }

    public static void Dump()
    {
        var atd = FilesystemAtdRepository.Instance.LoadByName("29");
        Print(atd.FindFirst(CreatureState.WALK));
        Print(atd.FindFirst(CreatureState.IDLE));
        Print(atd.FindFirst(CreatureState.FROZEN));
    }

    public static void Convert()
    {
        var dirPath = "D:\\1000y研究\\九星千年UI\\maps\\";
        var files = Directory.GetFiles(dirPath);
        foreach (var file in files)
        {
            Logger.Debug("File {0}.", file);
            var fileName = Path.GetFileName(file);
            var newFile = dirPath + fileName.ToLower();
            File.Move(file, newFile);
        }
    }
}