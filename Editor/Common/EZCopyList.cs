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
    [Serializable]
    public class EZCopyList
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

        public void CopyFiles(string destination)
        {
            for (int i = 0; i < copyPaths.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Copy Files", "", (float)i / copyPaths.Length);
                string src = copyPaths[i].sourcePath;
                string dst = Path.Combine(destination, copyPaths[i].destinationPath);
                if (string.IsNullOrEmpty(src) || string.IsNullOrEmpty(dst)) continue;
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

