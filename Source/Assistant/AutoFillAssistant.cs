using System;
using System.Collections.Generic;
using System.Text.Json;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.Assistance;
using y1000.Source.Storage;

namespace y1000.Source.Assistant;

public class AutoFillAssistant
{
    
    private readonly CharacterImpl _character;

    private double _timer;

    private Setting? _currentFill;

    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    private const string Name = "assist";

    public class Setting
    {
        public AutoFillOption Option { get; set; }
        
        public bool Enabled { get; set; }
        public int Threshold { get; set; }
        public string? UseItem { get; set; }
    }

    private AutoFillAssistant(CharacterImpl character)
    {
        _timer = 0;
        Settings = new Dictionary<AutoFillOption, Setting>();
        _character = character;
        GrindExp = false;
        Interval = 300;
    }

    public void UpdateAutoFillItem(AutoFillOption option, HealItemView view)
    {
        Settings.Remove(option);
        Settings.Add(option, new Setting()
        {
            Option = option,
            Threshold = view.Percentage() ?? 0,
            Enabled = view.Checked(),
            UseItem = view.BondName,
        });
    }

    public void UpdateExpAndInterval(bool enableExp, int interval)
    {
        GrindExp = enableExp;
        Interval = interval > 300 ? interval : 300;
    }
    
    public void OnViewClosed()
    {
        new FileStorage(_character.EntityName).Save(Name, ToString());
    }

    private bool ShouldUse(AutoFillOption option, int threshold)
    {
        switch (option)
        {
            case AutoFillOption.LIFE:
                return _character.HealthBar.Percent < threshold;
            case AutoFillOption.POWER:
                return _character.PowerBar.Percent < threshold;
            case AutoFillOption.INNER_POWER:
                return _character.InnerPowerBar.Percent < threshold;
            case AutoFillOption.OUTER_POWER:
                return _character.OuterPowerBar.Percent < threshold;
        }
        return false;
    }

    private bool IsValid(Setting setting)
    {
        return setting.Enabled && setting.Threshold > 0
                               && setting.UseItem != null;
    }

    private void UseItem(Setting setting)
    {
        if (!IsValid(setting)|| setting.UseItem == null)
            return;
        int slot = _character.Inventory.FindSlot(setting.UseItem);
        if (slot != 0)
            _character.Inventory.OnUIDoubleClick(slot);
    }


    private void UpdateKeepFill()
    {
        if (!GrindExp || _currentFill == null)
        {
            _currentFill = null;
            return;
        }
        bool done = true;
        switch (_currentFill.Option)
        {
            case AutoFillOption.POWER:
                done = _character.PowerBar.Percent >= 95;
                break;
            case AutoFillOption.INNER_POWER:
                done = _character.InnerPowerBar.Percent >= 95;
                break;
            case AutoFillOption.OUTER_POWER:
                done = _character.OuterPowerBar.Percent >= 95;
                break;
        }
        if (done)
        {
            _currentFill = null;
        }
    }
    

    public bool GrindExp { get; private set; }

    public int Interval { get; private set; }
        
    private IDictionary<AutoFillOption, Setting> Settings { get; set; }

    public Setting GetSettingsOrDefault(AutoFillOption option)
    {
        if (Settings.TryGetValue(option, out var s))
            return s;
        return new Setting()
        {
            Enabled = false,
            Option = option,
            Threshold = 10,
            UseItem = null
        };
    }
    
    private class Storage
    {
        public bool GrindExp { get; set; }
        public int Interval { get; set; }
        public IDictionary<AutoFillOption, Setting> Settings { get; set; } = new Dictionary<AutoFillOption, Setting>();
    }

    public override string ToString()
    {
        var storage = new Storage()
        {
            GrindExp = GrindExp,
            Interval = Interval,
            Settings = Settings,
        };
        return JsonSerializer.Serialize(storage);
    }

    private bool CanGrind(AutoFillOption option)
    {
        return option is AutoFillOption.POWER or AutoFillOption.INNER_POWER or AutoFillOption.OUTER_POWER;
    }


    public void Process(double delta)
    {
        if (_character.Dead) 
            return;
        UpdateKeepFill();
        _timer -= delta;
        if (_timer > 0)
        {
            return;
        }
        _timer = (double)Interval / 1000;
        if (_currentFill != null)
        {
            UseItem(_currentFill);
            return;
        }
        if (Settings.TryGetValue(AutoFillOption.LOW_LIFE, out var s))
        {
            if (ShouldUse(s.Option, s.Threshold))
            {
                UseItem(s);
                return;
            }
        }
        foreach (var setting in Settings.Values)
        {
            if (IsValid(setting) && ShouldUse(setting.Option, setting.Threshold))
            {
                UseItem(setting);
                if (GrindExp && CanGrind(setting.Option))
                    _currentFill = setting;
                break;
            }
        }
    }

    public static AutoFillAssistant Create(CharacterImpl character)
    {
        FileStorage fileStorage = new FileStorage(character.EntityName);
        var content = fileStorage.Load(Name);
        var autoFillAssistant = new AutoFillAssistant(character);
        if (string.IsNullOrEmpty(content))
        {
            return autoFillAssistant;
        }
        try
        {
            var storage = JsonSerializer.Deserialize<Storage>(content);
            if (storage != null)
            {
                autoFillAssistant.Settings = storage.Settings;
                autoFillAssistant.GrindExp = storage.GrindExp;
                autoFillAssistant.Interval = storage.Interval;
            }
        }
        catch (Exception e)
        {
            LOGGER.Debug(e);
        }
        return autoFillAssistant;
    }
}