/* Author:          ezhex1991@outlook.com
 * CreateTime:      2020-03-31 11:05:28
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZTypeReflectionHelper : EditorWindow
    {
        private static string typeName = "UnityEditor.GameView,UnityEditor";
        private static BindingFlags bindingFlags = ~BindingFlags.Default;
        private static Type type;
        private static bool hasQueried;

        private static ReorderableList fieldList;
        private static ReorderableList propertyList;
        private static ReorderableList methodList;
        private static ReorderableList enumNameList;

        private static Vector2 scrollPosition;
        private static bool[] foldouts = new bool[8];

        private float margin = 2;

        private static Rect DrawListIndex(Rect rect, int index, ReorderableList list)
        {
            float width = EZEditorGUIUtility.GetDigitWidth(list.count - 1);
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width, EditorGUIUtility.singleLineHeight), index.ToString());
            rect.x += width;
            rect.width -= width;
            return rect;
        }

        private static string L(string key) => EZLocalization.Get<EZTypeReflectionHelper>(key);

        private void OnEnable()
        {
            EZLocalization.LanguageChanged += OnLanguageChanged;
            OnLanguageChanged();
        }

        private void OnDisable() => EZLocalization.LanguageChanged -= OnLanguageChanged;

        private void OnLanguageChanged()
        {
            titleContent = new GUIContent(L("Title"));
            Repaint();
        }

        private void DrawFieldListHeader(Rect rect, ReorderableList list)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, list);
            float width = rect.width / 5;

            rect.width = width;
            EditorGUI.LabelField(rect, "DeclaringType");
            rect.x += rect.width;
            EditorGUI.LabelField(rect, "FieldType");
            rect.x += rect.width;
            rect.width = rect.width * 2;
            EditorGUI.LabelField(rect, "FieldName");
            rect.x += rect.width;
            rect.width = rect.width;
            EditorGUI.LabelField(rect, "IsStatic");
        }
        private void DrawFieldListElement(Rect rect, int index, ReorderableList list)
        {
            var info = list.list[index] as FieldInfo;
            rect = DrawListIndex(rect, index, list);
            float width = rect.width / 5;
            rect.height = EditorGUIUtility.singleLineHeight;

            rect.width = width - margin;
            EditorGUI.TextField(rect, info.DeclaringType.Name);
            rect.x += width;
            EditorGUI.TextField(rect, info.FieldType.Name);
            rect.x += width;
            rect.width = width * 2 - margin;
            EditorGUI.TextField(rect, info.Name);
            rect.x += width * 2;
            rect.width = width - margin;
            EditorGUI.TextField(rect, info.IsStatic.ToString());
        }
        private void DrawPropertyListHeader(Rect rect, ReorderableList list)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, list);
            float width = rect.width / 5;

            rect.width = width;
            EditorGUI.LabelField(rect, "DeclaringType");
            rect.x += rect.width;
            EditorGUI.LabelField(rect, "PropertyType");
            rect.x += rect.width;
            rect.width = width * 2;
            EditorGUI.LabelField(rect, "PropertyName");
            rect.x += rect.width;
            rect.width = width - margin;
            EditorGUI.LabelField(rect, "IsStatic(Get/Set)");
        }
        private void DrawPropertyListElement(Rect rect, int index, ReorderableList list)
        {
            var info = list.list[index] as PropertyInfo;
            rect = DrawListIndex(rect, index, list);
            float width = rect.width / 5;
            rect.height = EditorGUIUtility.singleLineHeight;

            rect.width = width - margin;
            EditorGUI.TextField(rect, info.DeclaringType.Name);
            rect.x += width;
            EditorGUI.TextField(rect, info.PropertyType.Name);
            rect.x += width;
            rect.width = width * 2 - margin;
            EditorGUI.TextField(rect, info.Name);
            rect.x += width * 2;
            rect.width = width - margin;
            EditorGUI.TextField(rect, string.Format("{0}/{1}", info.GetMethod == null ? "-" : info.GetMethod.IsStatic.ToString(), info.SetMethod == null ? "-" : info.SetMethod.IsStatic.ToString()));
        }
        private void DrawMethodListHeader(Rect rect, ReorderableList list)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, list);
            float width = rect.width / 5;

            rect.width = width;
            EditorGUI.LabelField(rect, "DeclaringType");
            rect.x += rect.width;
            EditorGUI.LabelField(rect, "ReturnType");
            rect.x += rect.width;
            rect.width = width * 2;
            EditorGUI.LabelField(rect, "MethodName");
            rect.x += rect.width;
            rect.width = width - margin;
            EditorGUI.LabelField(rect, "IsStatic");
        }
        private void DrawMethodListElement(Rect rect, int index, ReorderableList list)
        {
            var info = list.list[index] as MethodInfo;
            rect = DrawListIndex(rect, index, list);
            float width = rect.width / 5;
            rect.height = EditorGUIUtility.singleLineHeight;

            rect.width = width - margin;
            EditorGUI.TextField(rect, info.DeclaringType.Name);
            rect.x += width;
            EditorGUI.TextField(rect, info.ReturnType.Name);
            rect.x += width;
            rect.width = width * 2 - margin;
            EditorGUI.TextField(rect, info.Name);
            rect.x += width * 2;
            rect.width = width - margin;
            EditorGUI.TextField(rect, info.IsStatic.ToString());
        }
        private void DrawEnumNameListHeader(Rect rect, ReorderableList list)
        {
            rect = EZEditorGUIUtility.CalcReorderableListHeaderRect(rect, list);
            EditorGUI.LabelField(rect, "EnumNames");
        }
        private void DrawEnumNameListElement(Rect rect, int index, ReorderableList list)
        {
            var info = enumNameList.list[index] as string;
            rect = DrawListIndex(rect, index, list);
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.TextField(rect, info);
        }

        private void GetTypeInfo()
        {
            hasQueried = true;
            type = Type.GetType(typeName);
            for (int i = 0; i < foldouts.Length; i++)
            {
                foldouts[i] = true;
            }
            if (type != null)
            {
                if (type.IsClass)
                {
                    fieldList = new ReorderableList(type.GetFields(bindingFlags), typeof(FieldInfo), false, true, false, false)
                    {
                        drawHeaderCallback = (rect) => DrawFieldListHeader(rect, fieldList),
                        drawElementCallback = (rect, index, isActive, isFocused) => DrawFieldListElement(rect, index, fieldList),
                    };
                    propertyList = new ReorderableList(type.GetProperties(bindingFlags), typeof(PropertyInfo), false, true, false, false)
                    {
                        drawHeaderCallback = (rect) => DrawPropertyListHeader(rect, propertyList),
                        drawElementCallback = (rect, index, isActive, isFocused) => DrawPropertyListElement(rect, index, propertyList),
                    };
                    methodList = new ReorderableList(
                        (from methodInfo in type.GetMethods(bindingFlags)
                         where !(methodInfo.IsSpecialName && (methodInfo.Name.StartsWith("set_") || methodInfo.Name.StartsWith("get_")))
                         select methodInfo).ToList()
                        , typeof(MethodInfo), false, true, false, false)
                    {
                        drawHeaderCallback = (rect) => DrawMethodListHeader(rect, methodList),
                        drawElementCallback = (rect, index, isActive, isFocused) => DrawMethodListElement(rect, index, methodList),
                    };
                    enumNameList = null;
                }
                else if (type.IsEnum)
                {
                    fieldList = null;
                    propertyList = null;
                    methodList = null;
                    enumNameList = new ReorderableList(type.GetEnumNames(), typeof(string), false, true, false, false)
                    {
                        drawHeaderCallback = (rect) => DrawEnumNameListHeader(rect, enumNameList),
                        drawElementCallback = (rect, index, isActive, isFocused) => DrawEnumNameListElement(rect, index, enumNameList),
                    };
                }
            }
        }

        private void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            typeName = EditorGUILayout.TextField(L("TypeName"), typeName);
            bindingFlags = (BindingFlags)EditorGUILayout.EnumFlagsField(bindingFlags);
            if (GUILayout.Button(L("GetTypeInfo")))
            {
                GetTypeInfo();
            }

            if (!hasQueried)
            {
                EditorGUILayout.HelpBox(L("NotQueried"), MessageType.Info);
            }
            else if (type == null)
            {
                EditorGUILayout.HelpBox(L("NotFound"), MessageType.Info);
            }
            else if (!type.IsClass && !type.IsEnum)
            {
                EditorGUILayout.HelpBox(L("Unsupported"), MessageType.Info);
            }
            else
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                DrawInfo(ref foldouts[0], "Fields", fieldList);
                DrawInfo(ref foldouts[2], "Properties", propertyList);
                DrawInfo(ref foldouts[1], "Methods", methodList);
                DrawInfo(ref foldouts[3], "Enum Names", enumNameList);
                EditorGUILayout.EndScrollView();
            }
        }
        private void DrawInfo(ref bool foldout, string label, ReorderableList list)
        {
            if (list == null) return;
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            foldout = EditorGUILayout.Foldout(foldout, string.Format("{0} ({1})", label, list.count), true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;
            if (foldout)
            {
                list.DoLayoutList();
            }
        }
    }
}

