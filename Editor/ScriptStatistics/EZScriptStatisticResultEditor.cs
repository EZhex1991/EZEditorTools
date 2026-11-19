/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-04-24 15:52:28
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;
using static EZhex1991.EZEditorTools.EZScriptStatisticResult;

namespace EZhex1991.EZEditorTools
{
    [CustomEditor(typeof(EZScriptStatisticResult))]
    public class EZScriptStatisticResultEditor : Editor
    {
        private EZScriptStatisticResult result;

        private Vector2 scrollPosition;

        private const float FirstColumnWidth = 180f;
        private const float SecondColumnWidth = 100f;
        private const float LineCountColumnWidth = 60f;
        private const float LastColumnMinWidth = 100f;

        private void OnEnable()
        {
            result = target as EZScriptStatisticResult;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Contributors", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DrawContributorHeader();
            foreach (Contributor contributor in result.contributors)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;
                Rect authorRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, GUILayout.Width(FirstColumnWidth));
                contributor.foldout = EditorGUI.Foldout(authorRect, contributor.foldout, contributor.author, true);
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField(string.Format("{0:P2}", contributor.proportion), GUILayout.Width(SecondColumnWidth));
                EditorGUILayout.LabelField(contributor.lineCount.ToString(), GUILayout.Width(LineCountColumnWidth));
                EditorGUILayout.LabelField(contributor.validLineCount.ToString(), GUILayout.Width(LineCountColumnWidth));
                EditorGUILayout.LabelField(contributor.scriptList.Count.ToString(), GUILayout.MinWidth(LastColumnMinWidth));
                EditorGUILayout.EndHorizontal();

                if (contributor.foldout)
                {
                    DrawScriptHeader();
                    foreach (ScriptInfo script in contributor.scriptList)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.indentLevel++;
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.ObjectField(script.fileObject, typeof(UnityEngine.Object), true, GUILayout.Width(FirstColumnWidth));
                        EditorGUI.EndDisabledGroup();
                        EditorGUI.indentLevel--;
                        EditorGUILayout.TextField(script.createTime, GUILayout.Width(SecondColumnWidth));
                        EditorGUILayout.TextField(script.lineCount.ToString(), GUILayout.Width(LineCountColumnWidth));
                        EditorGUILayout.TextField(script.validLineCount.ToString(), GUILayout.Width(LineCountColumnWidth));
                        EditorGUILayout.TextField(script.filePath, GUILayout.MinWidth(LastColumnMinWidth));
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawContributorHeader()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Author", GUILayout.Width(FirstColumnWidth));
            EditorGUILayout.LabelField("Proportion", GUILayout.Width(SecondColumnWidth));
            EditorGUILayout.LabelField("Lines Total / Valid", GUILayout.Width(LineCountColumnWidth * 2f));
            EditorGUILayout.LabelField("Files", GUILayout.MinWidth(LastColumnMinWidth));
            EditorGUILayout.EndHorizontal();
        }

        private void DrawScriptHeader()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Asset", GUILayout.Width(FirstColumnWidth));
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("Create Time", GUILayout.Width(SecondColumnWidth));
            EditorGUILayout.LabelField("Lines Total / Valid", GUILayout.Width(LineCountColumnWidth * 2f));
            EditorGUILayout.LabelField("File", GUILayout.MinWidth(LastColumnMinWidth));
            EditorGUILayout.EndHorizontal();
        }
    }
}


