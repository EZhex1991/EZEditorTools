/* Author:          ezhex1991@outlook.com
 * CreateTime:      2016-08-08 10:49:31
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    [Serializable]
    public class EZScriptTemplateSettings
    {
        private const string PreferenceKey = "EZhex1991.EZEditorTools.ScriptTemplateSettings";
        private static EZScriptTemplateSettings instance;
        public static EZScriptTemplateSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EZScriptTemplateSettings();
                    string json = EditorPrefs.GetString(PreferenceKey, "");
                    if (!string.IsNullOrEmpty(json)) JsonUtility.FromJsonOverwrite(json, instance);
                }
                return instance;
            }
        }

        public void Save()
        {
            instance = this;
            EditorPrefs.SetString(PreferenceKey, JsonUtility.ToJson(this));
        }

        public string timeFormat = "yyyy-MM-dd HH:mm:ss";
        public string fileExtensions = ".cs, .lua, .txt, .shader, .cginc, .uxml, .uss, .asmdef";

        [Serializable]
        public class Pattern
        {
            public string Key = "";
            public string Value = "";
            public Pattern(string key = "", string value = "")
            {
                this.Key = key;
                this.Value = value;
            }
        }
        public List<Pattern> patternList = new List<Pattern>
        {
            new Pattern("#ORGANIZATION#", ""),
            new Pattern("#AUTHORNAME#", ""),
        };
    }
}
