/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-20
 * Organization:    #ORGANIZATION#
 * Description:     Provides editor preferences and Hierarchy extensions.
 */
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    [InitializeOnLoad]
    public static class EZEditorSettings
    {
        public static bool HierarchyToggleEnabled
        {
            get { return EditorPrefs.GetBool(EZEditorPreferenceKeys.HierarchyToggleEnabled, false); }
            set
            {
                EditorPrefs.SetBool(EZEditorPreferenceKeys.HierarchyToggleEnabled, value);
                EditorApplication.RepaintHierarchyWindow();
            }
        }

        static EZEditorSettings()
        {
            EditorApplication.hierarchyWindowItemOnGUI += DrawActiveToggleOnHierarchy;
        }

        private static void DrawActiveToggleOnHierarchy(int instanceID, Rect selectionRect)
        {
            if (!HierarchyToggleEnabled) return;

            Object item = EditorUtility.InstanceIDToObject(instanceID);
            GameObject gameObject = item as GameObject;
            if (gameObject == null) return;

            Rect activeRect = new Rect(selectionRect.x - 27, selectionRect.y, selectionRect.height, selectionRect.height);
            EditorGUI.BeginChangeCheck();
            bool active = EditorGUI.Toggle(activeRect, gameObject.activeSelf);
            if (EditorGUI.EndChangeCheck())
            {
                SetActive(gameObject, active);
            }
        }

        private static void SetActive(GameObject gameObject, bool active)
        {
            GameObject[] selections = Selection.GetFiltered<GameObject>(SelectionMode.Editable | SelectionMode.ExcludePrefab);
            if (selections.Contains(gameObject))
            {
                Undo.RecordObjects(selections, "Set Active");
                for (int i = 0; i < selections.Length; i++)
                {
                    selections[i].SetActive(active);
                }
            }
            else
            {
                Undo.RecordObject(gameObject, "Set Active");
                gameObject.SetActive(active);
            }
        }
    }
}
