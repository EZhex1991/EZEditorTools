/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-27 17:55:15
 * Organization:    ezhex1991@outlook.com
 * Description:     
 */
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZRegexTester : EditorWindow
    {
        public string inputString = "This is a test";
        public string regexString = "([Tt]\\w*)";

        private float height = EditorGUIUtility.singleLineHeight * 5;

        private void OnEnable()
        {
            EZLocalization.LanguageChanged += OnLanguageChanged;
            OnLanguageChanged();
        }

        private void OnDisable() => EZLocalization.LanguageChanged -= OnLanguageChanged;
        private static string L(string key) => EZLocalization.Get<EZRegexTester>(key);
        private void OnLanguageChanged()
        {
            titleContent = new GUIContent(L("Title"));
            Repaint();
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            EditorGUILayout.LabelField(L("Input"), EditorStyles.boldLabel);
            inputString = EditorGUILayout.TextArea(inputString, GUILayout.MinHeight(height));

            EditorGUILayout.LabelField(L("Regex"), EditorStyles.boldLabel);
            regexString = EditorGUILayout.TextArea(regexString, GUILayout.MinHeight(height));

            EditorGUILayout.LabelField(L("Matches"), EditorStyles.boldLabel);
            try
            {
                if (string.IsNullOrEmpty(regexString)) return;
                foreach (Match match in Regex.Matches(inputString, regexString))
                {
                    EditorGUILayout.LabelField(string.Format(L("Match"), match.Index, match.Value));
                    EditorGUI.indentLevel++;
                    foreach (Group group in match.Groups)
                    {
                        EditorGUILayout.LabelField(string.Format(L("Group"), group.Index, group.Value));
                        EditorGUI.indentLevel++;
                        foreach (Capture capture in group.Captures)
                        {
                            EditorGUILayout.LabelField(string.Format(L("Capture"), capture.Index, capture.Value));
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
            }
            catch (System.ArgumentException ex)
            {
                EditorGUILayout.HelpBox(string.Format(L("InvalidRegex"), ex.Message), MessageType.Error);
            }
        }
    }
}
