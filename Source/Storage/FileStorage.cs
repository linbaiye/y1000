using Godot;

namespace y1000.Source.Storage;

public class FileStorage
{

    private readonly string _space;

    private readonly string _dirPath;

    public FileStorage(string space)
    {
        _space = space;
        _dirPath = "user://data/" + _space;
        if (!DirAccess.DirExistsAbsolute(_dirPath))
            DirAccess.MakeDirRecursiveAbsolute(_dirPath);
    }

    private string MakePath(string name)
    {
        return _dirPath + "/" + name;
    }

    public void Save(string name, string content)
    {
        var path = MakePath(name);
        var fileAccess = FileAccess.Open(path, FileAccess.ModeFlags.Write);
        fileAccess.StoreString(content);
        fileAccess.Close();
    }

    public string? Load(string name)
    {
        var path = MakePath(name);
        if (!FileAccess.FileExists(path))
        {
            return null;
        }
        FileAccess fileAccess = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        var str = fileAccess.GetAsText();
        fileAccess.Close();
        return str;
    }
}