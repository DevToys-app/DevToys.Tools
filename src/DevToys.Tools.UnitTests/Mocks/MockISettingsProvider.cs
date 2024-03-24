namespace DevToys.Tools.UnitTests.Mocks;

internal class MockISettingsProvider : ISettingsProvider
{
    private readonly Dictionary<string, object> _settings = new();

    public event EventHandler<SettingChangedEventArgs> SettingChanged;

    public T GetSetting<T>(SettingDefinition<T> settingDefinition)
    {
        if (_settings.TryGetValue(settingDefinition.Name, out object value))
            return (T)value!;

        return settingDefinition.DefaultValue;
    }

    public void ResetSetting<T>(SettingDefinition<T> settingDefinition)
    {
        SettingChanged?.Invoke(this, new SettingChangedEventArgs(settingDefinition.Name, settingDefinition.DefaultValue));
    }

    public void SetSetting<T>(SettingDefinition<T> settingDefinition, T value)
    {
        _settings[settingDefinition.Name] = value;
        SettingChanged?.Invoke(this, new SettingChangedEventArgs(settingDefinition.Name, value));
    }
}
