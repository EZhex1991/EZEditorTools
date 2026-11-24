/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-24
 * Organization:    #ORGANIZATION#
 * Description:     Inspector drawer for source and destination copy paths.
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    [CustomPropertyDrawer(typeof(EZCopyPath))]
    public class EZCopyPathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sourcePath = property.FindPropertyRelative("sourcePath");
            SerializedProperty destinationPath = property.FindPropertyRelative("destinationPath");

            const float spacing = 2f;
            float width = (position.width - spacing) / 2f;
            EditorGUI.PropertyField(
                new Rect(position.x, position.y, width, position.height),
                sourcePath,
                GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(position.x + width + spacing, position.y, width, position.height),
                destinationPath,
                GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
