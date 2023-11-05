using Microsoft.Win32;
using System;
using System.Globalization;
using WPFLocalizeExtension.Extensions;

namespace DarkMode_2.Models;

public class LanguageHandler
{
    private string _i18nFolderPath;
    private string _currentLanguageCode;

    public LanguageHandler(string i18nFolderPath)
    {
        _i18nFolderPath = i18nFolderPath;
        _currentLanguageCode = RegistryInit.GetSavedLanguageCode();
        InitializeLanguage();
        MonitorLanguageChanges();
    }

    private void InitializeLanguage()
    {
        ChangeLanguage(_currentLanguageCode);
    }

    private void MonitorLanguageChanges()
    {
        SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
    }

    private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
    {
        if (e.Category == UserPreferenceCategory.Locale)
        {
            string newLanguageCode = RegistryInit.GetSavedLanguageCode();
            if (newLanguageCode != _currentLanguageCode)
            {
                _currentLanguageCode = newLanguageCode;
                ChangeLanguage(newLanguageCode);
            }
        }
    }

    public void ChangeLanguage(string languageCode)
    {
        WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = new CultureInfo(languageCode);
    }

    public static string GetLocalizedString(string key, string resourceFileName = "Languages", bool addSpaceAfter = false)
    {
        var localizedString = String.Empty;

        // Build up the fully-qualified name of the key
        var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        var fullKey = assemblyName + ":" + resourceFileName + ":" + key;
        var locExtension = new LocExtension(fullKey);
        locExtension.ResolveLocalizedValue(out localizedString);

        // Add a space to the end, if requested
        if (addSpaceAfter)
        {
            localizedString += " ";
        }

        return localizedString;
    }
}
