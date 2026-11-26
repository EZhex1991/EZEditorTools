/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-20
 * Organization:    #ORGANIZATION#
 * Description:     Views, edits, and deletes EditorPrefs data.
 */
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZEditorPrefsEditor : EditorWindow
    {
        private enum ValueType { String = 0, Int = 1, Float = 2 }

        [Serializable]
        private struct KeyTypePair
        {
            public string key;
            public ValueType type;

            public KeyTypePair(string key, ValueType type)
            {
                this.key = key;
                this.type = type;
            }
        }

        private readonly List<KeyTypePair> pairList = new List<KeyTypePair>();
        private Vector2 scrollPosition;

        private void GetPairs()
        {
            pairList.Clear();

#if UNITY_EDITOR_WIN && !NET_STANDARD_2_0
            Microsoft.Win32.RegistryKey registryKey = OpenEditorPrefsKey();
            if (registryKey != null)
            {
                HashSet<string> keys = new HashSet<string>();
                string[] valueNames = registryKey.GetValueNames();
                for (int i = 0; i < valueNames.Length; i++)
                {
                    string valueName = valueNames[i];
                    object value = registryKey.GetValue(valueName);
                    if (value == null) continue;

                    string key = GetKey(valueName);
                    if (string.IsNullOrEmpty(key) || !keys.Add(key)) continue;

                    Type type = value.GetType();
                    if (type == typeof(byte[]) || type == typeof(string))
                    {
                        pairList.Add(new KeyTypePair(key, ValueType.String));
                    }
                    else if (type == typeof(int))
                    {
                        if (IsKnownIntKey(key))
                            pairList.Add(new KeyTypePair(key, ValueType.Int));
                        else if (EditorPrefs.GetInt(key, -1) == -1 && EditorPrefs.GetInt(key, 0) == 0)
                            pairList.Add(new KeyTypePair(key, ValueType.Float));
                        else
                            pairList.Add(new KeyTypePair(key, ValueType.Int));
                    }
                }

                registryKey.Close();
            }
#endif

            // EditorPrefs does not expose an API for enumerating keys. Keep
            // known EZEditorTools keys visible even if their registry value
            // format is not recognized by the scanner.
            AddKnownKey(EZEditorPreferenceKeys.Language, ValueType.Int);
            AddKnownKey(EZEditorPreferenceKeys.HierarchyToggleEnabled, ValueType.Int);
            AddKnownKey(EZEditorPreferenceKeys.ImageCanvasCollectionScope, ValueType.String);
            AddKnownKey(EZEditorPreferenceKeys.ImageCanvasFillColor, ValueType.String);
            AddKnownKey(EZEditorPreferenceKeys.ImageCanvasPivot, ValueType.String);
            AddKnownKey(EZEditorPreferenceKeys.AssetReferenceDependenciesRecursive, ValueType.Int);
            AddKnownKey(EZEditorPreferenceKeys.MaterialPropertyName1, ValueType.String);
            AddKnownKey(EZEditorPreferenceKeys.MaterialPropertyName2, ValueType.String);
        }

        private void AddKnownKey(string key, ValueType type)
        {
            if (EditorPrefs.HasKey(key) && !ContainsKey(key))
                pairList.Add(new KeyTypePair(key, type));
        }

        private static bool IsKnownIntKey(string key)
        {
            return key == EZEditorPreferenceKeys.Language || key == EZEditorPreferenceKeys.HierarchyToggleEnabled || key == EZEditorPreferenceKeys.AssetReferenceDependenciesRecursive;
        }

        private bool ContainsKey(string key)
        {
            for (int i = 0; i < pairList.Count; i++)
            {
                if (pairList[i].key == key) return true;
            }
            return false;
        }

#if UNITY_EDITOR_WIN && !NET_STANDARD_2_0
        private static Microsoft.Win32.RegistryKey OpenEditorPrefsKey()
        {
            Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey registryKey = currentUser.OpenSubKey(
                "Software\\Unity Technologies\\UnityEditor 5.x");
            if (registryKey != null) return registryKey;

            // Fallback for older Unity versions.
            return currentUser.OpenSubKey("Software\\Unity Technologies\\UnityEditor");
        }
#endif

        private static string GetKey(string valueName)
        {
            int suffixIndex = valueName.LastIndexOf("_h", StringComparison.Ordinal);
            return suffixIndex > 0 ? valueName.Substring(0, suffixIndex) : valueName;
        }

        private void Refresh()
        {
            GetPairs();
        }

        protected void OnEnable()
        {
            EZLocalization.LanguageChanged += OnLanguageChanged;
            OnLanguageChanged();
            Refresh();
        }

        protected void OnDisable()
        {
            EZLocalization.LanguageChanged -= OnLanguageChanged;
        }

        private static string L(string key) => EZLocalization.Get<EZEditorPrefsEditor>(key);

        private void OnLanguageChanged()
        {
            titleContent = new GUIContent(L("Title"));
            Repaint();
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

#if UNITY_EDITOR_WIN && NET_STANDARD_2_0
            EditorGUILayout.HelpBox(L("UnsupportedApi"), MessageType.Error);
#elif UNITY_EDITOR_WIN
            if (GUILayout.Button(L("Refresh")))
            {
                Refresh();
            }

            EditorGUILayout.Space();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < pairList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                string key = pairList[i].key;
                ValueType type = pairList[i].type;
                if (type == ValueType.String)
                {
                    EditorGUILayout.LabelField("string", GUILayout.Width(40));
                    string input = EditorGUILayout.DelayedTextField(key, EditorPrefs.GetString(key));
                    if (input != EditorPrefs.GetString(key))
                        EditorPrefs.SetString(key, input);
                }
                else if (type == ValueType.Int)
                {
                    EditorGUILayout.LabelField("int", GUILayout.Width(40));
                    int input = EditorGUILayout.DelayedIntField(key, EditorPrefs.GetInt(key));
                    if (input != EditorPrefs.GetInt(key))
                        EditorPrefs.SetInt(key, input);
                }
                else if (type == ValueType.Float)
                {
                    EditorGUILayout.LabelField("float", GUILayout.Width(40));
                    float input = EditorGUILayout.DelayedFloatField(key, EditorPrefs.GetFloat(key));
                    if (input != EditorPrefs.GetFloat(key))
                        EditorPrefs.SetFloat(key, input);
                }

                if (GUILayout.Button(L("Delete"), GUILayout.Width(80)))
                {
                    EditorPrefs.DeleteKey(key);
                    Refresh();
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button(L("DeleteAll")))
            {
                EditorPrefs.DeleteAll();
                Refresh();
            }
            EditorGUILayout.EndScrollView();
#else
            EditorGUILayout.HelpBox(L("UnsupportedPlatform"), MessageType.Error);
#endif
        }
    }
}
