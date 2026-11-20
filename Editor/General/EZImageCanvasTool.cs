/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-19
 * Organization:    #ORGANIZATION#
 * Description:     Checks image resolutions and expands image canvases.
 */
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZImageCanvasTool : EditorWindow
    {
        private enum ImagePivot
        {
            TopLeft,
            TopCenter,
            TopRight,
            MiddleLeft,
            Center,
            MiddleRight,
            BottomLeft,
            BottomCenter,
            BottomRight
        }

        private enum ImageCollectionScope
        {
            SelectedImages,
            SelectedFolder,
            SelectedFolderAndSubfolders
        }

        private class TextureInfo
        {
            public string path;
            public int width;
            public int height;

            public bool IsMultipleOfFour => width % 4 == 0 && height % 4 == 0;
            public bool IsSupported => IsSupportedExtension(path);
        }

        private DefaultAsset folder;
        private string folderPath;
        private ImageCollectionScope collectionScope = ImageCollectionScope.SelectedFolderAndSubfolders;
        private Color fillColor = Color.clear;
        private ImagePivot pivot = ImagePivot.Center;
        private readonly List<TextureInfo> textures = new List<TextureInfo>();
        private Vector2 scrollPosition;

        protected void OnEnable()
        {
            LoadPreferences();
            EZLocalization.LanguageChanged += Repaint;
        }

        protected void OnDisable()
        {
            EZLocalization.LanguageChanged -= Repaint;
        }

        private void LoadPreferences()
        {
            string scopeName = EditorPrefs.GetString(EZEditorPreferenceKeys.ImageCanvasCollectionScope, ImageCollectionScope.SelectedFolderAndSubfolders.ToString());
            ImageCollectionScope savedScope;
            if (System.Enum.TryParse(scopeName, out savedScope) && System.Enum.IsDefined(typeof(ImageCollectionScope), savedScope))
                collectionScope = savedScope;

            string pivotName = EditorPrefs.GetString(EZEditorPreferenceKeys.ImageCanvasPivot, ImagePivot.Center.ToString());
            ImagePivot savedPivot;
            if (System.Enum.TryParse(pivotName, out savedPivot) && System.Enum.IsDefined(typeof(ImagePivot), savedPivot))
                pivot = savedPivot;

            string colorName = EditorPrefs.GetString(EZEditorPreferenceKeys.ImageCanvasFillColor, ColorUtility.ToHtmlStringRGBA(Color.clear));
            Color savedColor;
            if (ColorUtility.TryParseHtmlString("#" + colorName, out savedColor))
                fillColor = savedColor;
        }

        private void SavePreferences()
        {
            EditorPrefs.SetString(EZEditorPreferenceKeys.ImageCanvasCollectionScope, collectionScope.ToString());
            EditorPrefs.SetString(EZEditorPreferenceKeys.ImageCanvasPivot, pivot.ToString());
            EditorPrefs.SetString(EZEditorPreferenceKeys.ImageCanvasFillColor, ColorUtility.ToHtmlStringRGBA(fillColor));
        }

        protected void OnSelectionChange()
        {
            if (collectionScope == ImageCollectionScope.SelectedImages)
            {
                Refresh();
            }
        }

        public static bool CanProcessTextureAsset(Texture2D texture)
        {
            if (texture == null) return false;

            string path = AssetDatabase.GetAssetPath(texture);
            return (texture.width % 4 != 0 || texture.height % 4 != 0) && IsSupportedExtension(path);
        }

        public static void ProcessTextureAsset(Texture2D texture)
        {
            if (!CanProcessTextureAsset(texture)) return;

            TextureInfo info = new TextureInfo
            {
                path = AssetDatabase.GetAssetPath(texture),
                width = texture.width,
                height = texture.height
            };
            ProcessTexture(info, Color.clear, ImagePivot.Center);
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            EditorGUI.BeginChangeCheck();
            string[] scopeOptions = EZLocalization.GetOptions(
                "ImageCanvasTool.Scope.SelectedImages",
                "ImageCanvasTool.Scope.SelectedFolder",
                "ImageCanvasTool.Scope.SelectedFolderAndSubfolders");
            collectionScope = (ImageCollectionScope)EditorGUILayout.Popup(
                EZLocalization.Get("ImageCanvasTool.ImageScope"),
                (int)collectionScope,
                scopeOptions);
            if (collectionScope != ImageCollectionScope.SelectedImages)
            {
                folder = (DefaultAsset)EditorGUILayout.ObjectField(
                    EZLocalization.Get("ImageCanvasTool.Folder"), folder, typeof(DefaultAsset), false);
            }
            fillColor = EditorGUILayout.ColorField(EZLocalization.Get("ImageCanvasTool.FillColor"), fillColor);
            string[] pivotOptions = EZLocalization.GetOptions(
                "ImageCanvasTool.Pivot.TopLeft",
                "ImageCanvasTool.Pivot.TopCenter",
                "ImageCanvasTool.Pivot.TopRight",
                "ImageCanvasTool.Pivot.MiddleLeft",
                "ImageCanvasTool.Pivot.Center",
                "ImageCanvasTool.Pivot.MiddleRight",
                "ImageCanvasTool.Pivot.BottomLeft",
                "ImageCanvasTool.Pivot.BottomCenter",
                "ImageCanvasTool.Pivot.BottomRight");
            pivot = (ImagePivot)EditorGUILayout.Popup(
                EZLocalization.Get("ImageCanvasTool.Pivot"),
                (int)pivot,
                pivotOptions);
            if (EditorGUI.EndChangeCheck())
            {
                SavePreferences();
                Refresh();
            }

            bool processAll = false;
            using (new EditorGUILayout.HorizontalScope())
            {
                bool guiEnabled = GUI.enabled;
                GUI.enabled = guiEnabled && HasCollectionSource();
                if (GUILayout.Button(EZLocalization.Get("ImageCanvasTool.Refresh")))
                {
                    Refresh();
                }

                int invalidCount = textures.Count(info => !info.IsMultipleOfFour);
                GUI.enabled = guiEnabled && invalidCount > 0;
                if (GUILayout.Button(EZLocalization.Get("ImageCanvasTool.ProcessAll")))
                {
                    processAll = true;
                }
                GUI.enabled = guiEnabled;

                EditorGUILayout.LabelField(
                    string.Format(EZLocalization.Get("ImageCanvasTool.ImagesSummary"), textures.Count, invalidCount),
                    EditorStyles.miniLabel);
            }

            if (collectionScope != ImageCollectionScope.SelectedImages && folder != null && !IsValidFolder())
            {
                EditorGUILayout.HelpBox(EZLocalization.Get("ImageCanvasTool.InvalidFolder"), MessageType.Warning);
                return;
            }

            EditorGUILayout.HelpBox(EZLocalization.Get("ImageCanvasTool.ProcessingWarning"), MessageType.Info);

            if (textures.Count == 0)
            {
                EditorGUILayout.HelpBox(GetEmptyMessage(), MessageType.Info);
                return;
            }

            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label(EZLocalization.Get("ImageCanvasTool.Asset"), GUILayout.MinWidth(100));
                GUILayout.Label(EZLocalization.Get("ImageCanvasTool.Width"), GUILayout.Width(60));
                GUILayout.Label(EZLocalization.Get("ImageCanvasTool.Height"), GUILayout.Width(60));
                GUILayout.Label(string.Empty, GUILayout.Width(70));
            }

            TextureInfo textureToProcess = null;
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (TextureInfo info in textures)
            {
                Color contentColor = GUI.contentColor;
                if (!info.IsMultipleOfFour)
                {
                    GUI.contentColor = Color.red;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(info.path, GUILayout.MinWidth(100));
                    EditorGUILayout.LabelField(info.width.ToString(), GUILayout.Width(60));
                    EditorGUILayout.LabelField(info.height.ToString(), GUILayout.Width(60));

                    bool guiEnabled = GUI.enabled;
                    GUI.enabled = guiEnabled && !info.IsMultipleOfFour && info.IsSupported;
                    if (GUILayout.Button(EZLocalization.Get("ImageCanvasTool.Process"), GUILayout.Width(70)))
                    {
                        textureToProcess = info;
                    }
                    GUI.enabled = guiEnabled;
                }

                GUI.contentColor = contentColor;
            }
            EditorGUILayout.EndScrollView();

            if (processAll)
            {
                ProcessAll();
            }
            else if (textureToProcess != null)
            {
                ProcessTexture(textureToProcess, fillColor, pivot);
                Refresh();
            }
        }

        private bool IsValidFolder()
        {
            return !string.IsNullOrEmpty(folderPath) && AssetDatabase.IsValidFolder(folderPath);
        }

        private bool HasCollectionSource()
        {
            return collectionScope == ImageCollectionScope.SelectedImages || IsValidFolder();
        }

        private string GetEmptyMessage()
        {
            return collectionScope == ImageCollectionScope.SelectedImages
                ? EZLocalization.Get("ImageCanvasTool.NoSelectedImages")
                : EZLocalization.Get("ImageCanvasTool.NoFolderImages");
        }

        private void Refresh()
        {
            folderPath = folder == null ? string.Empty : AssetDatabase.GetAssetPath(folder);
            textures.Clear();

            if (collectionScope == ImageCollectionScope.SelectedImages)
            {
                foreach (Object selectedObject in Selection.objects)
                {
                    AddTextureInfo(AssetDatabase.GetAssetPath(selectedObject));
                }
            }
            else if (IsValidFolder())
            {
                HashSet<string> assetPaths = new HashSet<string>();
                AddAssetPaths(AssetDatabase.FindAssets("t:Texture2D", new[] { folderPath }), assetPaths);
                AddAssetPaths(AssetDatabase.FindAssets("t:Sprite", new[] { folderPath }), assetPaths);

                foreach (string path in assetPaths)
                {
                    if (collectionScope == ImageCollectionScope.SelectedFolder && !IsDirectChild(path)) continue;
                    AddTextureInfo(path);
                }
            }

            textures.Sort((left, right) => string.Compare(left.path, right.path, System.StringComparison.OrdinalIgnoreCase));
            Repaint();
        }

        private void AddTextureInfo(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                texture = sprite == null ? null : sprite.texture;
            }
            if (texture == null || textures.Any(info => info.path == path)) return;

            textures.Add(new TextureInfo
            {
                path = path,
                width = texture.width,
                height = texture.height
            });
        }

        private bool IsDirectChild(string path)
        {
            string directory = Path.GetDirectoryName(path).Replace('\\', '/');
            return string.Equals(directory, folderPath, System.StringComparison.OrdinalIgnoreCase);
        }

        private void ProcessAll()
        {
            List<TextureInfo> invalidTextures = textures
                .Where(info => !info.IsMultipleOfFour && info.IsSupported)
                .ToList();

            foreach (TextureInfo info in invalidTextures)
            {
                ProcessTexture(info, fillColor, pivot);
            }

            Refresh();
        }

        private static void ProcessTexture(TextureInfo info, Color fillColor, ImagePivot pivot)
        {
            if (!info.IsSupported) return;

            TextureImporter importer = AssetImporter.GetAtPath(info.path) as TextureImporter;
            bool restoreReadable = importer != null && !importer.isReadable;

            try
            {
                if (restoreReadable)
                {
                    importer.isReadable = true;
                    importer.SaveAndReimport();
                }

                Texture2D source = AssetDatabase.LoadAssetAtPath<Texture2D>(info.path);
                if (source == null)
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(info.path);
                    source = sprite == null ? null : sprite.texture;
                }
                if (source == null) return;

                int width = Mathf.CeilToInt(source.width / 4f) * 4;
                int height = Mathf.CeilToInt(source.height / 4f) * 4;
                if (width == source.width && height == source.height) return;

                Texture2D target = new Texture2D(width, height, TextureFormat.RGBA32, false);
                Color[] fillPixels = new Color[width * height];
                for (int i = 0; i < fillPixels.Length; i++)
                {
                    fillPixels[i] = fillColor;
                }
                target.SetPixels(fillPixels);

                Vector2 normalizedPivot = GetNormalizedPivot(pivot);
                int offsetX = Mathf.RoundToInt((width - source.width) * normalizedPivot.x);
                int offsetY = Mathf.RoundToInt((height - source.height) * normalizedPivot.y);
                target.SetPixels(offsetX, offsetY, source.width, source.height, source.GetPixels());
                target.Apply();

                File.WriteAllBytes(info.path, EncodeTexture(target, info.path));
                DestroyImmediate(target);
                AssetDatabase.ImportAsset(info.path, ImportAssetOptions.ForceUpdate);
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Failed to process image '{info.path}': {exception.Message}");
            }
            finally
            {
                if (restoreReadable)
                {
                    importer = AssetImporter.GetAtPath(info.path) as TextureImporter;
                    if (importer != null)
                    {
                        importer.isReadable = false;
                        importer.SaveAndReimport();
                    }
                }
            }
        }

        private static Vector2 GetNormalizedPivot(ImagePivot imagePivot)
        {
            switch (imagePivot)
            {
                case ImagePivot.TopLeft: return new Vector2(0, 1);
                case ImagePivot.TopCenter: return new Vector2(0.5f, 1);
                case ImagePivot.TopRight: return new Vector2(1, 1);
                case ImagePivot.MiddleLeft: return new Vector2(0, 0.5f);
                case ImagePivot.MiddleRight: return new Vector2(1, 0.5f);
                case ImagePivot.BottomLeft: return new Vector2(0, 0);
                case ImagePivot.BottomCenter: return new Vector2(0.5f, 0);
                case ImagePivot.BottomRight: return new Vector2(1, 0);
                default: return new Vector2(0.5f, 0.5f);
            }
        }

        private static byte[] EncodeTexture(Texture2D texture, string path)
        {
            switch (Path.GetExtension(path).ToLowerInvariant())
            {
                case ".jpg":
                case ".jpeg":
                    return texture.EncodeToJPG(100);
                case ".tga":
                    return texture.EncodeToTGA();
                default:
                    return texture.EncodeToPNG();
            }
        }

        private static bool IsSupportedExtension(string path)
        {
            switch (Path.GetExtension(path).ToLowerInvariant())
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".tga":
                    return true;
                default:
                    return false;
            }
        }

        private static void AddAssetPaths(string[] assetGuids, HashSet<string> assetPaths)
        {
            foreach (string assetGuid in assetGuids)
            {
                assetPaths.Add(AssetDatabase.GUIDToAssetPath(assetGuid));
            }
        }
    }
}
