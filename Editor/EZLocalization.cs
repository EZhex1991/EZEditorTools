/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-19
 * Organization:    #ORGANIZATION#
 * Description:     Provides lightweight bilingual localization for EZEditorTools.
 */
using System;
using System.Collections.Generic;
using UnityEditor;

namespace EZhex1991.EZEditorTools
{
    public enum EZLanguage
    {
        Chinese,
        English
    }

    public static class EZLocalization
    {
        private static readonly Dictionary<EZLanguage, Dictionary<string, string>> languages = CreateLanguages();

        public static EZLanguage Language
        {
            get { return (EZLanguage)EditorPrefs.GetInt(EZEditorPreferenceKeys.Language, (int)EZLanguage.English); }
        }

        public static event Action LanguageChanged;

        public static void SetLanguage(EZLanguage language)
        {
            EditorPrefs.SetInt(EZEditorPreferenceKeys.Language, (int)language);
            if (LanguageChanged != null) LanguageChanged();
        }

        public static string Get(string key)
        {
            string value;
            if (languages[Language].TryGetValue(key, out value)) return value;
            if (languages[EZLanguage.English].TryGetValue(key, out value)) return value;
            return key;
        }

        public static string Get(string group, string key) { return Get(group + "." + key); }
        public static string Get<T>(string key) { return Get(typeof(T).Name, key); }

        public static string[] GetOptions<T>(params string[] keys)
        {
            string[] options = new string[keys.Length];
            for (int i = 0; i < keys.Length; i++) options[i] = Get<T>(keys[i]);
            return options;
        }

