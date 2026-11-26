/* Author:          ezhex1991@outlook.com
 * CreateTime:      2019-07-11 19:36:24
 * Organization:    #ORGANIZATION#
 * Description:     
 */
namespace EZhex1991.EZEditorTools
{
    public enum EZMenuOrder
    {
        _____SECTION_0_____ = 10000,
        Language,
        LanguageChinese,
        LanguageEnglish,

        _____SECTION_1_____ = _____SECTION_0_____ + 100,
        HierarchyActiveToggle,
        ScriptTemplateManager,

        _____SECTION_2_____ = _____SECTION_0_____ + 200,
        SaveAssets,
        RefreshAssetDatabase,
        OpenPersistentFolder,
        ClearPersistentFolder,
        PlayerPrefsEditor,
        EditorPrefsEditor,

        _____SECTION_3_____ = _____SECTION_0_____ + 300,
        Renamer,
        MaterialOptimizer,
        ImageCanvasTool,

        _____SECTION_4_____ = _____SECTION_0_____ + 400,
        AssetReferenceViewer,
        CorrespondingObjectViewer,
        FontReferenceViewer,
        HierachyDiffChecker,
        PropertyPathViewer,
        AssetBundleManager,

        _____SECTION_5_____ = _____SECTION_0_____ + 500,

        _____SECTION_6_____ = _____SECTION_0_____ + 600,

        _____SECTION_7_____ = _____SECTION_0_____ + 700,
        GuidGenerator,
        ColorBlender,
        TimePanel,
        TypeReflectionHelper,
        RegexTester,

        _____SECTION_8_____ = _____SECTION_0_____ + 800,

        _____SECTION_9_____ = _____SECTION_0_____ + 900,
        ShaderKeywordManager,
        AnimatorControllerCombiner,
    }

    public enum EZAssetMenuOrder
    {
        _____SECTION_0_____ = 6990,
        AssetListRenamer,

        _____SECTION_1_____ = _____SECTION_0_____ + 100,
        BundleBuilder,
        PlayerBuilder,
        CopyList,

        _____SECTION_2_____ = _____SECTION_0_____ + 200,
        ScriptStatistics,

        _____SECTION_3_____ = _____SECTION_0_____ + 300,
        ImageCapture,

        _____SECTION_4_____ = _____SECTION_0_____ + 400,
        MaterialMap,
    }
}

