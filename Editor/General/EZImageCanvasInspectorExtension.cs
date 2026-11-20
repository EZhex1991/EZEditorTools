/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-19
 * Organization:    #ORGANIZATION#
 * Description:     Adds a canvas processing button to image inspectors.
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    [InitializeOnLoad]
    internal static class EZImageCanvasInspectorExtension
    {
        static EZImageCanvasInspectorExtension()
        {
            Editor.finishedDefaultHeaderGUI += OnFinishedDefaultHeaderGUI;
        }

        private static void OnFinishedDefaultHeaderGUI(Editor editor)
        {
            Texture2D texture = GetTexture(editor.target);
            if (texture == null) return;

            bool guiEnabled = GUI.enabled;
            GUI.enabled = guiEnabled && EZImageCanvasTool.CanProcessTextureAsset(texture);
            if (GUILayout.Button(EZLocalization.Get("ImageCanvasTool.InspectorProcess")))
            {
                EZImageCanvasTool.ProcessTextureAsset(texture);
            }
            GUI.enabled = guiEnabled;
        }

        private static Texture2D GetTexture(Object target)
        {
            Texture2D texture = target as Texture2D;
            if (texture != null) return texture;

            Sprite sprite = target as Sprite;
            if (sprite != null) return sprite.texture;

            TextureImporter importer = target as TextureImporter;
            if (importer == null) return null;

            texture = AssetDatabase.LoadAssetAtPath<Texture2D>(importer.assetPath);
            if (texture != null) return texture;

            sprite = AssetDatabase.LoadAssetAtPath<Sprite>(importer.assetPath);
            return sprite == null ? null : sprite.texture;
        }
    }
}
