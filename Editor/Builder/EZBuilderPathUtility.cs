/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-24
 * Organization:    #ORGANIZATION#
 * Description:     Shared path utilities for EZEditorTools builders.
 */
using System.IO;
using UnityEditor;

namespace EZhex1991.EZEditorTools.Build
{
    internal static class EZBuilderPathUtility
    {
        public static BuildTarget GetPreviewTarget(BuildTarget target)
        {
            return target == BuildTarget.NoTarget
                ? EditorUserBuildSettings.activeBuildTarget
                : target;
        }

        public static string GetTargetName(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.NoTarget: return "NoTarget";
                case BuildTarget.StandaloneOSX: return "OSX";
                case BuildTarget.StandaloneWindows: return "Win32";
                case BuildTarget.StandaloneWindows64: return "Win64";
                case BuildTarget.StandaloneLinux64: return "Linux64";
                case BuildTarget.iOS: return "iOS";
                case BuildTarget.Android: return "Android";
                case BuildTarget.WebGL: return "WebGL";
                case BuildTarget.WSAPlayer: return "WSA";
                case BuildTarget.PS4: return "PS4";
                case BuildTarget.XboxOne: return "Xbox";
                case BuildTarget.tvOS: return "tvOS";
                case BuildTarget.Switch: return "Switch";
                default: return "Unknown";
            }
        }

        public static string ResolvePlaceholders(
            string path,
            string buildTarget = null,
            string buildNumber = null,
            string companyName = null,
            string productName = null,
            string bundleIdentifier = null,
            string bundleVersion = null,
            string date = null,
            string time = null)
        {
            path = Replace(path, "BuildTarget", buildTarget);
            path = Replace(path, "CompanyName", companyName);
            path = Replace(path, "ProductName", productName);
            path = Replace(path, "BundleIdentifier", bundleIdentifier);
            path = Replace(path, "BundleVersion", bundleVersion);
            path = Replace(path, "BuildNumber", buildNumber);
            path = Replace(path, "Date", date);
            path = Replace(path, "Time", time);
            return path;
        }

        private static string Replace(string path, string name, string value)
        {
            return value == null ? path : path.Replace("<" + name + ">", value);
        }

        public static string GetFullPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            if (Path.IsPathRooted(path)) return Path.GetFullPath(path);

            return Path.GetFullPath(Path.Combine(GetProjectPath(), path));
        }

        public static string GetDisplayPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            if (path.Contains("<") || path.Contains(">"))
            {
                return (GetProjectPath() + Path.DirectorySeparatorChar + path).Replace('\\', '/');
            }
            return GetFullPath(path);
        }

        private static string GetProjectPath()
        {
            return Directory.GetParent(UnityEngine.Application.dataPath).FullName;
        }

        public static string CombinePath(string directory, string pattern)
        {
            if (string.IsNullOrEmpty(directory)) return pattern ?? string.Empty;
            if (string.IsNullOrEmpty(pattern)) return directory;
            return Path.Combine(directory, pattern).Replace('\\', '/');
        }

        public static void OpenFolder(string path)
        {
            string fullPath = GetFullPath(path);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
            EditorUtility.RevealInFinder(fullPath);
        }
    }
}
