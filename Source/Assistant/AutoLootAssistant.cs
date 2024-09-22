using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using y1000.Source.Character;
using y1000.Source.Entity;
using y1000.Source.Storage;
using y1000.Source.Util;

namespace y1000.Source.Assistant;

public class AutoLootAssistant
{
    private readonly EntityManager _entityManager;
    private readonly CharacterImpl _character;

    private const double Interval = 0.5;

    private double _timer;

    private FileStorage _fileStorage;
    private const string FileName = "autoloot";
    
    private class Settings
    {
        public List<string> Loot { get; set; } = new();
        public bool Auto { get; set; }
        public bool Reverse { get; set; }
    }

    private readonly Settings _settings;

    public List<string> Loot
    {
        get => _settings.Loot;
        private set => _settings.Loot = value;
    }

    public bool Auto
    {
        get => _settings.Auto;
        private set => _settings.Auto = value;
    }

    public bool Reverse
    {
        get => _settings.Reverse;
        private set => _settings.Reverse = value;
    }

    private void Save()
    {
        _fileStorage.Save(FileName, JsonSerializer.Serialize(_settings));
    }

    private AutoLootAssistant(EntityManager entityManager,
        CharacterImpl character, Settings settings,
        FileStorage fileStorage)
    {
        _entityManager = entityManager;
        _character = character;
        _timer = Interval;
        _settings = settings;
        _fileStorage = fileStorage;
    }

    
    public void Process(double d)
    {
        if (!Auto)
            return;
        _timer -= d;
        if (_timer > 0)
            return;
        _timer = Interval;
        _entityManager
            .Select<GroundItem>(item => item.Coordinate.Distance(_character.Coordinate) <= 2)
            .FirstOrDefault(item => Reverse ? !Loot.Contains(item.EntityName.Split(":")[0]) : Loot.Contains(item.EntityName.Split(":")[0]))?.Pick();
    }
    
    public void OnReverseCheckboxChanged(bool enable)
    {
        Reverse = enable;
        Save();
    }


    public List<string> VisibleItems => _entityManager.Select<GroundItem>(i => true)
                .Select(i => i.EntityName.Split(":")[0]).ToList();

    public void OnAutoCheckboxChanged(bool enable)
    {
        Auto = enable;
        Save();
    }

    public void OnLootItemsChanged(List<string> items)
    {
        Loot = items;
        Save();
    }

    public void OnAddItemClicked(string item) {
        if (!Loot.Contains(item)) {
            Loot.Add(item);
            Save();
        }
    }

    public void OnRemoveLootItemClicked(string item) {
        Loot.Remove(item);
    }

    public static AutoLootAssistant Create(EntityManager entityManager,
        CharacterImpl character)
    {
        FileStorage fileStorage = new FileStorage(character.EntityName);
        var content = fileStorage.Load(FileName);
        var settings = new Settings()
        {
            Loot = new List<string>(),
            Auto = true,
            Reverse = false,
        };
        if (!string.IsNullOrEmpty(content))
        {
            var s = JsonSerializer.Deserialize<Settings>(content);
            if (s != null)
            {
                settings = s;
            }
        }
        return new AutoLootAssistant(entityManager, character, settings, fileStorage);
    }
}