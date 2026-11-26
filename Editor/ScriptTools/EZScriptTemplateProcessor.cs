/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-07-03 18:44:14
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System.IO;
using System;
using System.Text.RegularExpressions;

namespace EZhex1991.EZEditorTools
{
    public class EZScriptTemplateProcessor : UnityEditor.AssetModificationProcessor
    {
        public enum FileType
        {
            Unknown,
            Template,
            Script,
        }

        private static EZScriptTemplateSettings settings { get { return EZScriptTemplateSettings.Instance; } }

        private static void OnWillCreateAsset(string metaPath)
        {
            string filePath = metaPath.Replace(".meta", "");
            Replace(filePath);
        }

        public static void Replace(string filePath)
        {
            if (GetFileType(filePath) == FileType.Script)
            {
                string content = File.ReadAllText(filePath);
                content = content.Replace("#SCRIPTNAME#", Path.GetFileNameWithoutExtension(filePath));
                content = content.Replace("#CREATETIME#", System.DateTime.Now.ToString(settings.timeFormat));
                foreach (EZScriptTemplateSettings.Pattern pattern in settings.patternList)
                {
                    if (!string.IsNullOrEmpty(pattern.Key) && !string.IsNullOrEmpty(pattern.Value)) content = content.Replace(pattern.Key, pattern.Value);
                }
                File.WriteAllText(filePath, content);
            }
        }

        public static FileType GetFileType(string filePath)
        {
            string fileName = Path.GetFileName(filePath.ToLower());
            string[] info = fileName.Split('-');
            string[] extensions = (settings.fileExtensions ?? "").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (info.Length == 3 && Regex.IsMatch(info[0], @"^[0-9]+$"))
            {
                foreach (string value in extensions)
                {
                    string ext = NormalizeExtension(value);
                    if (!string.IsNullOrEmpty(ext) && info[2].EndsWith(ext + ".txt")) return FileType.Template;
                }
            }
            foreach (string value in extensions)
            {
                string ext = NormalizeExtension(value);
                if (!string.IsNullOrEmpty(ext) && fileName.EndsWith(ext)) return FileType.Script;
            }
            return FileType.Unknown;
        }

        private static string NormalizeExtension(string value)
        {
            string extension = value.Trim().ToLowerInvariant();
            if (extension.Length == 0 || extension.StartsWith(".")) return extension;
            return "." + extension;
        }
    }
}
