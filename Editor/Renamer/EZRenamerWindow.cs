/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-09-02 18:23:42
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZRenamerWindow : EditorWindow
    {
        public enum CaseConversion
        {
            None = 0,
            ToUpper = 1,
            ToLower = 2,
        }

        private CaseConversion caseConversion = CaseConversion.None;
        private string oldValue, newValue;
        private string prefix, suffix;

        private static bool useRegex = false;
        private static bool showLog = false;
        private static bool collapse = false;

        private bool assetListFoldOut = true;
        private List<Object> assetList = new List<Object>();
        private List<string> assetNameList = new List<string>();
        private Dictionary<string, int> assetNameDict = new Dictionary<string, int>();

        private bool sceneObjectListFoldOut = true;
        private List<GameObject> sceneObjectList = new List<GameObject>();
        private List<string> sceneObjectNameList = new List<string>();
        private Dictionary<string, int> sceneObjectNameDict = new Dictionary<string, int>();

        private Vector2 scrollView;

        private void ResetConfigs()
        {
            GUI.FocusControl(null);
            caseConversion = CaseConversion.None;
            oldValue = ""; newValue = "";
            prefix = ""; suffix = "";
            GetObjects();
        }

        private void GetAssets()
        {
            assetList.Clear();
            assetNameList.Clear();
            assetNameDict.Clear();
            foreach (Object asset in Selection.GetFiltered<Object>(SelectionMode.Assets | SelectionMode.TopLevel))
            {
                assetList.Add(asset);
                assetNameList.Add(GetAssetName(asset));
            }
            assetNameList.Sort();
            foreach (string assetName in assetNameList)
            {
                if (assetNameDict.ContainsKey(assetName)) assetNameDict[assetName]++;
                else assetNameDict[assetName] = 1;
            }
        }
        private void GetSceneObjects()
        {
            sceneObjectList.Clear();
            sceneObjectNameList.Clear();
            sceneObjectNameDict.Clear();
            foreach (GameObject go in Selection.GetFiltered<GameObject>(SelectionMode.Editable))
            {
                sceneObjectList.Add(go);
                sceneObjectNameList.Add(go.name);
            }
            sceneObjectNameList.Sort();
            foreach (string sceneObjectName in sceneObjectNameList)
            {
                if (sceneObjectNameDict.ContainsKey(sceneObjectName)) sceneObjectNameDict[sceneObjectName]++;
                else sceneObjectNameDict[sceneObjectName] = 1;
            }
        }
        private void GetObjects()
        {
            GetAssets();
            GetSceneObjects();
            Repaint();
        }

        private string GetNewName(string oldName)
        {
            switch (caseConversion)
            {
                case CaseConversion.None:
                    break;
                case CaseConversion.ToUpper:
                    oldName = oldName.ToUpper();
                    break;
                case CaseConversion.ToLower:
                    oldName = oldName.ToLower();
                    break;
            }
            if (oldValue != "")
            {
                if (useRegex)
                {
                    try
                    {
                        Regex reg = new Regex(oldValue);
                        oldName = reg.Replace(oldName, newValue);
                    }
                    catch
                    {
                        oldName = oldName.Replace(oldValue, newValue);
                    }
                }
                else
                {
                    oldName = oldName.Replace(oldValue, newValue);
                }
            }
            oldName = prefix + oldName + suffix;
            return oldName;
        }
        private string GetAssetName(Object asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            return Path.GetFileNameWithoutExtension(path);
        }

        private void RenameAssets()
        {
            EditorUtility.DisplayProgressBar(EZLocalization.Get<EZRenamerWindow>("Processing"), "", 0);
            int process = 0;
            foreach (Object obj in assetList)
            {
                string oldValue = obj.name;
                string newValue = GetNewName(oldValue);
                if (showLog) Debug.LogFormat("rename -> \t{0}\nto -> \t{1}", oldValue, newValue);
                EditorUtility.DisplayProgressBar(EZLocalization.Get<EZRenamerWindow>("Processing"), oldValue, process++ / assetList.Count);
                string error = AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(obj), newValue);
                if (error != string.Empty)
                {
                    Debug.Log(error);
                }
            }
            EditorUtility.ClearProgressBar();
        }
        private void RenameSceneObjects()
        {
            EditorUtility.DisplayProgressBar(EZLocalization.Get<EZRenamerWindow>("Processing"), "", 0);
            int process = 0;
            foreach (Object obj in sceneObjectList)
            {
                string oldValue = obj.name;
                string newValue = GetNewName(oldValue);
                if (showLog) Debug.LogFormat("rename -> \t{0}\nto -> \t{1}", oldValue, newValue);
                EditorUtility.DisplayProgressBar(EZLocalization.Get<EZRenamerWindow>("Processing"), oldValue, process++ / sceneObjectList.Count);
                // set name won't mark the scene as dirty
                // obj.name = newValue;
                SerializedObject so = new SerializedObject(obj);
                SerializedProperty sp = so.FindProperty("m_Name");
                sp.stringValue = newValue;
                so.ApplyModifiedProperties();
            }
            EditorUtility.ClearProgressBar();
        }

        protected void OnEnable()
        {
            ResetConfigs();
            GetObjects();
        }
        protected void OnSelectionChange()
        {
            GetObjects();
        }
        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            EditorGUILayout.Space();
            if (GUILayout.Button(EZLocalization.Get<EZRenamerWindow>("Reset")))
            {
                assetList.Clear();
                sceneObjectList.Clear();
                ResetConfigs();
            }
            DrawConfig();

            scrollView = EditorGUILayout.BeginScrollView(scrollView);

            EditorGUILayout.Space();
            if (GUILayout.Button(EZLocalization.Get<EZRenamerWindow>("RenameAssets")))
            {
                RenameAssets();
                ResetConfigs();
            }
            if (assetListFoldOut = EditorGUILayout.Foldout(assetListFoldOut, string.Format(EZLocalization.Get<EZRenamerWindow>("AssetList"), assetNameList.Count.ToString("(00)"))))
            {
                if (collapse) DrawNameDict(assetNameDict);
                else DrawNameList(assetNameList);
            }

            EditorGUILayout.Space();
            if (GUILayout.Button(EZLocalization.Get<EZRenamerWindow>("RenameSceneObjects")))
            {
                RenameSceneObjects();
                ResetConfigs();
            }
            if (sceneObjectListFoldOut = EditorGUILayout.Foldout(sceneObjectListFoldOut, string.Format(EZLocalization.Get<EZRenamerWindow>("SceneObjectList"), sceneObjectNameList.Count.ToString("(00)"))))
            {
                if (collapse) DrawNameDict(sceneObjectNameDict);
                else DrawNameList(sceneObjectNameList);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawConfig()
        {
            string[] caseOptions = EZLocalization.GetOptions<EZRenamerWindow>("Case.None", "Case.ToUpper", "Case.ToLower");
            caseConversion = (CaseConversion)EditorGUILayout.Popup(
                EZLocalization.Get<EZRenamerWindow>("CaseConversion"),
                (int)caseConversion,
                caseOptions);
            useRegex = EditorGUILayout.Toggle(EZLocalization.Get<EZRenamerWindow>("UseRegex"), useRegex);
            {
                EditorGUILayout.BeginHorizontal();
                oldValue = EditorGUILayout.TextField(EZLocalization.Get<EZRenamerWindow>("Replace"), oldValue);
                newValue = EditorGUILayout.TextField(EZLocalization.Get<EZRenamerWindow>("To"), newValue);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                prefix = EditorGUILayout.TextField(EZLocalization.Get<EZRenamerWindow>("Prefix"), prefix);
                suffix = EditorGUILayout.TextField(EZLocalization.Get<EZRenamerWindow>("Suffix"), suffix);
                EditorGUILayout.EndHorizontal();
            }
            {
                EditorGUILayout.BeginHorizontal();
                showLog = EditorGUILayout.Toggle(EZLocalization.Get<EZRenamerWindow>("ShowLog"), showLog);
                collapse = EditorGUILayout.Toggle(EZLocalization.Get<EZRenamerWindow>("Collapse"), collapse);
                EditorGUILayout.EndHorizontal();
            }
        }
        private void DrawNameDict(Dictionary<string, int> nameDict)
        {
            foreach (var nameInfo in nameDict)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(nameInfo.Value.ToString(), new GUILayoutOption[] { GUILayout.Width(20), });
                EditorGUILayout.TextField(nameInfo.Key);
                EditorGUILayout.TextField(GetNewName(nameInfo.Key));
                EditorGUILayout.EndHorizontal();
            }
        }
        private void DrawNameList(List<string> nameList)
        {
            for (int i = 0; i < nameList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(i.ToString("00"), new GUILayoutOption[] { GUILayout.Width(20), });
                EditorGUILayout.TextField(nameList[i]);
                EditorGUILayout.TextField(GetNewName(nameList[i]));
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}


