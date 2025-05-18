using System;
using System.Globalization;
using System.Windows;

namespace EasySave2.Services
{
    public static class LanguageService
    {
        public enum SupportedLanguage
        {
            English,
            French
        }

        public static void SetLanguage(SupportedLanguage language)
        {
            string langCode = language switch
            {
                SupportedLanguage.English => "en",
                SupportedLanguage.French => "fr",
                _ => "en"
            };

            var dict = new ResourceDictionary
            {
                Source = new Uri($"/Resources/StringResources.{langCode}.xaml", UriKind.Relative)
            };

            
            for (int i = Application.Current.Resources.MergedDictionaries.Count - 1; i >= 0; i--)
            {
                var md = Application.Current.Resources.MergedDictionaries[i];
                if (md.Source != null && md.Source.OriginalString.Contains("StringResources."))
                    Application.Current.Resources.MergedDictionaries.RemoveAt(i);
            }

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
