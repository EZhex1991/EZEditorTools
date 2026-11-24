/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-24
 * Organization:    #ORGANIZATION#
 * Description:     Shared GUI helpers for builder tools.
 */
using System;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools.Build
{
    public static class EZBuilderGUIUtility
    {
        public static void DrawQuickBuildButtons(Action<BuildTarget> onBuild)
        {
            EditorGUILayout.LabelField("Quick Build", EditorStyles.boldLabel);
            DrawQuickBuildButtonRow(
                new string[] { "Android", "iOS" },
                new BuildTarget[] { BuildTarget.Android, BuildTarget.iOS },
                onBuild);
            DrawQuickBuildButtonRow(
                new string[] { "Win32", "Win64", "OSX" },
                new BuildTarget[] { BuildTarget.StandaloneWindows, BuildTarget.StandaloneWindows64, BuildTarget.StandaloneOSX },
                onBuild);
            DrawQuickBuildButtonRow(
                new string[] { "WebGL" },
                new BuildTarget[] { BuildTarget.WebGL },
                onBuild);
        }

        private static void DrawQuickBuildButtonRow(string[] labels, BuildTarget[] buildTargets, Action<BuildTarget> onBuild)
        {
            Rect rowRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            const float buttonSpacing = 2f;
            float buttonWidth = (rowRect.width - buttonSpacing * (labels.Length - 1)) / labels.Length;
            for (int i = 0; i < labels.Length; i++)
            {
                Rect buttonRect = new Rect(rowRect.x + (buttonWidth + buttonSpacing) * i, rowRect.y, buttonWidth, rowRect.height);
                if (GUI.Button(buttonRect, labels[i]))
                {
                    onBuild(buildTargets[i]);
                }
            }
        }
    }
}
