/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-03-07 18:22:12
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZScriptTemplateManager : EditorWindow
    {
        private string UnityScriptTemplatesDirPath { get { return EditorApplication.applicationContentsPath + "/Resources/ScriptTemplates/"; } }
        private TextAsset[] newTemplates;
        private string[] allTemplates;

        private Vector2 scrollRect;
        private bool templateListExpanded = true;

        [SerializeField] private EZScriptTemplateSettings settings;
        private SerializedObject serializedObject;
        private SerializedProperty m_TimeFormat;
        private SerializedProperty m_FileExtensions;
        private SerializedProperty m_PatternList;

        public void OnEnable()
        {
            EZLocalization.LanguageChanged += OnLanguageChanged;
            OnLanguageChanged();
            settings = EZScriptTemplateSettings.Instance;
            serializedObject = new SerializedObject(this);
            m_TimeFormat = serializedObject.FindProperty("settings.timeFormat");
            m_FileExtensions = serializedObject.FindProperty("settings.fileExtensions");
            m_PatternList = serializedObject.FindProperty("settings.patternList");
            GetUnityTemplates();
            GetSelectedTemplates();
            Selection.selectionChanged += GetSelectedTemplates;
        }
        public void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);
            serializedObject.Update();

            scrollRect = EditorGUILayout.BeginScrollView(scrollRect);
            DrawAddTemplates();
            DrawTemplateList();
            EditorGUILayout.Space();
            DrawPatternList();
            EditorGUILayout.EndScrollView();

            if (serializedObject.ApplyModifiedProperties()) settings.Save();
        }
        public void OnDisable()
        {
            Selection.selectionChanged -= GetSelectedTemplates;
            EZLocalization.LanguageChanged -= OnLanguageChanged;
        }

        private static string L(string key) => EZLocalization.Get<EZScriptTemplateManager>(key);

        private void OnLanguageChanged()
        {
            titleContent = new GUIContent(L("Title"));
            Repaint();
        }

        protected void DrawTemplateList()
        {
            templateListExpanded = EditorGUILayout.Foldout(templateListExpanded, L("TemplateList"), true, EditorStyles.foldoutHeader);
            if (!templateListExpanded) return;
            for (int i = 0; i < allTemplates.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.SelectableLabel(allTemplates[i], EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                DrawDeleteTemplateButton(allTemplates[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
        protected void DrawPatternList()
        {
            EditorGUILayout.LabelField(L("Patterns"), EditorStyles.boldLabel);
            EditorGUILayout.LabelField("00 #SCRIPTNAME#", "System.IO.Path.GetFileNameWithoutExtension(filePath)");
            EditorGUILayout.PropertyField(m_TimeFormat, new GUIContent("01 #CREATETIME#"));
            EditorGUILayout.PropertyField(m_PatternList, new GUIContent(L("CustomPatterns")), true);
            EditorGUILayout.PropertyField(m_FileExtensions, new GUIContent(L("FileExtensions")));
            if (serializedObject.ApplyModifiedProperties()) settings.Save();

            string[] selectedFiles = Selection.objects
                .Select(AssetDatabase.GetAssetPath)
                .Where(path => !string.IsNullOrEmpty(path) && File.Exists(path)
                    && EZScriptTemplateProcessor.GetFileType(path) == EZScriptTemplateProcessor.FileType.Script)
                .Distinct().ToArray();
            EditorGUI.BeginDisabledGroup(selectedFiles.Length == 0);
            if (GUILayout.Button(L("ApplyPatterns")) &&
                EditorUtility.DisplayDialog(L("ApplyTitle"), string.Format(L("ApplyConfirm"), selectedFiles.Length), L("Apply"), L("Cancel")))
            {
                foreach (string filePath in selectedFiles)
                    EZScriptTemplateProcessor.Replace(filePath);
                AssetDatabase.Refresh();
            }
            EditorGUI.EndDisabledGroup();
        }

        protected void DrawAddTemplates()
        {
            EditorGUILayout.LabelField(L("AddTemplates"), EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(L("TemplateInfo"), MessageType.Info);
            if (newTemplates.Length > 0)
            {
                EditorGUI.indentLevel++;
                Color originalColor = GUI.backgroundColor;
                for (int i = 0; i < newTemplates.Length; i++)
                {
                    GUI.backgroundColor = allTemplates.Contains(newTemplates[i].name + ".txt") ? Color.red : originalColor;
                    EditorGUILayout.ObjectField(newTemplates[i], typeof(TextAsset), false);
                }
                GUI.backgroundColor = originalColor;
                EditorGUI.indentLevel--;
            }
            EditorGUI.BeginDisabledGroup(newTemplates.Length == 0);
            if (GUILayout.Button(L(newTemplates.Length == 0 ? "NoTemplatesSelected" : "AddSelectedTemplates")))
            {
                foreach (TextAsset template in newTemplates)
                {
                    string tmplPath = AssetDatabase.GetAssetPath(template);
                    try
                    {
                        File.Copy(tmplPath, UnityScriptTemplatesDirPath + template.name + ".txt", true);
                        GetUnityTemplates();
                        Debug.Log("Template added: " + template.name + ". You may need to restart Unity to apply changes.");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Failed to add template: " + template.name + "\n" + ex.Message);
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button(L("OpenTemplateFolder")))
            {
                try
                {
                    Application.OpenURL(UnityScriptTemplatesDirPath);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        protected void DrawDeleteTemplateButton(string tplName)
        {
            if (GUILayout.Button(L("Delete"), new GUILayoutOption[] { GUILayout.Width(80), }))
            {
                try
                {
                    File.Delete(UnityScriptTemplatesDirPath + tplName);
                    GetUnityTemplates();
                    Debug.Log("Template deleted: " + tplName + ". Restart Unity to apply changes.");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Failed to delete template: " + tplName + "\n" + ex.Message);
                }
            }
        }

        private void GetSelectedTemplates()
        {
            newTemplates = (from template in Selection.GetFiltered<TextAsset>(SelectionMode.Assets)
                            where EZScriptTemplateProcessor.GetFileType(template.name + ".txt") == EZScriptTemplateProcessor.FileType.Template
                            select template).ToArray();
            Repaint();
        }
        private void GetUnityTemplates()
        {
            allTemplates = (from template in Directory.GetFiles(UnityScriptTemplatesDirPath, "*.txt", SearchOption.TopDirectoryOnly)
                            where EZScriptTemplateProcessor.GetFileType(template) == EZScriptTemplateProcessor.FileType.Template
                            select Path.GetFileName(template))
                            .OrderBy(fileName => int.Parse(fileName.Split('-')[0]))
                            .ToArray();
        }
    }

    [CustomPropertyDrawer(typeof(EZScriptTemplateSettings.Pattern))]
    public class EZScriptPatternDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.IndentedRect(position);
            position.height = EditorGUIUtility.singleLineHeight;
            const float spacing = 5f;
            float width = (position.width - spacing) * 0.5f;
            var keyRect = new Rect(position.x, position.y, width, position.height);
            var valueRect = new Rect(position.x + width + spacing, position.y, width, position.height);
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("Key"), GUIContent.none);
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Value"), GUIContent.none);
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
