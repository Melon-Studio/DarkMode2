using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace DarkMode_2.ViewModels;

public class SetSettingViewModel : ObservableObject
{
    private bool _dataInitialized = false;

    private IEnumerable<string> _comboCollection = new string[] { };

    public IEnumerable<string> ComboCollection
    {
        get => _comboCollection;
        set => SetProperty(ref _comboCollection, value);
    }

    public void OnNavigatedTo()
    {
        if (!_dataInitialized)
            InitializeData();
    }
    private void InitializeData()
    {
        ComboCollection = new[]
        {
            "简体中文(zh-CN)",
            "繁体中文(zh-TW)",
            "日本語(ja-JP)",
            "Русский(ru-RU)",
            "English(en-EN)"
        };
        _dataInitialized = true;
    }
}
