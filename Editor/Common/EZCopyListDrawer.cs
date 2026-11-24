/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-24
 * Organization:    #ORGANIZATION#
 * Description:     Inspector drawer for copy lists and blacklist settings.
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    [CustomPropertyDrawer(typeof(EZCopyList))]
    public class EZCopyListDrawer : PropertyDrawer
    {
        public SerializedProperty m_CopyPaths;
        public EZCopyPathListGUI copyPathsGUI;
        public SerializedProperty m_BlackList;
        public ReorderableList blackList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (m_CopyPaths == null)
            {
                m_CopyPaths = property.FindPropertyRelative("m_CopyPaths");
                copyPathsGUI = new EZCopyPathListGUI(property.serializedObject, m_CopyPaths, "Source Path", "Output Relative Path");
            }
            if (m_BlackList == null)
            {
                m_BlackList = property.FindPropertyRelative("m_BlackList");
                blackList = new ReorderableList(property.serializedObject, m_BlackList)
                {
                    drawHeaderCallback = DrawBlackListHeader,
                    drawElementCallback = DrawBlackListElement,
                };
            }
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, label, EditorStyles.boldLabel);
            copyPathsGUI.DoLayoutList("Copy Paths");
            EZEditorGUIUtility.DoLayoutReorderableList(blackList);
            EditorGUI.EndProperty();
        }

        public void DrawBlackListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, blackList);
            EditorGUI.LabelField(rect, "Black List");
        }
        public void DrawBlackListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, blackList);
            rect.y += 1; rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, m_BlackList.GetArrayElementAtIndex(index), GUIContent.none);
        }
    }
}
