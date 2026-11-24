/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-04 16:14:44
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools.Build
{
    [CustomEditor(typeof(EZPlayerBuilder)), CanEditMultipleObjects]
    public class EZPlayerBuilderEditor : Editor
    {
        protected EZPlayerBuilder playerBuilder;

        protected SerializedProperty m_BuildTarget;
        protected SerializedProperty m_BuildOptions;

        protected SerializedProperty m_BundleBuilder;

        protected SerializedProperty m_OutputDirectory;
        protected SerializedProperty m_OutputPattern;
        protected SerializedProperty m_ExeFileName;
        protected SerializedProperty m_Scenes;

        protected SerializedProperty m_CompanyName;
        protected SerializedProperty m_ProductName;
        protected SerializedProperty m_BundleIdentifier;
        protected SerializedProperty m_BundleVersion;
        protected SerializedProperty m_BuildNumber;
        protected SerializedProperty m_Icon;

        protected SerializedProperty m_PreBuildCopyList;
        protected SerializedProperty m_PostBuildCopyList;

        public static class Styles
        {
            public static string Tooltip_Placeholders = "Supported Path Variables: <Date>|<Time>|<CompanyName>|<ProductName>|<BundleIdentifier>|<BundleVersion>|<BuildNumber>|<BuildTarget>";
            public static GUIContent Label_OutputPattern = new GUIContent("Output Pattern", Tooltip_Placeholders);
            public static GUIContent Label_ExeFileName = new GUIContent("EXE File Name", Tooltip_Placeholders);
        }

        protected void OnEnable()
        {
            playerBuilder = target as EZPlayerBuilder;
            m_BuildTarget = serializedObject.FindProperty("buildTarget");
            m_BuildOptions = serializedObject.FindProperty("buildOptions");
            m_BundleBuilder = serializedObject.FindProperty("bundleBuilder");
            m_OutputDirectory = serializedObject.FindProperty("outputDirectory");
            m_OutputPattern = serializedObject.FindProperty("outputPattern");
            m_ExeFileName = serializedObject.FindProperty("exeFileName");
            m_Scenes = serializedObject.FindProperty("scenes");
            m_CompanyName = serializedObject.FindProperty("companyName");
            m_ProductName = serializedObject.FindProperty("productName");
            m_BundleIdentifier = serializedObject.FindProperty("bundleIdentifier");
            m_BundleVersion = serializedObject.FindProperty("bundleVersion");
            m_BuildNumber = serializedObject.FindProperty("buildNumber");
            m_Icon = serializedObject.FindProperty("icon");

            m_PreBuildCopyList = serializedObject.FindProperty("preBuildCopyList");
            m_PostBuildCopyList = serializedObject.FindProperty("postBuildCopyList");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EZEditorGUIUtility.ScriptableObjectTitle(target as ScriptableObject, !serializedObject.isEditingMultipleObjects);

            if (!serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(m_BuildTarget);

                EditorGUI.BeginDisabledGroup((BuildTarget)m_BuildTarget.intValue == BuildTarget.NoTarget);
                if (GUILayout.Button("Build"))
                {
                    playerBuilder.BuildPlayer();
                    GUIUtility.ExitGUI();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.Space();
                EZBuilderGUIUtility.DrawQuickBuildButtons(QuickBuild);
                EditorGUILayout.Space();
            }

            OutputGUI();
            EditorGUILayout.Space();
            BuildOptionsGUI();
            EditorGUILayout.Space();
            PlayerSettingsGUI();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Copy Lists", EditorStyles.boldLabel);
            EZBuilderGUIUtility.DrawPreBuildCopyListHelpBox();
            EditorGUILayout.PropertyField(m_PreBuildCopyList, new GUIContent("Pre-build Copy List"));
            EZBuilderGUIUtility.DrawPostBuildCopyListHelpBox();
            EditorGUILayout.PropertyField(m_PostBuildCopyList, new GUIContent("Post-build Copy List"));

            serializedObject.ApplyModifiedProperties();
        }
        private void QuickBuild(BuildTarget buildTarget)
        {
            m_BuildTarget.intValue = (int)buildTarget;
            serializedObject.ApplyModifiedProperties();
            playerBuilder.BuildPlayer();
            GUIUtility.ExitGUI();
        }

        private void BuildOptionsGUI()
        {
            EditorGUILayout.LabelField("Build Options", EditorStyles.boldLabel);
            m_BuildOptions.intValue = (int)(BuildOptions)EditorGUILayout.EnumFlagsField("Build Options", (BuildOptions)m_BuildOptions.intValue);
            EditorGUILayout.PropertyField(m_BundleBuilder);
            EditorGUILayout.PropertyField(m_Scenes, true);
        }

        private void OutputGUI()
        {
            EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
            DrawOutputDirectory();
            DrawOutputDirectoryPreview();
            EditorGUILayout.PropertyField(m_OutputPattern, Styles.Label_OutputPattern);
            DrawOutputPatternPreview();
            EditorGUILayout.PropertyField(m_ExeFileName, Styles.Label_ExeFileName);
            DrawExeFileNamePreview();
        }

        private void DrawOutputDirectory()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Output Directory");
            EZEditorGUIUtility.DrawFolderPathField(EditorGUILayout.GetControlRect(), m_OutputDirectory);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawOutputDirectoryPreview()
        {
            string fullPath = EZBuilderPathUtility.GetFullPath(m_OutputDirectory.stringValue);
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(fullPath);
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(m_OutputDirectory.stringValue));
            if (GUILayout.Button("Open", GUILayout.Width(50)))
            {
                EZBuilderPathUtility.OpenFolder(m_OutputDirectory.stringValue);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }

        private void DrawOutputPatternPreview()
        {
            BuildTarget previewTarget = EZBuilderPathUtility.GetPreviewTarget((BuildTarget)m_BuildTarget.intValue);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(playerBuilder.GetOutputPattern(previewTarget));
            EditorGUI.indentLevel--;
        }

        private void DrawExeFileNamePreview()
        {
            BuildTarget previewTarget = EZBuilderPathUtility.GetPreviewTarget((BuildTarget)m_BuildTarget.intValue);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField(playerBuilder.GetExeFileName(previewTarget));
            EditorGUI.indentLevel--;
        }

        private void PlayerSettingsGUI()
        {
            EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_CompanyName);
            EditorGUILayout.PropertyField(m_ProductName);
            EditorGUILayout.PropertyField(m_BundleIdentifier);

            EZEditorGUIUtility.DrawBuildNumberField(m_BuildNumber);
            EditorGUILayout.PropertyField(m_BundleVersion);

            EditorGUILayout.PropertyField(m_Icon);
            if (GUILayout.Button("Apply Player Settings"))
            {
                playerBuilder.ConfigPlayerSettings((BuildTarget)m_BuildTarget.intValue);
            }
        }
    }
}


