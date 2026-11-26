/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-18
 * Organization:    #ORGANIZATION#
 * Description:     Menu entries for EZEditorTools general tools.
 */
using EZhex1991.EZEditorTools.Build;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    internal static class EZEditorToolsMenuItems
    {
        private const string Root = "EZEditorTools/";

        // SECTION_0
        private const string LanguageRoot = Root + "Language/";
        private const string ChineseLanguageMenuName = "简体中文（仅对窗口生效）";
        private const string EnglishLanguageMenuName = "English (Window UI Only)";

        [MenuItem(LanguageRoot + ChineseLanguageMenuName, false, (int)EZMenuOrder.LanguageChinese)]
        private static void UseChineseLanguage() => EZLocalization.SetLanguage(EZLanguage.Chinese);

        [MenuItem(LanguageRoot + ChineseLanguageMenuName, true)]
        private static bool ValidateChineseLanguage() => SetLanguageMenuChecked(EZLanguage.Chinese);

        [MenuItem(LanguageRoot + EnglishLanguageMenuName, false, (int)EZMenuOrder.LanguageEnglish)]
        private static void UseEnglishLanguage() => EZLocalization.SetLanguage(EZLanguage.English);

        [MenuItem(LanguageRoot + EnglishLanguageMenuName, true)]
        private static bool ValidateEnglishLanguage() => SetLanguageMenuChecked(EZLanguage.English);

        private static bool SetLanguageMenuChecked(EZLanguage language)
        {
            string menuName = language == EZLanguage.Chinese ? ChineseLanguageMenuName : EnglishLanguageMenuName;
            Menu.SetChecked(LanguageRoot + menuName, EZLocalization.Language == language);
            return true;
        }

        // SECTION_1
        [MenuItem(Root + "场景对象快捷开关丨Scene Object Active Toggle", false, (int)EZMenuOrder.HierarchyActiveToggle)]
        private static void ToggleHierarchyActiveToggle()
        {
            EZEditorSettings.HierarchyToggleEnabled = !EZEditorSettings.HierarchyToggleEnabled;
        }

        [MenuItem(Root + "场景对象快捷开关丨Scene Object Active Toggle", true)]
        private static bool ValidateHierarchyActiveToggle()
        {
            Menu.SetChecked(Root + "场景对象快捷开关丨Scene Object Active Toggle", EZEditorSettings.HierarchyToggleEnabled);
            return true;
        }

        [MenuItem(Root + "脚本模板管理器丨Script Template Manager", false, (int)EZMenuOrder.ScriptTemplateManager)]
        private static void ShowScriptTemplateManager() => EditorWindow.GetWindow<EZScriptTemplateManager>().Show();

        // SECTION_2
        [MenuItem(Root + "保存资产修改丨Save Assets", false, (int)EZMenuOrder.SaveAssets)]
        private static void SaveAssets() => AssetDatabase.SaveAssets();

        [MenuItem(Root + "刷新资产目录丨Refresh AssetDatabase", false, (int)EZMenuOrder.RefreshAssetDatabase)]
        private static void RefreshAssetDatabase() => AssetDatabase.Refresh();

        [MenuItem(Root + "打开持久化目录丨Open Persistent Folder", false, (int)EZMenuOrder.OpenPersistentFolder)]
        private static void OpenPersistentFolder()
        {
            try
            {
                Application.OpenURL("file://" + Application.persistentDataPath);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        [MenuItem(Root + "清空持久化目录丨Clear Persistent Folder", false, (int)EZMenuOrder.ClearPersistentFolder)]
        private static void ClearPersistentFolder()
        {
            try
            {
                Directory.Delete(Application.persistentDataPath, true);
                Debug.LogFormat("{0} Cleared", Application.persistentDataPath);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

#if UNITY_EDITOR_WIN
        [MenuItem(Root + "PlayerPrefs编辑器丨PlayerPrefs Editor", false, (int)EZMenuOrder.PlayerPrefsEditor)]
        private static void ShowPlayerPrefsEditor() => EditorWindow.GetWindow<EZPlayerPrefsEditor>().Show();

        [MenuItem(Root + "EditorPrefs编辑器丨EditorPrefs Editor", false, (int)EZMenuOrder.EditorPrefsEditor)]
        private static void ShowEditorPrefsEditor() => EditorWindow.GetWindow<EZEditorPrefsEditor>().Show();
#endif

        // SECTION_3
        [MenuItem(Root + "批量重命名丨Renamer", false, (int)EZMenuOrder.Renamer)]
        private static void ShowRenamer() => EditorWindow.GetWindow<EZRenamerWindow>().Show();

        [MenuItem(Root + "图片画布工具丨Image Canvas Tool", false, (int)EZMenuOrder.ImageCanvasTool)]
        private static void ShowImageCanvasTool() => EditorWindow.GetWindow<EZImageCanvasTool>().Show();

        [MenuItem(Root + "材质优化器丨Material Optimizer", false, (int)EZMenuOrder.MaterialOptimizer)]
        private static void ShowMaterialOptimizer() => EditorWindow.GetWindow<EZMaterialOptimizer>().Show();

        // SECTION_4
        [MenuItem(Root + "资源引用查看器丨Asset Reference Viewer", false, (int)EZMenuOrder.AssetReferenceViewer)]
        private static void ShowAssetReferenceViewer() => EditorWindow.GetWindow<EZAssetReferenceViewer>().Show();

        [MenuItem(Root + "对应物体查看器丨Corresponding Object Viewer", false, (int)EZMenuOrder.CorrespondingObjectViewer)]
        private static void ShowCorrespondingObjectViewer() => EditorWindow.GetWindow<EZCorrespondingObjectViewer>().Show();

        [MenuItem(Root + "字体引用查看器丨Font Reference Viewer", false, (int)EZMenuOrder.FontReferenceViewer)]
        private static void ShowFontReferenceViewer() => EditorWindow.GetWindow<EZFontReferenceViewer>().Show();

        [MenuItem(Root + "层级差异检查器丨Hierarchy Diff Checker", false, (int)EZMenuOrder.HierachyDiffChecker)]
        private static void ShowHierarchyDiffChecker() => EditorWindow.GetWindow<EZHierachyDiffChecker>().Show();

        [MenuItem(Root + "属性路径查看器丨Property Path Viewer", false, (int)EZMenuOrder.PropertyPathViewer)]
        private static void ShowPropertyPathViewer() => EditorWindow.GetWindow<EZPropertyPathViewer>().Show();

        [MenuItem(Root + "AssetBundle分配查看器丨AssetBundle Assignment Viewer", false, (int)EZMenuOrder.AssetBundleManager)]
        private static void ShowAssetBundleManager() => EditorWindow.GetWindow<EZBundleManager>().Show();

        // SECTION_7
        [MenuItem(Root + "Guid生成器丨Guid Generator", false, (int)EZMenuOrder.GuidGenerator)]
        private static void ShowGuidGenerator() => EditorWindow.GetWindow<EZGuidGenerator>().Show();

        [MenuItem(Root + "正则表达式测试丨Regex Tester", false, (int)EZMenuOrder.RegexTester)]
        private static void ShowRegexTester() => EditorWindow.GetWindow<EZRegexTester>().Show();

        [MenuItem(Root + "颜色混合器丨Color Blender", false, (int)EZMenuOrder.ColorBlender)]
        private static void ShowColorBlender() => EditorWindow.GetWindow<EZColorBlender>().Show();

        [MenuItem(Root + "时间帮助面板丨Time Panel", false, (int)EZMenuOrder.TimePanel)]
        private static void ShowTimePanel() => EditorWindow.GetWindow<EZTimePanel>().Show();

        [MenuItem(Root + "类型反射助手丨Type Reflection Helper", false, (int)EZMenuOrder.TypeReflectionHelper)]
        private static void ShowTypeReflectionHelper() => EditorWindow.GetWindow<EZTypeReflectionHelper>().Show();

        // SECTION_9
        [MenuItem(Root + "实验性功能丨Experimental/Shader关键字管理丨Shader Keyword Manager", false, (int)EZMenuOrder.ShaderKeywordManager)]
        private static void ShowShaderKeywordManager() => EditorWindow.GetWindow<EZShaderKeywordManager>().Show();

        [MenuItem(Root + "实验性功能丨Experimental/动画控制器合并器丨Animator Controller Combiner", false, (int)EZMenuOrder.AnimatorControllerCombiner)]
        private static void ShowAnimatorControllerCombiner() => EditorWindow.GetWindow<EZAnimatorControllerCombiner>().Show();
    }
}
