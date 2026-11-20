/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-19
 * Organization:    #ORGANIZATION#
 * Description:     Provides lightweight JSON-based localization for EZEditorTools.
 */
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public enum EZLanguage
    {
        System,
        Chinese,
        English
    }

    public static class EZLocalization
    {
        private const string LanguagePreferenceKey = "EZEditorTools.Language";
        private const string LanguageAssetRoot = "Packages/ezhex1991.ezeditortools/Editor/Localization/Languages/";

        [Serializable]
        private class LanguageEntry
        {
            public string key;
            public string value;
        }

        [Serializable]
        private class LanguageFile
        {
            public string language;
            public LanguageEntry[] entries;
        }

        private static readonly Dictionary<EZLanguage, Dictionary<string, string>> languages =
            new Dictionary<EZLanguage, Dictionary<string, string>>();
        private static readonly Dictionary<EZLanguage, string> languageSources =
            new Dictionary<EZLanguage, string>();

        public static EZLanguage Language
        {
            get { return (EZLanguage)EditorPrefs.GetInt(LanguagePreferenceKey, (int)EZLanguage.System); }
        }

        public static event Action LanguageChanged;

        public static void SetLanguage(EZLanguage language)
        {
            EditorPrefs.SetInt(LanguagePreferenceKey, (int)language);
            if (LanguageChanged != null)
            {
                LanguageChanged();
            }
        }

        public static string Get(string key)
        {
            EZLanguage language = ResolveLanguage(Language);
            string value;
            if (GetLanguage(language).TryGetValue(key, out value)) return value;
            if (GetLanguage(EZLanguage.English).TryGetValue(key, out value)) return value;
            return key;
        }

        public static string[] GetOptions(params string[] keys)
        {
            string[] options = new string[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                options[i] = Get(keys[i]);
            }
            return options;
        }

        private static Dictionary<string, string> GetLanguage(EZLanguage language)
        {
            string assetName = language == EZLanguage.Chinese ? "zh-CN" : "en-US";
            string assetPath = LanguageAssetRoot + assetName + ".json";
            TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            string source = asset == null ? string.Empty : asset.text;

            string previousSource;
            if (!languages.ContainsKey(language) || !languageSources.TryGetValue(language, out previousSource) || previousSource != source)
            {
                languages[language] = ParseLanguage(source, assetPath);
                languageSources[language] = source;
            }
            return languages[language];
        }

        private static Dictionary<string, string> ParseLanguage(string source, string assetPath)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(source)) return result;

            try
            {
                LanguageFile languageFile = JsonUtility.FromJson<LanguageFile>(source);
                if (languageFile == null || languageFile.entries == null) return result;

                for (int i = 0; i < languageFile.entries.Length; i++)
                {
                    LanguageEntry entry = languageFile.entries[i];
                    if (entry == null || string.IsNullOrEmpty(entry.key)) continue;
                    result[entry.key] = entry.value ?? string.Empty;
                }
            }
            catch (Exception exception)
            {
                Debug.LogErrorFormat("Failed to load localization file '{0}': {1}", assetPath, exception.Message);
            }
            return result;
        }

        private static EZLanguage ResolveLanguage(EZLanguage language)
        {
            if (language != EZLanguage.System) return language;

            switch (Application.systemLanguage)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.ChineseTraditional:
                    return EZLanguage.Chinese;
                default:
                    return EZLanguage.English;
            }
        }
    }
}
