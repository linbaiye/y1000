using System;
using System.Collections.Generic;
using y1000.Source.Character;
using y1000.Source.Control.Assistance;

namespace y1000.Source.Assistant;

public class AutoFillAssistant
{
    
    private CharacterImpl _character;

    private long _nextMillis;

    private bool _grindExp;

    private int _interval;

    private class Setting
    {
        public AutoFillOption Option { get; set; }
        public int Threshold { get; set; }
        public int UseSlot { get; set; }

        public bool Valid => Threshold > 0 && UseSlot > 0;
    }

    private IDictionary<AutoFillOption, Setting> _settings;
    
    public AutoFillAssistant(CharacterImpl character)
    {
        _nextMillis = 0;
        _grindExp = false;
        _settings = new Dictionary<AutoFillOption, Setting>();
        _character = character;
        _interval = 300;
    }

    public void UpdateAutoFillItem(AutoFillOption option, HealItemView view)
    {
        _settings.Remove(option);
        if (view.IsAutoFillEnabled())
        {
            _settings.Add(option, new Setting()
            {
                Option = option,
                Threshold = view.Percentage(),
                UseSlot = view.BondSlot,
            });
        }
        else
        {
            _settings.Add(option, new Setting()
            {
                Option = option,
                Threshold = -1,
                UseSlot = 0,
            });
        }
    }

    public void UpdateExpAndInterval(bool enableExp, int interval)
    {
        _grindExp = enableExp;
        _interval = interval > 300 ? interval : 300;
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

    public void Update()
    {
        var current = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        if (current < _nextMillis)
        {
            return;
        }
        _nextMillis = current + _interval;
        foreach (var setting in _settings.Values)
        {
            if (setting.Valid && ShouldUse(setting.Option, setting.Threshold))
            {
                _character.Inventory.OnUIDoubleClick(setting.UseSlot);
                break;
            }
        }
    }
}