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

            AddEZImageCanvasToolTexts(result);
            AddEZRenamerWindowTexts(result);
            AddEZScriptTemplateManagerTexts(result);
            AddEZPropertyPathViewerTexts(result);
            AddEZHierachyDiffCheckerTexts(result);
            AddEZFontReferenceViewerTexts(result);
            AddEZCorrespondingObjectViewerTexts(result);
            AddEZAssetReferenceViewerTexts(result);
            AddEZMaterialOptimizerTexts(result);
            AddEZPlayerPrefsEditorTexts(result);
            AddEZEditorPrefsEditorTexts(result);
            AddEZBundleManagerTexts(result);
            AddEZGuidGeneratorTexts(result);
            AddEZRegexTesterTexts(result);
            AddEZColorBlenderTexts(result);
            AddEZTimePanelTexts(result);
            AddEZTypeReflectionHelperTexts(result);
            AddEZShaderKeywordManagerTexts(result);
            AddEZAnimatorControllerCombinerTexts(result);
            AddPrefsSharedTexts(result);
            return result;
        }


        private static void AddEZImageCanvasToolTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZImageCanvasTool", "Title", "图片画布工具", "Image Canvas Tool");
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
        }

        private static void AddEZRenamerWindowTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZRenamerWindow", "Title", "批量重命名", "Renamer");
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
        }

        private static void AddEZScriptTemplateManagerTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
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
        }

        private static void AddEZPropertyPathViewerTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZPropertyPathViewer", "Title", "属性路径查看器", "Property Path Viewer");
            Add(result, "EZPropertyPathViewer", "Target", "目标对象", "Target");
            Add(result, "EZPropertyPathViewer", "EnterChildren", "进入子属性", "Enter Children");
            Add(result, "EZPropertyPathViewer", "ShowInvisible", "显示不可见属性", "Show Invisible");
            Add(result, "EZPropertyPathViewer", "Properties", "属性", "Properties");
        }

        private static void AddEZHierachyDiffCheckerTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZHierachyDiffChecker", "Title", "层级差异检查器", "Hierarchy Diff Checker");
            Add(result, "EZHierachyDiffChecker", "Target", "目标层级", "Target Hierarchy");
            Add(result, "EZHierachyDiffChecker", "Source", "来源层级", "Source Hierarchy");
            Add(result, "EZHierachyDiffChecker", "Copy", "从来源复制 TRS", "Copy TRS from Source");
            Add(result, "EZHierachyDiffChecker", "CopyTooltip", "复制根对象及名称匹配的子对象的局部位置、旋转和缩放。", "Copy local position, rotation and scale for the root and matching children.");
        }

        private static void AddEZFontReferenceViewerTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZFontReferenceViewer", "Title", "字体引用查看器", "Font Reference Viewer");
            Add(result, "EZFontReferenceViewer", "Refresh", "刷新", "Refresh");
            Add(result, "EZFontReferenceViewer", "SourceFont", "源字体", "Source Font");
            Add(result, "EZFontReferenceViewer", "ReplacementFont", "替换字体", "Replacement Font");
            Add(result, "EZFontReferenceViewer", "Replace", "替换匹配的字体", "Replace Matching Fonts");
            Add(result, "EZFontReferenceViewer", "TmpUnavailable", "未安装 TextMeshPro，无法查看 TextMeshPro 字体。", "TextMeshPro is not installed.");
            Add(result, "EZFontReferenceViewer", "NoTexts", "未找到场景文本组件。", "No scene text components found.");
            Add(result, "EZFontReferenceViewer", "Object", "文本组件", "Text Component");
            Add(result, "EZFontReferenceViewer", "Font", "字体", "Font");
        }

        private static void AddEZCorrespondingObjectViewerTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZCorrespondingObjectViewer", "Title", "对应物体查看器", "Corresponding Object Viewer");
            Add(result, "EZCorrespondingObjectViewer", "SelectedObject", "选中对象", "Selected Object");
            Add(result, "EZCorrespondingObjectViewer", "PrefabAssetType", "Prefab 资源类型", "Prefab Asset Type");
            Add(result, "EZCorrespondingObjectViewer", "OriginalSource", "原始源对象", "Original Source Object");
            Add(result, "EZCorrespondingObjectViewer", "SourceChain", "源对象链", "Source Object Chain");
            Add(result, "EZCorrespondingObjectViewer", "SourceObject", "源对象", "Source Object");
            Add(result, "EZCorrespondingObjectViewer", "ContainingAsset", "所属资源", "Containing Asset");
        }

        private static void AddEZAssetReferenceViewerTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZAssetReferenceViewer", "Title", "资源引用查看器", "Asset Reference Viewer");
            Add(result, "EZAssetReferenceViewer", "Target", "目标对象", "Target");
            Add(result, "EZAssetReferenceViewer", "RecursiveDependencies", "显示递归依赖", "Show Recursive Dependencies");
            Add(result, "EZAssetReferenceViewer", "Dependencies", "资源依赖", "Asset Dependencies");
            Add(result, "EZAssetReferenceViewer", "SceneReferences", "场景中的引用", "Scene References");
        }

        private static void AddEZMaterialOptimizerTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZMaterialOptimizer", "Title", "材质优化器", "Material Optimizer");
            Add(result, "EZMaterialOptimizer", "NoMaterialSelected", "未选中材质", "No Material Selected");
            Add(result, "EZMaterialOptimizer", "RemoveUnusedProperties", "清理未使用的属性", "Remove Unused Properties");
            Add(result, "EZMaterialOptimizer", "CopyFrom", "源属性名", "Source Property");
            Add(result, "EZMaterialOptimizer", "CopyTo", "目标属性名", "Target Property");
            Add(result, "EZMaterialOptimizer", "CopyProperty", "复制属性", "Copy Property");
        }

        private static void AddEZPlayerPrefsEditorTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZPlayerPrefsEditor", "Title", "PlayerPrefs 编辑器", "PlayerPrefs Editor");
        }

        private static void AddEZEditorPrefsEditorTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZEditorPrefsEditor", "Title", "EditorPrefs 编辑器", "EditorPrefs Editor");
        }

        private static void AddEZAnimatorControllerCombinerTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZAnimatorControllerCombiner", "Title", "动画控制器合并器", "Animator Controller Combiner");
            Add(result, "EZAnimatorControllerCombiner", "Layers", "合并层", "Combine Layers");
            Add(result, "EZAnimatorControllerCombiner", "Parameters", "合并参数", "Combine Parameters");
            Add(result, "EZAnimatorControllerCombiner", "Target", "目标控制器", "Target Controller");
            Add(result, "EZAnimatorControllerCombiner", "Sources", "来源控制器", "Source Controllers");
            Add(result, "EZAnimatorControllerCombiner", "Warning", "操作会直接修改目标控制器，并追加来源控制器的内容。", "This operation modifies the target controller and appends content from the source controllers.");
            Add(result, "EZAnimatorControllerCombiner", "Append", "追加到目标控制器", "Append to Target Controller");
        }

        private static void AddEZShaderKeywordManagerTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZShaderKeywordManager", "Title", "Shader 关键字管理器", "Shader Keyword Manager");
            Add(result, "EZShaderKeywordManager", "AdditionalKeywords", "额外关键字", "Additional Keywords");
            Add(result, "EZShaderKeywordManager", "AdditionalInfo", "粘贴来自 Shader 警告或日志的关键字，以空格分隔。这些关键字会加入材质扫描结果。", "Paste space-separated keywords from shader warnings or logs. These keywords are added to the material scan results.");
            Add(result, "EZShaderKeywordManager", "DisableInfo", "禁用操作会关闭关联材质和全局状态中的关键字，不会删除 Shader 中的关键字定义。", "Disable turns off the keyword on associated materials and globally; it does not delete the keyword definition from shaders.");
            Add(result, "EZShaderKeywordManager", "Scan", "扫描关键字", "Scan Keywords");
            Add(result, "EZShaderKeywordManager", "ScanSection", "扫描结果", "Scan Results");
            Add(result, "EZShaderKeywordManager", "DisplaySection", "列表显示", "List Display");
            Add(result, "EZShaderKeywordManager", "Clear", "清空结果", "Clear Results");
            Add(result, "EZShaderKeywordManager", "Expand", "全部展开", "Expand All");
            Add(result, "EZShaderKeywordManager", "Collapse", "全部收起", "Collapse All");
            Add(result, "EZShaderKeywordManager", "Count", "材质数：{0}", "Materials: {0}");
            Add(result, "EZShaderKeywordManager", "GlobalEnabled", "全局状态已启用：{0}", "Globally Enabled: {0}");
            Add(result, "EZShaderKeywordManager", "Disable", "禁用", "Disable");
        }

        private static void AddEZTypeReflectionHelperTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZTypeReflectionHelper", "Title", "类型反射助手", "Type Reflection Helper");
            Add(result, "EZTypeReflectionHelper", "TypeName", "类型名称", "Type Name");
            Add(result, "EZTypeReflectionHelper", "GetTypeInfo", "获取类型信息", "Get Type Info");
            Add(result, "EZTypeReflectionHelper", "NotQueried", "请输入类型名称并获取类型信息。", "Enter a type name and get type information.");
            Add(result, "EZTypeReflectionHelper", "NotFound", "未找到类型。", "Type not found.");
            Add(result, "EZTypeReflectionHelper", "Unsupported", "当前仅支持类和枚举。", "Only classes and enums are currently supported.");
        }

        private static void AddEZTimePanelTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZTimePanel", "Title", "时间帮助面板", "Time Panel");
            Add(result, "EZTimePanel", "Format", "当前时间格式", "Current Time Format");
            Add(result, "EZTimePanel", "DateTime", "日期时间", "Date Time");
            Add(result, "EZTimePanel", "TicksSeconds", "Ticks（秒）", "Ticks (Seconds)");
            Add(result, "EZTimePanel", "TicksMilliseconds", "Ticks（毫秒）", "Ticks (Milliseconds)");
            Add(result, "EZTimePanel", "UnixSeconds", "Unix 时间戳（秒）", "Unix Timestamp (Seconds)");
            Add(result, "EZTimePanel", "UnixMilliseconds", "Unix 时间戳（毫秒）", "Unix Timestamp (Milliseconds)");
            Add(result, "EZTimePanel", "Parse", "解析", "Parse");
            Add(result, "EZTimePanel", "Now", "填入当前时间", "Use Current Time");
            Add(result, "EZTimePanel", "ParseError", "无法解析日期时间：{0}", "Could not parse date and time: {0}");
        }

        private static void AddEZColorBlenderTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZColorBlender", "Title", "颜色混合器", "Color Blender");
            Add(result, "EZColorBlender", "InputColors", "输入颜色", "Input Colors");
            Add(result, "EZColorBlender", "UseHDR", "使用 HDR", "Use HDR");
            Add(result, "EZColorBlender", "Source", "源颜色", "Source Color");
            Add(result, "EZColorBlender", "Destination", "目标颜色", "Destination Color");
            Add(result, "EZColorBlender", "Results", "结果", "Results");
            Add(result, "EZColorBlender", "HexInfo", "十六进制 RGBA 不能完整表示超出 0–1 范围的 HDR 值。", "Hexadecimal RGBA cannot fully represent HDR values outside the 0–1 range.");
            Add(result, "EZColorBlender", "Add", "相加", "Add");
            Add(result, "EZColorBlender", "Minus", "相减", "Minus");
            Add(result, "EZColorBlender", "Multiply", "正片叠底", "Multiply");
            Add(result, "EZColorBlender", "Divide", "相除", "Divide");
            Add(result, "EZColorBlender", "Opacity", "不透明度", "Opacity");
            Add(result, "EZColorBlender", "Darken", "变暗", "Darken");
            Add(result, "EZColorBlender", "Lighten", "变亮", "Lighten");
            Add(result, "EZColorBlender", "Screen", "滤色", "Screen");
            Add(result, "EZColorBlender", "ColorBurn", "颜色加深", "ColorBurn");
            Add(result, "EZColorBlender", "ColorDodge", "颜色减淡", "ColorDodge");
            Add(result, "EZColorBlender", "LinearBurn", "线性加深", "LinearBurn");
            Add(result, "EZColorBlender", "LinearDodge", "线性减淡", "LinearDodge");
            Add(result, "EZColorBlender", "Overlay", "叠加", "Overlay");
            Add(result, "EZColorBlender", "HardLight", "强光", "HardLight");
            Add(result, "EZColorBlender", "SoftLight", "柔光", "SoftLight");
            Add(result, "EZColorBlender", "VividLight", "亮光", "VividLight");
            Add(result, "EZColorBlender", "LinearLight", "线性光", "LinearLight");
            Add(result, "EZColorBlender", "PinLight", "点光", "PinLight");
            Add(result, "EZColorBlender", "HardMix", "实色混合", "HardMix");
            Add(result, "EZColorBlender", "Difference", "差值", "Difference");
            Add(result, "EZColorBlender", "Exclusion", "排除", "Exclusion");
            Add(result, "EZColorBlender", "HSVBlend_H", "HSV 色相", "HSVBlend_H");
            Add(result, "EZColorBlender", "HSVBlend_S", "HSV 饱和度", "HSVBlend_S");
            Add(result, "EZColorBlender", "HSVBlend_V", "HSV 明度", "HSVBlend_V");
            Add(result, "EZColorBlender", "HSVBlend_HS", "HSV 色相及饱和度", "HSVBlend_HS");
        }

        private static void AddEZRegexTesterTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZRegexTester", "Title", "正则表达式测试", "Regex Tester");
            Add(result, "EZRegexTester", "Input", "输入文本", "Input");
            Add(result, "EZRegexTester", "Regex", "正则表达式", "Regex");
            Add(result, "EZRegexTester", "Matches", "匹配结果", "Matches");
            Add(result, "EZRegexTester", "Match", "匹配 Match：起始位置 {0}，值 {1}", "Match: Index: {0}  Value: {1}");
            Add(result, "EZRegexTester", "Group", "组 Group：起始位置 {0}，值 {1}", "Group: Index: {0}  Value: {1}");
            Add(result, "EZRegexTester", "Capture", "捕获 Capture：起始位置 {0}，值 {1}", "Capture: Index: {0}  Value: {1}");
            Add(result, "EZRegexTester", "InvalidRegex", "正则表达式无效：{0}", "Invalid regular expression: {0}");
        }

        private static void AddEZGuidGeneratorTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZGuidGenerator", "Title", "GUID 生成器", "GUID Generator");
            Add(result, "EZGuidGenerator", "Generate", "生成新 GUID", "Generate New GUIDs");
        }

        private static void AddEZBundleManagerTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            Add(result, "EZBundleManager", "Title", "AssetBundle 分配查看器", "AssetBundle Assignment Viewer");
            Add(result, "EZBundleManager", "AssetDisplay", "资源显示方式", "Asset Display");
            Add(result, "EZBundleManager", "DependencyDisplay", "依赖显示方式", "Dependency Display");
            Add(result, "EZBundleManager", "Object", "对象", "Object");
            Add(result, "EZBundleManager", "Path", "路径", "Path");
            Add(result, "EZBundleManager", "PathAndObject", "路径和对象", "Path And Object");
            Add(result, "EZBundleManager", "DontShow", "不显示", "Don't Show");
            Add(result, "EZBundleManager", "Direct", "直接依赖", "Direct");
            Add(result, "EZBundleManager", "Recursive", "递归依赖", "Recursive");
            Add(result, "EZBundleManager", "Refresh", "刷新", "Refresh");
            Add(result, "EZBundleManager", "RemoveUnused", "移除未使用的 AssetBundle 名称", "Remove Unused AssetBundle Names");
            Add(result, "EZBundleManager", "Dependencies", "依赖", "Dependencies");
        }

        private static void AddPrefsSharedTexts(Dictionary<EZLanguage, Dictionary<string, string>> result)
        {
            foreach (string group in new[] { "EZPlayerPrefsEditor", "EZEditorPrefsEditor" })
            {
                Add(result, group, "Refresh", "刷新", "Refresh");
                Add(result, group, "Delete", "删除", "Delete");
                Add(result, group, "DeleteAll", "全部删除", "Delete All");
                Add(result, group, "UnknownType", "未知类型", "Unknown Type");
                Add(result, group, "UnsupportedApi", "不支持当前 API 兼容级别", "API Compatibility Level Not Supported");
                Add(result, group, "UnsupportedPlatform", "不支持当前平台", "Platform Not Supported");
            }
        }

        private static void Add(Dictionary<EZLanguage, Dictionary<string, string>> languages, string group, string key, string chinese, string english)
        {
            string fullKey = group + "." + key;
            languages[EZLanguage.Chinese][fullKey] = chinese;
            languages[EZLanguage.English][fullKey] = english;
        }
    }
}
