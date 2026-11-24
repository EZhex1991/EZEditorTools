/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-24
 * Organization:    #ORGANIZATION#
 * Description:     Shared Inspector list GUI for source and destination copy paths.
 */
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZCopyPathListGUI
    {
        private readonly ReorderableList list;
        private readonly string sourceLabel;
        private readonly string destinationLabel;

        public EZCopyPathListGUI(SerializedObject serializedObject, SerializedProperty property, string sourceLabel, string destinationLabel)
        {
            this.sourceLabel = sourceLabel;
            this.destinationLabel = destinationLabel;
            list = new ReorderableList(serializedObject, property, true, true, true, true)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawElement
            };
        }

        public void DoLayoutList(string label)
        {
            EZEditorGUIUtility.DoLayoutReorderableList(list, label);
        }

        private void DrawHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, list);
            rect.y += 1;
            float width = rect.width / 2f;
            const float margin = 5f;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - margin, rect.height), sourceLabel);
            EditorGUI.LabelField(new Rect(rect.x + width, rect.y, width - margin, rect.height), destinationLabel);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, list);
            rect.y += 1;
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, list.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
        }
    }
}