        private static Dictionary<EZLanguage, Dictionary<string, string>> CreateLanguages()
        {
            var result = new Dictionary<EZLanguage, Dictionary<string, string>>
            {
                { EZLanguage.Chinese, new Dictionary<string, string>() },
                { EZLanguage.English, new Dictionary<string, string>() }
            };

            Add(result, "Common", "Script", "\u811a\u672c", "Script");
            Add(result, "Common", "Target", "\u76ee\u6807", "Target");
            Add(result, "EZImageCanvasTool", "ImageScope", "\u56fe\u7247\u8303\u56f4", "Image Scope");
            Add(result, "EZImageCanvasTool", "Scope.SelectedImages", "\u9009\u4e2d\u7684\u6240\u6709\u56fe\u7247", "Selected Images");
            Add(result, "EZImageCanvasTool", "Scope.SelectedFolder", "\u9009\u4e2d\u7684\u76ee\u5f55\u4e2d\u7684\u6240\u6709\u56fe\u7247", "Selected Folder");
            Add(result, "EZImageCanvasTool", "Scope.SelectedFolderAndSubfolders", "\u9009\u4e2d\u7684\u76ee\u5f55\u53ca\u5b50\u76ee\u5f55\u4e2d\u7684\u6240\u6709\u56fe\u7247", "Selected Folder And Subfolders");
            Add(result, "EZImageCanvasTool", "Folder", "\u76ee\u5f55", "Folder");
            Add(result, "EZImageCanvasTool", "FillColor", "\u586b\u5145\u989c\u8272", "Fill Color");
            Add(result, "EZImageCanvasTool", "Pivot", "\u4e2d\u5fc3\u70b9\u4f4d\u7f6e", "Pivot");
            Add(result, "EZImageCanvasTool", "Pivot.TopLeft", "\u5de6\u4e0a", "Top Left");
            Add(result, "EZImageCanvasTool", "Pivot.TopCenter", "\u4e0a\u4e2d", "Top Center");
            Add(result, "EZImageCanvasTool", "Pivot.TopRight", "\u53f3\u4e0a", "Top Right");
            Add(result, "EZImageCanvasTool", "Pivot.MiddleLeft", "\u5de6\u4e2d", "Middle Left");
            Add(result, "EZImageCanvasTool", "Pivot.Center", "\u4e2d\u5fc3", "Center");
            Add(result, "EZImageCanvasTool", "Pivot.MiddleRight", "\u53f3\u4e2d", "Middle Right");
            Add(result, "EZImageCanvasTool", "Pivot.BottomLeft", "\u5de6\u4e0b", "Bottom Left");
            Add(result, "EZImageCanvasTool", "Pivot.BottomCenter", "\u4e0b\u4e2d", "Bottom Center");
            Add(result, "EZImageCanvasTool", "Pivot.BottomRight", "\u53f3\u4e0b", "Bottom Right");
            Add(result, "EZImageCanvasTool", "Refresh", "\u5237\u65b0", "Refresh");
            Add(result, "EZImageCanvasTool", "ProcessAll", "\u5168\u90e8\u5904\u7406", "Process All");
            Add(result, "EZImageCanvasTool", "ImagesSummary", "\u56fe\u7247\uff1a{0}    \u4e0d\u7b26\u5408\uff1a{1}", "Images: {0}    Invalid: {1}");
            Add(result, "EZImageCanvasTool", "InvalidFolder", "\u8bf7\u9009\u62e9\u6709\u6548\u7684\u9879\u76ee\u76ee\u5f55\u3002", "Please select a valid project folder.");
            Add(result, "EZImageCanvasTool", "ProcessingWarning", "\u5904\u7406\u4f1a\u76f4\u63a5\u91cd\u5199\u652f\u6301\u7684\u56fe\u7247\u6587\u4ef6\uff0c\u65b0\u7684\u753b\u5e03\u5c3a\u5bf8\u4f1a\u6269\u5c55\u5230\u6700\u8fd1\u7684 4 \u7684\u500d\u6570\u3002", "Processing rewrites supported image files in place. The new canvas size is rounded up to the nearest multiple of 4.");
            Add(result, "EZImageCanvasTool", "NoSelectedImages", "Project \u7a97\u53e3\u4e2d\u6ca1\u6709\u9009\u4e2d\u7684\u56fe\u7247\u8d44\u6e90\u3002", "No image assets are selected in the Project window.");
            Add(result, "EZImageCanvasTool", "NoFolderImages", "\u9009\u4e2d\u7684\u76ee\u5f55\u4e2d\u6ca1\u6709\u627e\u5230\u56fe\u7247\u8d44\u6e90\u3002", "No image assets were found in the selected folder.");
            Add(result, "EZImageCanvasTool", "Asset", "\u8d44\u6e90", "Asset");
            Add(result, "EZImageCanvasTool", "Width", "\u5bbd\u5ea6", "Width");
            Add(result, "EZImageCanvasTool", "Height", "\u9ad8\u5ea6", "Height");
            Add(result, "EZImageCanvasTool", "Process", "\u5904\u7406", "Process");
            Add(result, "EZImageCanvasTool", "InspectorProcess", "\u6269\u5c55\u753b\u5e03\u5230 4 \u7684\u500d\u6570", "Expand Canvas to Multiple of 4");
            Add(result, "EZRenamerWindow", "Reset", "\u91cd\u7f6e", "Reset");
            Add(result, "EZRenamerWindow", "Case.None", "\u4e0d\u8f6c\u6362", "None");
            Add(result, "EZRenamerWindow", "Case.ToUpper", "\u8f6c\u6362\u4e3a\u5927\u5199", "To Upper");
            Add(result, "EZRenamerWindow", "Case.ToLower", "\u8f6c\u6362\u4e3a\u5c0f\u5199", "To Lower");
            Add(result, "EZRenamerWindow", "RenameAssets", "\u91cd\u547d\u540d\u8d44\u6e90", "Rename Assets");
            Add(result, "EZRenamerWindow", "AssetList", "\u8d44\u6e90\u5217\u8868 {0}", "Asset List {0}");
            Add(result, "EZRenamerWindow", "RenameSceneObjects", "\u91cd\u547d\u540d\u573a\u666f\u5bf9\u8c61", "Rename Scene Objects");
            Add(result, "EZRenamerWindow", "SceneObjectList", "\u573a\u666f\u5bf9\u8c61\u5217\u8868 {0}", "Scene Object List {0}");
            Add(result, "EZRenamerWindow", "CaseConversion", "\u5927\u5c0f\u5199\u8f6c\u6362", "Case Conversion");
            Add(result, "EZRenamerWindow", "UseRegex", "\u4f7f\u7528\u6b63\u5219\u8868\u8fbe\u5f0f", "Use Regex");
            Add(result, "EZRenamerWindow", "Replace", "\u66ff\u6362", "Replace");
            Add(result, "EZRenamerWindow", "To", "\u66ff\u6362\u4e3a", "To");
            Add(result, "EZRenamerWindow", "Prefix", "\u524d\u7f00", "Prefix");
            Add(result, "EZRenamerWindow", "Suffix", "\u540e\u7f00", "Suffix");
            Add(result, "EZRenamerWindow", "ShowLog", "\u663e\u793a\u65e5\u5fd7", "Show Log");
            Add(result, "EZRenamerWindow", "Collapse", "\u6298\u53e0\u663e\u793a", "Collapse");
            Add(result, "EZRenamerWindow", "Processing", "\u5904\u7406\u4e2d", "Processing");
            return result;
        }

        private static void Add(Dictionary<EZLanguage, Dictionary<string, string>> languages, string group, string key, string chinese, string english)
        {
            string fullKey = group + "." + key;
            languages[EZLanguage.Chinese][fullKey] = chinese;
            languages[EZLanguage.English][fullKey] = english;
        }
    }
}
