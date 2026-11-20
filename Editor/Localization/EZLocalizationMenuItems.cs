/* Author:          ezhex1991@outlook.com
 * CreateTime:      2026-11-19
 * Organization:    #ORGANIZATION#
 * Description:     Provides language selection menu items for EZEditorTools.
 */
using UnityEditor;

namespace EZhex1991.EZEditorTools
{
    internal static class EZLocalizationMenuItems
    {
        private const string Root = "EZEditorTools/Language/";
        private const string SystemMenuName = "System";
        private const string ChineseMenuName = "简体中文（仅对窗口生效）";
        private const string EnglishMenuName = "English (Window UI Only)";

        [MenuItem(Root + SystemMenuName, false, (int)EZMenuOrder.LanguageSystem)]
        private static void UseSystemLanguage() => EZLocalization.SetLanguage(EZLanguage.System);

        [MenuItem(Root + SystemMenuName, true)]
        private static bool ValidateSystemLanguage() => SetChecked(EZLanguage.System);

        [MenuItem(Root + ChineseMenuName, false, (int)EZMenuOrder.LanguageChinese)]
        private static void UseChinese() => EZLocalization.SetLanguage(EZLanguage.Chinese);

        [MenuItem(Root + ChineseMenuName, true)]
        private static bool ValidateChinese() => SetChecked(EZLanguage.Chinese);

        [MenuItem(Root + EnglishMenuName, false, (int)EZMenuOrder.LanguageEnglish)]
        private static void UseEnglish() => EZLocalization.SetLanguage(EZLanguage.English);

        [MenuItem(Root + EnglishMenuName, true)]
        private static bool ValidateEnglish() => SetChecked(EZLanguage.English);

        private static bool SetChecked(EZLanguage language)
        {
            string menuName = language == EZLanguage.System
                ? SystemMenuName
                : language == EZLanguage.Chinese ? ChineseMenuName : EnglishMenuName;
            Menu.SetChecked(Root + menuName, EZLocalization.Language == language);
            return true;
        }
    }
}
