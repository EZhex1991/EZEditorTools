/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-24
 * Organization:    #ORGANIZATION#
 * Description:     Inspector editor for copy list assets.
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    [CustomEditor(typeof(EZCopyList))]
    public class EZCopyListEditor : Editor
    {
        private SerializedProperty m_CopyPaths;
        private SerializedProperty m_BlackList;
        private ReorderableList copyPaths;
        private ReorderableList blackList;

        private void OnEnable()
        {
            m_CopyPaths = serializedObject.FindProperty("m_CopyPaths");
            m_BlackList = serializedObject.FindProperty("m_BlackList");
            copyPaths = new ReorderableList(serializedObject, m_CopyPaths, true, true, true, true)
            {
                drawHeaderCallback = DrawCopyPathsHeader,
                drawElementCallback = DrawCopyPathElement,
            };
            blackList = new ReorderableList(serializedObject, m_BlackList)
            {
                drawHeaderCallback = DrawBlackListHeader,
                drawElementCallback = DrawBlackListElement,
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject);
            EZEditorGUIUtility.DoLayoutReorderableList(copyPaths, "Copy Paths");
            EZEditorGUIUtility.DoLayoutReorderableList(blackList);
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCopyPathsHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, copyPaths);
            rect.y += 1;
            float width = rect.width / 2f;
            const float margin = 5f;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - margin, rect.height), "Source Path");
            EditorGUI.LabelField(new Rect(rect.x + width, rect.y, width - margin, rect.height), "Destination Path");
        }

        private void DrawCopyPathElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, copyPaths);
            rect.y += 1;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, copyPaths.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
        }

        private void DrawBlackListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, blackList);
            EditorGUI.LabelField(rect, "Black List");
        }

        private void DrawBlackListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, blackList);
            rect.y += 1;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, m_BlackList.GetArrayElementAtIndex(index), GUIContent.none);
        }
    }
}
