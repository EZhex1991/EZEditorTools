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

        [MenuItem(Root + "Renamer", false, (int)EZMenuOrder.Renamer)]
        private static void ShowRenamer() => EditorWindow.GetWindow<EZRenamerWindow>("Renamer").Show();

        [MenuItem(Root + "Asset Bundle Manager", false, (int)EZMenuOrder.AssetBundleManager)]
        private static void ShowAssetBundleManager() => EditorWindow.GetWindow<EZBundleManager>("Asset Bundle Manager").Show();

        [MenuItem(Root + "Experimental/Shader Keyword Manager", false, (int)EZMenuOrder.ShaderKeywordManager)]
        private static void ShowShaderKeywordManager() => EditorWindow.GetWindow<EZShaderKeywordManager>("ShaderKeyword Manager").Show();

        [MenuItem(Root + "Guid Generator", false, (int)EZMenuOrder.GuidGenerator)]
        private static void ShowGuidGenerator() => EditorWindow.GetWindow<EZGuidGenerator>("Guid Generator").Show();

        [MenuItem(Root + "Regex Tester", false, (int)EZMenuOrder.RegexTester)]
        private static void ShowRegexTester() => EditorWindow.GetWindow<EZRegexTester>("Regex Tester").Show();

        [MenuItem(Root + "Color Blender", false, (int)EZMenuOrder.ColorBlender)]
        private static void ShowColorBlender() => EditorWindow.GetWindow<EZColorBlender>("Color Blender").Show();

        [MenuItem(Root + "Hierarchy Active Toggle", false, (int)EZMenuOrder.HierarchyActiveToggle)]
        private static void ToggleHierarchyActiveToggle()
        {
            EZEditorSettings.HierarchyToggleEnabled = !EZEditorSettings.HierarchyToggleEnabled;
        }

        [MenuItem(Root + "Hierarchy Active Toggle", true)]
        private static bool ValidateHierarchyActiveToggle()
        {
            Menu.SetChecked(Root + "Hierarchy Active Toggle", EZEditorSettings.HierarchyToggleEnabled);
            return true;
        }

#if UNITY_EDITOR_WIN
        [MenuItem(Root + "PlayerPrefs Editor", false, (int)EZMenuOrder.PlayerPrefsEditor)]
        private static void ShowPlayerPrefsEditor() => EditorWindow.GetWindow<EZPlayerPrefsEditor>("PlayerPrefs Editor").Show();

        [MenuItem(Root + "EditorPrefs Editor", false, (int)EZMenuOrder.EditorPrefsEditor)]
        private static void ShowEditorPrefsEditor() => EditorWindow.GetWindow<EZEditorPrefsEditor>("EditorPrefs Editor").Show();
#endif

        [MenuItem(Root + "Time Panel", false, (int)EZMenuOrder.TimePanel)]
        private static void ShowTimePanel() => EditorWindow.GetWindow<EZTimePanel>(" Time Panel").Show();

        [MenuItem(Root + "Property Path Viewer", false, (int)EZMenuOrder.PropertyPathViewer)]
        private static void ShowPropertyPathViewer() => EditorWindow.GetWindow<EZPropertyPathViewer>("Property Path Viewer").Show();

        [MenuItem(Root + "Asset Reference Viewer", false, (int)EZMenuOrder.AssetReferenceViewer)]
        private static void ShowAssetReferenceViewer() => EditorWindow.GetWindow<EZAssetReferenceViewer>("Asset Reference Viewer").Show();

        [MenuItem(Root + "Font Reference Viewer", false, (int)EZMenuOrder.FontReferenceViewer)]
        private static void ShowFontReferenceViewer() => EditorWindow.GetWindow<EZFontReferenceViewer>("Font Reference Viewer").Show();

        [MenuItem(Root + "Corresponding Object Viewer", false, (int)EZMenuOrder.CorrespondingObjectViewer)]
        private static void ShowCorrespondingObjectViewer() => EditorWindow.GetWindow<EZCorrespondingObjectViewer>("Corresponding Object Viewer").Show();

        [MenuItem(Root + "Hierarchy Diff Checker", false, (int)EZMenuOrder.HierachyDiffChecker)]
        private static void ShowHierarchyDiffChecker() => EditorWindow.GetWindow<EZHierachyDiffChecker>("Hierarchy Diff Checker").Show();

        [MenuItem(Root + "Type Reflection Helper", false, (int)EZMenuOrder.TypeReflectionHelper)]
        private static void ShowTypeReflectionHelper() => EditorWindow.GetWindow<EZTypeReflectionHelper>("Type Reflection Helper").Show();

        [MenuItem(Root + "Image Canvas Tool", false, (int)EZMenuOrder.ImageCanvasTool)]
        private static void ShowImageCanvasTool() => EditorWindow.GetWindow<EZImageCanvasTool>("Image Canvas Tool").Show();

        [MenuItem(Root + "Experimental/Animator Controller Combiner", false, (int)EZMenuOrder.AnimatorControllerCombiner)]
        private static void ShowAnimatorControllerCombiner() => EditorWindow.GetWindow<EZAnimatorControllerCombiner>("Animator Controller Combiner").Show();

        [MenuItem(Root + "Material Optimizer", false, (int)EZMenuOrder.MaterialOptimizer)]
        private static void ShowMaterialOptimizer() => EditorWindow.GetWindow<EZMaterialOptimizer>("Material Optimizer").Show();

        [MenuItem(Root + "Save Assets", false, (int)EZMenuOrder.SaveAssets)]
        private static void SaveAssets() => AssetDatabase.SaveAssets();

        [MenuItem(Root + "Refresh AssetDatabase", false, (int)EZMenuOrder.RefreshAssetDatabase)]
        private static void RefreshAssetDatabase() => AssetDatabase.Refresh();

        [MenuItem(Root + "Open Persistent Folder", false, (int)EZMenuOrder.OpenPersistentFolder)]
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

        [MenuItem(Root + "Clear Persistent Folder", false, (int)EZMenuOrder.ClearPersistentFolder)]
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
    }
}
