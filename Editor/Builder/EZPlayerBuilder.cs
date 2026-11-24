/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-04-02 20:41:29
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;
using UnityEngine;
using System.IO;

using UnityEditor.Build.Reporting;

namespace EZhex1991.EZEditorTools.Build
{
    [CreateAssetMenu(fileName = "EZPlayerBuilder", menuName = "EZEditorTools/Player Builder", order = (int)EZAssetMenuOrder.PlayerBuilder)]
    public class EZPlayerBuilder : ScriptableObject
    {
        public static BuildTargetGroup GetGroup(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.NoTarget: return BuildTargetGroup.Unknown;
                case BuildTarget.StandaloneOSX: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneWindows: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneWindows64: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneLinux64: return BuildTargetGroup.Standalone;
                case BuildTarget.iOS: return BuildTargetGroup.iOS;
                case BuildTarget.Android: return BuildTargetGroup.Android;
                case BuildTarget.WebGL: return BuildTargetGroup.WebGL;
                case BuildTarget.WSAPlayer: return BuildTargetGroup.WSA;
                case BuildTarget.PS4: return BuildTargetGroup.PS4;
                case BuildTarget.XboxOne: return BuildTargetGroup.XboxOne;
                case BuildTarget.tvOS: return BuildTargetGroup.tvOS;
                case BuildTarget.Switch: return BuildTargetGroup.Switch;
                default: return BuildTargetGroup.Unknown;
            }
        }
        public BuildTarget buildTarget = BuildTarget.NoTarget;
        public BuildOptions buildOptions = BuildOptions.ShowBuiltPlayer;

        public EZBundleBuilder bundleBuilder;

        public string outputDirectory = "Builds/Players";
        public string outputPattern = "<BuildTarget>/<ProductName>-<BundleVersion>";
        public string exeFileName = "<ProductName>-<BundleVersion>";
        public SceneAsset[] scenes;

        public string companyName;
        public string productName;
        public string bundleIdentifier;
        public string bundleVersion = "1.0.0";
        public int buildNumber;
        public Texture2D icon;

        [EZNestedEditor]
        public EZCopyList preBuildCopyList;
        [EZNestedEditor]
        public EZCopyList postBuildCopyList;

        public bool CheckTarget(BuildTargetGroup buildGroup)
        {
            BuildTargetGroup selectedGroup = GetGroup(EditorUserBuildSettings.activeBuildTarget);
            if (selectedGroup != buildGroup)
            {
                string message = string.Format("Selected BuildTargetGroup is {0}, Use {1} anyway?", selectedGroup, buildTarget);
                if (!EditorUtility.DisplayDialog("BuildTarget Not Match with selected BuildTargetGroup", message, "Yes", "Cancel"))
                {
                    Debug.Log("Build Canceled");
                    return false;
                }
            }
            return true;
        }
        public void ConfigTargetGroup(BuildTargetGroup buildTargetGroup)
        {
            if (!string.IsNullOrEmpty(companyName))
            {
                PlayerSettings.companyName = companyName;
            }
            if (!string.IsNullOrEmpty(productName))
            {
                PlayerSettings.productName = productName;
            }
            if (!string.IsNullOrEmpty(bundleVersion))
            {
                PlayerSettings.bundleVersion = bundleVersion;
            }

            if (icon != null)
            {
                Texture2D[] icons = PlayerSettings.GetIconsForTargetGroup(buildTargetGroup, IconKind.Any);
                for (int i = 0; i < icons.Length; i++)
                {
                    icons[i] = icon;
                }
                PlayerSettings.SetIconsForTargetGroup(buildTargetGroup, icons, IconKind.Any);
            }

            if (!string.IsNullOrEmpty(bundleIdentifier))
            {
                PlayerSettings.SetApplicationIdentifier(buildTargetGroup, bundleIdentifier);
            }

            switch (buildTargetGroup)
            {
                case BuildTargetGroup.Standalone:
                    PlayerSettings.macOS.buildNumber = buildNumber.ToString();
                    break;
                case BuildTargetGroup.iOS:
                    PlayerSettings.iOS.buildNumber = buildNumber.ToString();
                    break;
                case BuildTargetGroup.Android:
                    PlayerSettings.Android.bundleVersionCode = buildNumber;
                    break;
            }
        }

