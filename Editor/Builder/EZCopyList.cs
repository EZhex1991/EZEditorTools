/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-27 16:35:04
 * Organization:    ezhex1991@outlook.com
 * Description:     
 */
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    [CreateAssetMenu(fileName = "EZCopyList", menuName = "EZEditorTools/Copy List", order = (int)EZAssetMenuOrder.CopyList)]
    public class EZCopyList : ScriptableObject
    {
        [SerializeField]
        private EZCopyPath[] m_CopyPaths;
        public EZCopyPath[] copyPaths { get { return m_CopyPaths; } }

        [SerializeField]
        private string[] m_BlackList = new string[] {
            "\\.meta$",
            "\\.tmp$",
            "^~",
        };
        public string[] blackList { get { return m_BlackList; } }

        public void CopyFiles(string destination = null)
        {
            string projectPath = GetProjectPath();
            string destinationRoot = string.IsNullOrEmpty(destination)
                ? projectPath
                : ResolvePath(destination, projectPath);

            for (int i = 0; i < copyPaths.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Copy Files", "", (float)i / copyPaths.Length);
                if (string.IsNullOrEmpty(copyPaths[i].sourcePath) || string.IsNullOrEmpty(copyPaths[i].destinationPath)) continue;

                string src = ResolvePath(copyPaths[i].sourcePath, projectPath);
                string dst = ResolvePath(copyPaths[i].destinationPath, destinationRoot);
                if (File.Exists(src))
                {
                    try
                    {
                        File.Copy(src, dst, true);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                    }
                }
                else if (Directory.Exists(src))
                {
                    Directory.CreateDirectory(dst);
                    string[] files = Directory.GetFiles(src);
                    foreach (string filePath in files)
                    {
                        try
                        {
                            if (IsInBlackList(Path.GetFileName(filePath)))
                            {
                                Debug.LogFormat("CopyList Ignored: {0}", filePath);
                                continue;
                            }
                            string newPath = dst + filePath.Substring(src.Length);
                            Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                            File.Copy(filePath, newPath, true);
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning(e.Message);
                        }
                    }
                }
            }
            EditorUtility.ClearProgressBar();
        }

        private static string ResolvePath(string path, string rootPath)
        {
            return Path.IsPathRooted(path)
                ? Path.GetFullPath(path)
                : Path.GetFullPath(Path.Combine(rootPath, path));
        }

        private static string GetProjectPath()
        {
            return Directory.GetParent(Application.dataPath).FullName;
        }

        public bool IsInBlackList(string fileName)
        {
            for (int i = 0; i < blackList.Length; i++)
            {
                if (string.IsNullOrEmpty(blackList[i])) continue;
                if (Regex.IsMatch(fileName, blackList[i])) return true;
            }
            return false;
        }
    }

}

