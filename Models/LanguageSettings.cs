using System.Collections.Generic;

namespace DarkMode_2.Models
{
    public class LanguageSettings
    {
        public List<LanguageInfo> Languages { get; set; }

        public string GetLanguageCodeByName(string name)
        {
            foreach (var language in Languages)
            {
                if (language.Name == name)
                {
                    return language.Code;
                }
            }
            return null;
        }

        public string GetLanguageNameByCode(string code)
        {
            foreach (var language in Languages)
            {
                if (language.Code == code)
                {
                    return language.Name;
                }
            }
            return null;
        }
    }

    public class LanguageInfo
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
