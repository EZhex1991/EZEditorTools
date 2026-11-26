/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-11 19:17:13
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using System;
using UnityEditor;
using UnityEngine;

namespace EZhex1991.EZEditorTools
{
    public class EZTimePanel : EditorWindow
    {
        private string ticksText;
        private string ticksSecondsText;
        private string ticksMillisecondsText;
        private string unixSecondsText;
        private string unixMillisecondsText;
        public string timeText;

        public DateTime time;
        public string format = "yyyy-MM-dd HH:mm:ss";

        public GUIStyle style;
        private string parseError;

        private static string L(string key) => EZLocalization.Get<EZTimePanel>(key);

        private void OnLanguageChanged()
        {
            titleContent = new GUIContent(L("Title"));
            Repaint();
        }

        public void GetCurrentTime()
        {
            time = DateTime.Now;
            timeText = time.ToString(format);
            UpdateNumericTexts();
        }

        private void UpdateNumericTexts()
        {
            ticksText = time.Ticks.ToString();
            ticksSecondsText = (time.Ticks / TimeSpan.TicksPerSecond).ToString();
            ticksMillisecondsText = (time.Ticks / TimeSpan.TicksPerMillisecond).ToString();
            DateTimeOffset offset = new DateTimeOffset(time);
            unixSecondsText = offset.ToUnixTimeSeconds().ToString();
            unixMillisecondsText = offset.ToUnixTimeMilliseconds().ToString();
        }

        private void ParseValue(Func<DateTime> parse)
        {
            try
            {
                time = parse();
                timeText = time.ToString(format);
                UpdateNumericTexts();
                parseError = null;
            }
            catch (Exception e)
            {
                parseError = e.Message;
                Debug.LogException(e);
            }
            Repaint();
            GUIUtility.ExitGUI();
        }

        private void DrawNumericInput(string label, ref string text, Func<string, DateTime> parse)
        {
            EditorGUILayout.BeginHorizontal();
            text = EditorGUILayout.TextField(label, text);
            bool clicked = GUILayout.Button(L("Parse"), GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();
            if (clicked)
            {
                string value = text;
                ParseValue(() => parse(value));
            }
        }
        public void DrawLabel(string label1, string text)
        {
            EditorGUILayout.TextField(label1, text, EditorStyles.label);
        }

        private void OnEnable()
        {
            EZLocalization.LanguageChanged += OnLanguageChanged;
            OnLanguageChanged();
            GetCurrentTime();
        }

        private void OnDisable() => EZLocalization.LanguageChanged -= OnLanguageChanged;

        private void OnGUI()
        {
            EZEditorGUIUtility.WindowTitle(this);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(L("Format"), format);

            EditorGUILayout.BeginHorizontal();
            timeText = EditorGUILayout.TextField(L("DateTime"), timeText);
            bool parseClicked = GUILayout.Button(L("Parse"), GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();
            if (parseClicked) ParseValue(() => DateTime.Parse(timeText));
            if (!string.IsNullOrEmpty(parseError))
                EditorGUILayout.HelpBox(string.Format(L("ParseError"), parseError), MessageType.Error);

            if (GUILayout.Button(L("Now")))
            {
                GetCurrentTime();
                parseError = null;
                GUI.FocusControl(null);
                Repaint();
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.Space();
            DrawLabel("ToLongDateString", time.ToLongDateString());
            DrawLabel("ToShortDateString", time.ToShortDateString());
            DrawLabel("ToLongTimeString", time.ToLongTimeString());
            DrawLabel("ToShortTimeString", time.ToShortTimeString());

            EditorGUILayout.Space();
            DrawLabel("Kind", time.Kind.ToString());
            DrawLabel("Year", time.Year.ToString());
            DrawLabel("Month", time.Month.ToString());
            DrawLabel("Day", time.Day.ToString());
            DrawLabel("Hour", time.Hour.ToString());
            DrawLabel("Minute", time.Minute.ToString());
            DrawLabel("Second", time.Second.ToString());
            DrawLabel("Millisecond", time.Millisecond.ToString());

            EditorGUILayout.Space();
            DrawLabel("DayOfWeek", time.DayOfWeek.ToString());
            DrawLabel("DayOfYear", time.DayOfYear.ToString());
            DrawLabel("TimeOfDay", time.TimeOfDay.ToString());
            EditorGUILayout.Space();
            DrawNumericInput("Ticks", ref ticksText, value => new DateTime(long.Parse(value), time.Kind));
            DrawNumericInput(L("TicksSeconds"), ref ticksSecondsText, value => new DateTime(checked(long.Parse(value) * TimeSpan.TicksPerSecond), time.Kind));
            DrawNumericInput(L("TicksMilliseconds"), ref ticksMillisecondsText, value => new DateTime(checked(long.Parse(value) * TimeSpan.TicksPerMillisecond), time.Kind));
            DrawNumericInput(L("UnixSeconds"), ref unixSecondsText, value => DateTimeOffset.FromUnixTimeSeconds(long.Parse(value)).LocalDateTime);
            DrawNumericInput(L("UnixMilliseconds"), ref unixMillisecondsText, value => DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(value)).LocalDateTime);

            EditorGUILayout.Space();
            DrawLabel("ToBinary", time.ToBinary().ToString());
            DrawLabel("ToFileTime", time.ToFileTime().ToString());
            DrawLabel("ToFileTimeUtc", time.ToFileTimeUtc().ToString());
            DrawLabel("ToLocalTime", time.ToLocalTime().ToString());
            DrawLabel("ToOADate", time.ToOADate().ToString());
            DrawLabel("ToUniversalTime", time.ToUniversalTime().ToString());
        }
    }
}

