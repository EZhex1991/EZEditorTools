/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-03-08 19:42:27
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZEditorTools.Build
{
    [CustomEditor(typeof(EZBundleBuilder))]
    public class EZBundleBuilderEditor : Editor
    {
        private EZBundleBuilder bundleBuilder;

        private SerializedProperty m_BuildTarget;
        private SerializedProperty m_BuildOptions;
        private SerializedProperty m_BuildNumber;
        private SerializedProperty m_OutputDirectory;
        private SerializedProperty m_OutputPattern;
        private SerializedProperty m_ListFileName;
        private SerializedProperty m_ForceRebuild;
        private SerializedProperty m_PreBuildCopyList;
        private SerializedProperty m_BundleList;

        private ReorderableList preBuildCopyList;
        private ReorderableList bundleList;

        void OnEnable()
        {
            bundleBuilder = target as EZBundleBuilder;
            m_BuildTarget = serializedObject.FindProperty("buildTarget");
            m_BuildOptions = serializedObject.FindProperty("buildOptions");
            m_BuildNumber = serializedObject.FindProperty("buildNumber");
            m_OutputDirectory = serializedObject.FindProperty("outputDirectory");
            m_OutputPattern = serializedObject.FindProperty("outputPattern");
            m_ListFileName = serializedObject.FindProperty("listFileName");
            m_ForceRebuild = serializedObject.FindProperty("forceRebuild");
            m_PreBuildCopyList = serializedObject.FindProperty("preBuildCopyList");
            m_BundleList = serializedObject.FindProperty("bundleList");
            preBuildCopyList = new ReorderableList(serializedObject, m_PreBuildCopyList, true, true, true, true)
            {
                drawHeaderCallback = DrawPreBuildCopyListHeader,
                drawElementCallback = DrawPreBuildCopyListElement
            };
            bundleList = new ReorderableList(serializedObject, m_BundleList, true, true, true, true)
            {
                drawHeaderCallback = DrawBundleListHeader,
                drawElementCallback = DrawBundleListElement
            };
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
                    EditorApplication.delayCall += delegate () { bundleBuilder.Execute((BuildTarget)m_BuildTarget.intValue); };
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.Space();
                EZBuilderGUIUtility.DrawQuickBuildButtons(
                    target => EditorApplication.delayCall += delegate () { bundleBuilder.Execute(target); });
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
            DrawOutputProperties();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Build Options", EditorStyles.boldLabel);
            DrawBuildOptions();

            EZEditorGUIUtility.DoLayoutReorderableList(preBuildCopyList, "Pre-build Copy");

            EZEditorGUIUtility.DoLayoutReorderableList(bundleList);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawOutputProperties()
        {
            DrawOutputDirectory(m_OutputDirectory);
            DrawOutputDirectoryPreview();
            EditorGUILayout.PropertyField(m_OutputPattern);
            DrawOutputPatternPreview();
        }

        private void DrawBuildOptions()
        {
            m_BuildOptions.intValue = (int)(BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("Build Options", (BuildAssetBundleOptions)m_BuildOptions.intValue);
            EZEditorGUIUtility.DrawBuildNumberField(m_BuildNumber);
            EditorGUILayout.PropertyField(m_ListFileName);
            EditorGUILayout.PropertyField(m_ForceRebuild);
        }

        private static void DrawOutputDirectory(SerializedProperty property)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Output Directory");
            EZEditorGUIUtility.DrawFolderPathField(EditorGUILayout.GetControlRect(), property);
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
            EditorGUILayout.LabelField(bundleBuilder.GetOutputPattern(previewTarget));
            EditorGUI.indentLevel--;
        }

        private void DrawPreBuildCopyListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, preBuildCopyList);
            rect.y += 1;
            float width = rect.width / 2; float margin = 5;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - margin, rect.height), "Destination Path");
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - margin, rect.height), "Source Path");
        }
        private void DrawPreBuildCopyListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, preBuildCopyList);
            rect.y += 1; rect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty m_CopyInfo = m_PreBuildCopyList.GetArrayElementAtIndex(index);
            SerializedProperty m_DestDirPath = m_CopyInfo.FindPropertyRelative("dstPath");
            SerializedProperty m_SrcDirPath = m_CopyInfo.FindPropertyRelative("srcPath");

            float width = rect.width / 2; float margin = 5;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, rect.height), m_DestDirPath, GUIContent.none);
            rect.x += width;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width - margin, rect.height), m_SrcDirPath, GUIContent.none);
        }

        private void DrawBundleListHeader(Rect rect)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, bundleList);
            rect.y += 1;

            float width = Mathf.Min(100, rect.width / 4); float residue = (rect.width - width * 4) / 10; float margin = 5;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue - margin, rect.height), "Bundle Name");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue - margin, rect.height), "File Pattern");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue - margin, rect.height), "Search Option");
            rect.x += width + residue;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width + residue * 7 - margin, rect.height), "Source");
        }
        private void DrawBundleListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = EZEditorGUIUtility.DrawReorderableListIndex(rect, index, bundleList);
            rect.y += 1; rect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty m_BundleInfo = m_BundleList.GetArrayElementAtIndex(index);
            SerializedProperty m_BundleName = m_BundleInfo.FindPropertyRelative("bundleName");
            SerializedProperty m_FilePattern = m_BundleInfo.FindPropertyRelative("filePattern");
            SerializedProperty m_SearchOption = m_BundleInfo.FindPropertyRelative("searchOption");
            SerializedProperty m_SrcDirPath = m_BundleInfo.FindPropertyRelative("dirPath");

            float width = Mathf.Min(100, rect.width / 4); float residue = (rect.width - width * 4) / 10; float margin = 5;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - margin, rect.height), m_BundleName, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - margin, rect.height), m_FilePattern, GUIContent.none);
            rect.x += width + residue;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, width + residue - margin, rect.height), m_SearchOption, GUIContent.none);
            rect.x += width + residue;
            EZEditorGUIUtility.DrawFolderPathField(new Rect(rect.x, rect.y, width + residue * 7 - margin, rect.height), m_SrcDirPath);
        }

    }
}

