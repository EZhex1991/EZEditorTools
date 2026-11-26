/* Author:          ezhex1991@outlook.com
 * CreateTime:      2022-07-01 19:50:32
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZAnimatorControllerCombiner : EditorWindow
    {
        [SerializeField] private bool combineLayers = true;
        [SerializeField] private bool combineParameters = true;
        [SerializeField] private AnimatorController output;
        [SerializeField] private AnimatorController[] elements;

        private SerializedObject settings;

        private static string L(string key) => EZLocalization.Get<EZAnimatorControllerCombiner>(key);

        private void OnEnable()
        {
            settings = new SerializedObject(this);
            settings.FindProperty("elements").isExpanded = true;
            EZLocalization.LanguageChanged += OnLanguageChanged;
            OnLanguageChanged();
        }

        private void OnDisable() => EZLocalization.LanguageChanged -= OnLanguageChanged;

        private void OnLanguageChanged()
        {
            titleContent = new GUIContent(L("Title"));
            Repaint();
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            settings.Update();
            EditorGUILayout.PropertyField(settings.FindProperty("combineLayers"), new GUIContent(L("Layers")));
            EditorGUILayout.PropertyField(settings.FindProperty("combineParameters"), new GUIContent(L("Parameters")));
            EditorGUILayout.PropertyField(settings.FindProperty("output"), new GUIContent(L("Target")));
            EditorGUILayout.PropertyField(settings.FindProperty("elements"), new GUIContent(L("Sources")), true);
            settings.ApplyModifiedProperties();
            EditorGUILayout.HelpBox(L("Warning"), MessageType.Warning);

            if (GUILayout.Button(L("Append")))
            {
                if (Combine())
                {
                    Selection.activeObject = output;
                }
            }
        }

        private bool Combine()
        {
            if (output == null) return false;
            if (combineLayers)
            {
                foreach (AnimatorController controller in elements)
                {
                    foreach (AnimatorControllerLayer layer in controller.layers)
                        output.AddLayer(layer);
                }
            }
            if (combineParameters)
            {
                foreach (AnimatorController controller in elements)
                {
                    foreach (AnimatorControllerParameter parameter in controller.parameters)
                        output.AddParameter(parameter);
                }
            }
            EditorUtility.SetDirty(output);
            return true;
        }
    }
}

