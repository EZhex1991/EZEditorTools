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

            Add(result, "EZImageCanvasTool", "Title", "图片画布工具", "Image Canvas Tool");
            Add(result, "EZRenamerWindow", "Title", "批量重命名", "Renamer");
            Add(result, "EZImageCanvasTool", "ImageScope", "图片范围", "Image Scope");
            Add(result, "EZImageCanvasTool", "Scope.SelectedImages", "选中的所有图片", "Selected Images");
            Add(result, "EZImageCanvasTool", "Scope.SelectedFolder", "选中的目录中的所有图片", "Selected Folder");
            Add(result, "EZImageCanvasTool", "Scope.SelectedFolderAndSubfolders", "选中的目录及子目录中的所有图片", "Selected Folder And Subfolders");
            Add(result, "EZImageCanvasTool", "Folder", "目录", "Folder");
            Add(result, "EZImageCanvasTool", "FillColor", "填充颜色", "Fill Color");
            Add(result, "EZImageCanvasTool", "Pivot", "中心点位置", "Pivot");
            Add(result, "EZImageCanvasTool", "Pivot.TopLeft", "左上", "Top Left");
            Add(result, "EZImageCanvasTool", "Pivot.TopCenter", "上中", "Top Center");
            Add(result, "EZImageCanvasTool", "Pivot.TopRight", "右上", "Top Right");
            Add(result, "EZImageCanvasTool", "Pivot.MiddleLeft", "左中", "Middle Left");
            Add(result, "EZImageCanvasTool", "Pivot.Center", "中心", "Center");
            Add(result, "EZImageCanvasTool", "Pivot.MiddleRight", "右中", "Middle Right");
            Add(result, "EZImageCanvasTool", "Pivot.BottomLeft", "左下", "Bottom Left");
            Add(result, "EZImageCanvasTool", "Pivot.BottomCenter", "下中", "Bottom Center");
            Add(result, "EZImageCanvasTool", "Pivot.BottomRight", "右下", "Bottom Right");
            Add(result, "EZImageCanvasTool", "Refresh", "刷新", "Refresh");
            Add(result, "EZImageCanvasTool", "ProcessAll", "全部处理", "Process All");
            Add(result, "EZImageCanvasTool", "ImagesSummary", "图片：{0}    不符合：{1}", "Images: {0}    Invalid: {1}");
            Add(result, "EZImageCanvasTool", "InvalidFolder", "请选择有效的项目目录。", "Please select a valid project folder.");
            Add(result, "EZImageCanvasTool", "ProcessingWarning", "处理会直接重写支持的图片文件，新的画布尺寸会扩展到最近的 4 的倍数。", "Processing rewrites supported image files in place. The new canvas size is rounded up to the nearest multiple of 4.");
            Add(result, "EZImageCanvasTool", "NoSelectedImages", "Project 窗口中没有选中的图片资源。", "No image assets are selected in the Project window.");
            Add(result, "EZImageCanvasTool", "NoFolderImages", "选中的目录中没有找到图片资源。", "No image assets were found in the selected folder.");
            Add(result, "EZImageCanvasTool", "Asset", "资源", "Asset");
            Add(result, "EZImageCanvasTool", "Width", "宽度", "Width");
            Add(result, "EZImageCanvasTool", "Height", "高度", "Height");
            Add(result, "EZImageCanvasTool", "Process", "处理", "Process");
            Add(result, "EZImageCanvasTool", "InspectorProcess", "扩展画布到 4 的倍数", "Expand Canvas to Multiple of 4");
            Add(result, "EZRenamerWindow", "Reset", "重置", "Reset");
            Add(result, "EZRenamerWindow", "Case.None", "不转换", "None");
            Add(result, "EZRenamerWindow", "Case.ToUpper", "转换为大写", "To Upper");
            Add(result, "EZRenamerWindow", "Case.ToLower", "转换为小写", "To Lower");
            Add(result, "EZRenamerWindow", "RenameAssets", "重命名资源", "Rename Assets");
            Add(result, "EZRenamerWindow", "AssetList", "资源列表 {0}", "Asset List {0}");
            Add(result, "EZRenamerWindow", "RenameSceneObjects", "重命名场景对象", "Rename Scene Objects");
            Add(result, "EZRenamerWindow", "SceneObjectList", "场景对象列表 {0}", "Scene Object List {0}");
            Add(result, "EZRenamerWindow", "CaseConversion", "大小写转换", "Case Conversion");
            Add(result, "EZRenamerWindow", "UseRegex", "使用正则表达式", "Use Regex");
            Add(result, "EZRenamerWindow", "Replace", "替换", "Replace");
            Add(result, "EZRenamerWindow", "To", "替换为", "To");
            Add(result, "EZRenamerWindow", "Prefix", "前缀", "Prefix");
            Add(result, "EZRenamerWindow", "Suffix", "后缀", "Suffix");
            Add(result, "EZRenamerWindow", "ShowLog", "显示日志", "Show Log");
            Add(result, "EZRenamerWindow", "Collapse", "折叠显示", "Collapse");
            Add(result, "EZRenamerWindow", "Processing", "处理中", "Processing");
            Add(result, "EZScriptTemplateManager", "Title", "脚本模板管理器", "Script Template Manager");
            Add(result, "EZScriptTemplateManager", "TemplateList", "模板列表", "Template List");
            Add(result, "EZScriptTemplateManager", "Patterns", "替换规则", "Patterns");
            Add(result, "EZScriptTemplateManager", "CustomPatterns", "自定义规则", "Custom Patterns");
            Add(result, "EZScriptTemplateManager", "FileExtensions", "文件扩展名", "File Extensions");
            Add(result, "EZScriptTemplateManager", "ApplyPatterns", "对选中文件应用规则", "Apply Patterns to Selected Files");
            Add(result, "EZScriptTemplateManager", "ApplyTitle", "应用替换规则", "Apply Patterns");
            Add(result, "EZScriptTemplateManager", "ApplyConfirm", "将替换 {0} 个选中文件中的内容。此操作会修改文件。", "Apply patterns to {0} selected file(s)? This modifies the files.");
            Add(result, "EZScriptTemplateManager", "Apply", "应用", "Apply");
            Add(result, "EZScriptTemplateManager", "Cancel", "取消", "Cancel");
            Add(result, "EZScriptTemplateManager", "AddTemplates", "添加模板", "Add Templates");
            Add(result, "EZScriptTemplateManager", "TemplateInfo", "模板保存在当前 Unity 安装目录中，会影响使用此安装的其他项目。红色条目会覆盖已有模板。\n\n文件名格式：[优先级]-[菜单名称]__[子菜单名称]-[默认名称].[扩展名].txt", "Templates are managed in this Unity installation and affect other projects using it. Red entries will replace existing templates.\n\nFile Name Format: [priority]-[menu text]__[submenu text]-[default name].[ext].txt");
            Add(result, "EZScriptTemplateManager", "NoTemplatesSelected", "未选择模板文件", "No Template Files Selected");
            Add(result, "EZScriptTemplateManager", "AddSelectedTemplates", "添加选中的模板", "Add Selected Templates");
            Add(result, "EZScriptTemplateManager", "OpenTemplateFolder", "打开模板目录", "Open Template Folder");
            Add(result, "EZScriptTemplateManager", "Delete", "删除", "Delete");
            foreach (string group in new[] { "EZPlayerPrefsEditor", "EZEditorPrefsEditor" })
            {
                Add(result, group, "Refresh", "刷新", "Refresh");
                Add(result, group, "Delete", "删除", "Delete");
                Add(result, group, "DeleteAll", "全部删除", "Delete All");
                Add(result, group, "UnknownType", "未知类型", "Unknown Type");
                Add(result, group, "UnsupportedApi", "不支持当前 API 兼容级别", "API Compatibility Level Not Supported");
                Add(result, group, "UnsupportedPlatform", "不支持当前平台", "Platform Not Supported");
            }
            Add(result, "EZCorrespondingObjectViewer", "Title", "对应物体查看器", "Corresponding Object Viewer");
            Add(result, "EZCorrespondingObjectViewer", "SelectedObject", "选中对象", "Selected Object");
            Add(result, "EZCorrespondingObjectViewer", "PrefabAssetType", "Prefab 资源类型", "Prefab Asset Type");
            Add(result, "EZCorrespondingObjectViewer", "OriginalSource", "原始源对象", "Original Source Object");
            Add(result, "EZCorrespondingObjectViewer", "SourceChain", "源对象链", "Source Object Chain");
            Add(result, "EZCorrespondingObjectViewer", "SourceObject", "源对象", "Source Object");
            Add(result, "EZCorrespondingObjectViewer", "ContainingAsset", "所属资源", "Containing Asset");
            Add(result, "EZAssetReferenceViewer", "Title", "资源引用查看器", "Asset Reference Viewer");
            Add(result, "EZAssetReferenceViewer", "Target", "目标", "Target");
            Add(result, "EZAssetReferenceViewer", "RecursiveDependencies", "显示递归依赖", "Show Recursive Dependencies");
            Add(result, "EZAssetReferenceViewer", "Dependencies", "资源依赖", "Asset Dependencies");
            Add(result, "EZAssetReferenceViewer", "SceneReferences", "场景中的引用", "Scene References");
            Add(result, "EZMaterialOptimizer", "Title", "材质优化器", "Material Optimizer");
            Add(result, "EZMaterialOptimizer", "NoMaterialSelected", "未选中材质", "No Material Selected");
            Add(result, "EZMaterialOptimizer", "RemoveUnusedProperties", "清理未使用的属性", "Remove Unused Properties");
            Add(result, "EZMaterialOptimizer", "CopyFrom", "源属性名", "Source Property");
            Add(result, "EZMaterialOptimizer", "CopyTo", "目标属性名", "Target Property");
            Add(result, "EZMaterialOptimizer", "CopyProperty", "复制属性", "Copy Property");
            Add(result, "EZPlayerPrefsEditor", "Title", "PlayerPrefs 编辑器", "PlayerPrefs Editor");
            Add(result, "EZEditorPrefsEditor", "Title", "EditorPrefs 编辑器", "EditorPrefs Editor");
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
