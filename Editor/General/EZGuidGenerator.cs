/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-10-08 11:20:03
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZGuidGenerator : EditorWindow
    {
        public System.Guid systemGuid;
        public UnityEditor.GUID unityGuid;

        public void RefreshGuid()
        {
            systemGuid = System.Guid.NewGuid();
            unityGuid = UnityEditor.GUID.Generate();
        }

        protected void OnEnable()
        {
            EZLocalization.LanguageChanged += OnLanguageChanged;
            OnLanguageChanged();
            RefreshGuid();
        }

        protected void OnDisable() => EZLocalization.LanguageChanged -= OnLanguageChanged;

        private void OnLanguageChanged()
        {
            titleContent = new GUIContent(EZLocalization.Get<EZGuidGenerator>("Title"));
            Repaint();
        }

        protected void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);
            if (GUILayout.Button(EZLocalization.Get<EZGuidGenerator>("Generate")))
            {
                RefreshGuid();
            }
            EditorGUILayout.PrefixLabel("System.Guid");
            EditorGUILayout.TextArea(systemGuid.ToString());
            EditorGUILayout.PrefixLabel("UnityEditor.GUID");
            EditorGUILayout.TextArea(unityGuid.ToString());
        }
    }
}
