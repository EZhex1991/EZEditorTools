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
        _____SECTION_1_____ = _____SECTION_0_____ + 100,
        SaveAssets,
        RefreshAssetDatabase,
        OpenPersistentFolder,
        ClearPersistentFolder,
        PlayerPrefsEditor,

        _____SECTION_2_____ = _____SECTION_0_____ + 200,
        Renamer,
        MaterialOptimizer,

        _____SECTION_3_____ = _____SECTION_0_____ + 300,
        AssetReferenceViewer,
        CorrespondingObjectViewer,
        FontReferenceViewer,
        HierachyDiffChecker,
        PropertyPathViewer,
        AssetBundleManager,

        _____SECTION_4_____ = _____SECTION_0_____ + 400,

        _____SECTION_5_____ = _____SECTION_0_____ + 500,

        _____SECTION_6_____ = _____SECTION_0_____ + 600,
        GuidGenerator,
        RegexTester,
        ColorBlender,
        TimePanel,
        TypeReflectionHelper,

        _____SECTION_7_____ = _____SECTION_0_____ + 700,

        _____SECTION_8_____ = _____SECTION_0_____ + 800,
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

        _____SECTION_2_____ = _____SECTION_0_____ + 200,
        ScriptStatistics,

        _____SECTION_3_____ = _____SECTION_0_____ + 300,
        ImageCapture,

        _____SECTION_4_____ = _____SECTION_0_____ + 400,
        MaterialMap,
    }
}