        public void ConfigPlayerSettings(BuildTarget buildTarget)
        {
            BuildTargetGroup buildGroup = GetGroup(buildTarget);
            if (!CheckTarget(buildGroup)) return;

            ConfigTargetGroup(buildGroup);
        }

        public BuildPlayerOptions GetBuildOptions(string path)
        {
            BuildPlayerOptions options = new BuildPlayerOptions();
            string[] scenePaths = new string[scenes.Length];
            for (int i = 0; i < scenePaths.Length; i++)
            {
                scenePaths[i] = AssetDatabase.GetAssetPath(scenes[i]);
            }
            options.scenes = scenePaths;
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    options.locationPathName = string.Format("{0}/{1}.exe", path, GetExeFileName());
                    break;
                case BuildTarget.StandaloneWindows64:
                    options.locationPathName = string.Format("{0}/{1}.exe", path, GetExeFileName());
                    break;
                case BuildTarget.Android:
                    options.locationPathName = string.Format("{0}.apk", path);
                    break;
                default:
                    options.locationPathName = path;
                    break;
            }
            options.target = buildTarget;
            options.options = buildOptions;
            return options;
        }
        public void BuildPlayer()
        {
            BuildTargetGroup buildGroup = GetGroup(buildTarget);
            if (!CheckTarget(buildGroup)) return;

            if (string.IsNullOrEmpty(outputDirectory))
            {
                outputDirectory = EditorUtility.SaveFolderPanel("Choose Output Folder", "", "");
                if (string.IsNullOrEmpty(outputDirectory)) return;
            }
            string path = GetOutputPath();

            string projectSettingsPath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + "/ProjectSettings/ProjectSettings.asset";
            string oldSettings = File.ReadAllText(projectSettingsPath);

            ConfigTargetGroup(buildGroup);
            if (bundleBuilder != null)
            {
                bundleBuilder.Execute(buildTarget);
            }
            if (preBuildCopyList != null)
            {
                preBuildCopyList.CopyFiles();
            }

            BuildReport report = BuildPipeline.BuildPlayer(GetBuildOptions(path));
            var summary = report.summary;
            switch (summary.result)
            {
                case BuildResult.Failed:
                    Debug.LogError("Build Failed");
                    break;
                case BuildResult.Succeeded:
                    Debug.Log("Build Succeeded");
                    if (postBuildCopyList != null)
                    {
                        postBuildCopyList.CopyFiles(path);
                    }
                    break;
            }
            File.WriteAllText(projectSettingsPath, oldSettings);

        }

        public string HandlePlaceholders(string text)
        {
            return HandlePlaceholders(text, buildTarget);
        }
        public string HandlePlaceholders(string text, BuildTarget target)
        {
            return EZBuilderPathUtility.ResolvePlaceholders(
                text,
                buildTarget: EZBuilderPathUtility.GetTargetName(target),
                buildNumber: buildNumber.ToString(),
                companyName: companyName,
                productName: productName,
                bundleIdentifier: bundleIdentifier,
                bundleVersion: bundleVersion,
                date: DateTime.Now.ToString("yyyyMMdd"),
                time: DateTime.Now.ToString("HHmmss"));
        }
        public string GetOutputPath()
        {
            return EZBuilderPathUtility.CombinePath(outputDirectory, GetOutputPattern(buildTarget));
        }
        public string GetOutputPattern(BuildTarget target)
        {
            return HandlePlaceholders(outputPattern, target);
        }
        public string GetExeFileName()
        {
            return HandlePlaceholders(exeFileName);
        }
        public string GetExeFileName(BuildTarget target)
        {
            return HandlePlaceholders(exeFileName, target);
        }
    }
}


