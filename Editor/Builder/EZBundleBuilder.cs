/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-03-06 14:13:28
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools.Build
{
    [CreateAssetMenu(fileName = "EZBundleBuilder", menuName = "EZEditorTools/Bundle Builder", order = (int)EZAssetMenuOrder.BundleBuilder)]
    public class EZBundleBuilder : ScriptableObject
    {
        [AttributeUsage(AttributeTargets.Method)]
        public class OnPreBuildAttribute : Attribute
        {
            public int priority;
        }
        [AttributeUsage(AttributeTargets.Method)]
        public class OnPostBuildAttribute : Attribute
        {
            public int priority;
        }

        protected const char DELIMITER = '|';

        public BuildTarget buildTarget = BuildTarget.NoTarget;
        public BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.DeterministicAssetBundle;
        public int buildNumber;
        public string outputDirectory = "Builds/AssetBundles";
        public string outputPattern = "<BuildTarget>-<BuildNumber>";
        public bool forceRebuild = false;

        [EZNestedEditor]
        public EZCopyList preBuildCopyList;
        [EZNestedEditor]
        public EZCopyList postBuildCopyList;
        public BundleInfo[] bundleList;

        public void Execute(BuildTarget buildTarget)
        {
            string buildPath = GetOutputPath(buildTarget);
            OnPreBuild();
            if (forceRebuild && Directory.Exists(buildPath)) Directory.Delete(buildPath, true);
            Directory.CreateDirectory(buildPath);
            AssetDatabase.Refresh();
            if (preBuildCopyList != null)
            {
                preBuildCopyList.CopyFiles();
            }

            AssetDatabase.Refresh();
            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(buildPath, GetBuildList(), buildOptions, buildTarget);
            OnPostBuild(manifest);
            CreateFileList(buildPath);
            AssetDatabase.Refresh();
            if (postBuildCopyList != null)
            {
                postBuildCopyList.CopyFiles(buildPath);
            }

            Debug.Log("build complete.");
        }

        public string GetOutputPath(BuildTarget buildTarget)
        {
            return EZBuilderPathUtility.CombinePath(outputDirectory, GetOutputPattern(buildTarget));
        }

        public string GetOutputPattern(BuildTarget buildTarget)
        {
            return EZBuilderPathUtility.ResolvePlaceholders(
                outputPattern,
                buildTarget: EZBuilderPathUtility.GetTargetName(buildTarget),
                buildNumber: buildNumber.ToString());
        }

        protected void OnPreBuild()
        {
            foreach (Type type in (from type in EZEditorUtility.GetAllTypes()
                                   where type.IsClass
                                   select type))
            {
                foreach (MethodInfo methods in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (methods.IsDefined(typeof(OnPreBuildAttribute), false))
                    {
                        methods.Invoke(null, null);
                    }
                }
            }
        }
        protected void OnPostBuild(AssetBundleManifest manifest)
        {
            foreach (Type type in (from type in EZEditorUtility.GetAllTypes()
                                   where type.IsClass
                                   select type))
            {
                foreach (MethodInfo methods in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (methods.IsDefined(typeof(OnPostBuildAttribute), false))
                    {
                        methods.Invoke(null, new object[] { manifest });
                    }
                }
            }
        }

        protected AssetBundleBuild[] GetBuildList()
        {
            List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
            foreach (BundleInfo bundleInfo in bundleList)
            {
                if (bundleInfo.bundleName == "") continue;
                if (bundleInfo.filePattern == "") bundleInfo.filePattern = "*.*";
                string dirPath = AssetDatabase.GetAssetPath(bundleInfo.directory);
                if (!AssetDatabase.IsValidFolder(dirPath)) continue;
                string[] files = Directory.GetFiles(dirPath, bundleInfo.filePattern, bundleInfo.searchOption);
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = files[i].Replace('\\', '/');
                }
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = bundleInfo.bundleName;
                build.assetNames = files;
                buildList.Add(build);
            }
            return buildList.ToArray();
        }

        protected static void CreateFileList(string dirPath)
        {
            const string fileListName = "_files.json";
            string listFilePath = Path.Combine(dirPath, fileListName);
            if (File.Exists(listFilePath)) File.Delete(listFilePath);

            List<string> fileList = new List<string>();
            DirTraverse(dirPath, fileList);

            List<FileInfo> fileInfos = new List<FileInfo>();
            foreach (string filePath in fileList)
            {
                if (filePath.EndsWith(".meta") || filePath.EndsWith(".manifest")) continue;
                fileInfos.Add(new FileInfo()
                {
                    fileName = filePath.Substring(dirPath.Length + 1),
                    md5 = GetFileMD5(filePath),
                    size = Mathf.CeilToInt(new System.IO.FileInfo(filePath).Length >> 10)
                });
            }
            File.WriteAllText(listFilePath, JsonUtility.ToJson(new FileInfoCollection()
            {
                files = fileInfos.ToArray()
            }, true));
        }
        protected static void DirTraverse(string dirPath, List<string> fileList)
        {
            if (!Directory.Exists(dirPath)) return;
            string[] files = Directory.GetFiles(dirPath);
            string[] dirs = Directory.GetDirectories(dirPath);
            foreach (string file in files)
            {
                if (file.EndsWith(".meta")) continue;
                fileList.Add(file.Replace('\\', '/'));
            }
            foreach (string dir in dirs)
            {
                DirTraverse(dir, fileList);
            }
        }
        protected static string GetFileMD5(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] md5Data = md5Hasher.ComputeHash(fs);
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < md5Data.Length; i++)
                {
                    sBuilder.Append(md5Data[i].ToString("x2"));
                }
                fs.Close();
                return sBuilder.ToString();
            }
            catch (Exception ex)
            {
                Debug.Log("MD5File() fail, error: " + ex.Message);
                return null;
            }
        }
    }
}

