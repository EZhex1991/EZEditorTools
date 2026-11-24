/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-05-24 14:30:01
 * Organization:    ezhex1991@outlook.com
 * Description:     
 */
using System;
using UnityEditor;

namespace EZhex1991.EZEditorTools.Build
{
    public enum AssetsViewOption
    {
        Object = 0,
        Path = 1,
        PathAndObject = 2,
    }
    public enum BundleDependenciesViewOption
    {
        DontShow = 0,
        Direct = 1,
        Recursive = 2,
    }

    public enum BundleBuildMode
    {
        Single = 0,
        DirectSubfolders = 1,
        AllSubfolders = 2,
    }

    [Serializable]
    public class BundleInfo
    {
        public string bundleName = "";
        public string filePattern = "*.*";
        public BundleBuildMode buildMode = BundleBuildMode.Single;
        public DefaultAsset directory;
    }
    [Serializable]
    public class FileInfo
    {
        public string fileName;
        public string md5;
        public int size;
    }

    [Serializable]
    public class FileInfoCollection
    {
        public FileInfo[] files;
    }
}


