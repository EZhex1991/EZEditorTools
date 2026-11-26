/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-14 20:13:43
 * Organization:    #ORGANIZATION#
 * Description:     Views and replaces scene text fonts.
 */
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace EZhex1991.EZEditorTools
{
    public class EZFontReferenceViewer : EditorWindow
    {
        private Text[] texts = Array.Empty<Text>();
        private Component[] tmpTexts = Array.Empty<Component>();
        private Type tmpTextType;
        private Type tmpFontType;
        private Font sourceFont;
        private Font replacementFont;
        private Object sourceTmpFont;
        private Object replacementTmpFont;
        private Vector2 scrollPosition;

        private static string L(string key) => EZLocalization.Get<EZFontReferenceViewer>(key);

        private void OnEnable()
        {
            tmpTextType = Type.GetType("TMPro.TMP_Text, Unity.TextMeshPro");
            tmpFontType = Type.GetType("TMPro.TMP_FontAsset, Unity.TextMeshPro");
            EZLocalization.LanguageChanged += OnLanguageChanged;
            OnLanguageChanged();
            Refresh();
        }

        private void OnDisable() => EZLocalization.LanguageChanged -= OnLanguageChanged;

        private void OnLanguageChanged()
        {
            titleContent = new GUIContent(L("Title"));
            Repaint();
        }

        private void Refresh()
        {
            texts = Resources.FindObjectsOfTypeAll<Text>().Where(IsSceneComponent).ToArray();
            tmpTexts = tmpTextType == null ? Array.Empty<Component>() :
                Resources.FindObjectsOfTypeAll(tmpTextType).OfType<Component>().Where(IsSceneComponent).ToArray();
            Repaint();
        }

        private static bool IsSceneComponent(Component component)
        {
            return component != null && !EditorUtility.IsPersistent(component) && component.gameObject.scene.IsValid()
                && component.gameObject.scene.isLoaded && (component.hideFlags & HideFlags.HideAndDontSave) == 0;
        }

        private void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);
            if (GUILayout.Button(L("Refresh"))) Refresh();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.LabelField("UGUI Text", EditorStyles.boldLabel);
            sourceFont = (Font)EditorGUILayout.ObjectField(L("SourceFont"), sourceFont, typeof(Font), false);
            replacementFont = (Font)EditorGUILayout.ObjectField(L("ReplacementFont"), replacementFont, typeof(Font), false);
            using (new EditorGUI.DisabledScope(sourceFont == null || replacementFont == null || sourceFont == replacementFont))
            {
                if (GUILayout.Button(L("Replace"))) ReplaceFonts(texts, sourceFont, replacementFont, "m_FontData.m_Font");
            }
            DrawTexts(texts, "m_FontData.m_Font", typeof(Font));

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("TextMeshPro", EditorStyles.boldLabel);
            if (tmpTextType == null || tmpFontType == null)
            {
                EditorGUILayout.HelpBox(L("TmpUnavailable"), MessageType.Info);
            }
            else
            {
                sourceTmpFont = EditorGUILayout.ObjectField(L("SourceFont"), sourceTmpFont, tmpFontType, false);
                replacementTmpFont = EditorGUILayout.ObjectField(L("ReplacementFont"), replacementTmpFont, tmpFontType, false);
                using (new EditorGUI.DisabledScope(sourceTmpFont == null || replacementTmpFont == null || sourceTmpFont == replacementTmpFont))
                {
                    if (GUILayout.Button(L("Replace"))) ReplaceFonts(tmpTexts, sourceTmpFont, replacementTmpFont, "m_fontAsset");
                }
                DrawTexts(tmpTexts, "m_fontAsset", tmpFontType);
            }
            EditorGUILayout.EndScrollView();
        }

        private static void DrawTexts<T>(T[] components, string propertyPath, Type fontType) where T : Component
        {
            if (components.Length == 0)
            {
                EditorGUILayout.HelpBox(L("NoTexts"), MessageType.Info);
                return;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(L("Object"), EditorStyles.boldLabel);
            EditorGUILayout.LabelField(L("Font"), EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            foreach (T component in components)
            {
                if (component == null) continue;
                SerializedObject serialized = new SerializedObject(component);
                SerializedProperty font = serialized.FindProperty(propertyPath);
                if (font == null) continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField(component, typeof(Component), true);
                EditorGUI.BeginChangeCheck();
                Object selected = EditorGUILayout.ObjectField(font.objectReferenceValue, fontType, false);
                if (EditorGUI.EndChangeCheck())
                {
                    SetFont(component, selected, serialized, font, propertyPath);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private static void ReplaceFonts<T>(T[] components, Object source, Object replacement, string propertyPath) where T : Component
        {
            foreach (T component in components)
            {
                if (component == null) continue;
                SerializedObject serialized = new SerializedObject(component);
                SerializedProperty font = serialized.FindProperty(propertyPath);
                if (font == null || font.objectReferenceValue != source) continue;
                SetFont(component, replacement, serialized, font, propertyPath);
            }
        }

        private static void SetFont(Component component, Object value, SerializedObject serialized, SerializedProperty font, string propertyPath)
        {
            if (propertyPath == "m_fontAsset")
            {
                var fontProperty = component.GetType().GetProperty("font");
                if (fontProperty == null || !fontProperty.CanWrite) return;
                Undo.RecordObject(component, "Change Text Font");
                fontProperty.SetValue(component, value, null);
                PrefabUtility.RecordPrefabInstancePropertyModifications(component);
                EditorUtility.SetDirty(component);
            }
            else
            {
                font.objectReferenceValue = value;
                serialized.ApplyModifiedProperties();
            }
            EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
        }
    }
}
