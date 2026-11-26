/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-06 17:53:16
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZCorrespondingObjectViewer : EditorWindow
    {
        private Object target;

        private PrefabAssetType prefabAssetType;
        private Object originalObject;
        private List<Object> correspondingObjects = new List<Object>();
        private List<Object> correspondingAssets = new List<Object>();

        private bool correspondingObjectsFoldout = true;

        protected void Refresh()
        {
            prefabAssetType = PrefabAssetType.NotAPrefab;
            originalObject = null;
            correspondingObjects.Clear();
            correspondingAssets.Clear();

            target = Selection.activeObject;
            if (target != null)
            {
                prefabAssetType = PrefabUtility.GetPrefabAssetType(target);
                originalObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(target);
                Object source = PrefabUtility.GetCorrespondingObjectFromSource(target);
                while (source != null)
                {
                    correspondingObjects.Add(source);
                    correspondingAssets.Add(AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GetAssetPath(source)));
                    source = PrefabUtility.GetCorrespondingObjectFromSource(source);
                }
            }
            Repaint();
        }

        private void OnEnable()
        {
            EZLocalization.LanguageChanged += OnLanguageChanged;
            OnLanguageChanged();
            Refresh();
        }
        private void OnDisable()
        {
            EZLocalization.LanguageChanged -= OnLanguageChanged;
        }
        private static string L(string key) => EZLocalization.Get<EZCorrespondingObjectViewer>(key);
        private void OnLanguageChanged()
        {
            titleContent = new GUIContent(L("Title"));
            Repaint();
        }
        private void OnSelectionChange()
        {
            Refresh();
        }
        private void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);
            EditorGUILayout.ObjectField(L("SelectedObject"), target, typeof(Object), false);

            EditorGUILayout.EnumPopup(L("PrefabAssetType"), prefabAssetType);
            EditorGUILayout.ObjectField(L("OriginalSource"), originalObject, typeof(Object), false);
            correspondingObjectsFoldout = EditorGUILayout.Foldout(correspondingObjectsFoldout, L("SourceChain"));
            if (correspondingObjectsFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(30);
                EditorGUILayout.LabelField(L("SourceObject"));
                EditorGUILayout.LabelField(L("ContainingAsset"));
                EditorGUILayout.EndHorizontal();
                for (int i = 0; i < correspondingObjects.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(i.ToString("00"), GUILayout.Width(26));
                    EditorGUILayout.ObjectField(correspondingObjects[i], typeof(Object), false);
                    EditorGUILayout.ObjectField(correspondingAssets[i], typeof(Object), false);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}

