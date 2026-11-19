# EZEditorTools

[中文](README.md)

EZEditorTools is a Unity editor tools module that provides general-purpose editor windows, asset utilities, and editor foundations.

The code is defined in the `EZhex1991.EZEditorTools.Editor` assembly and targets the Unity Editor platform only.

Minimum supported Unity version: `2020.3.x`.

## Custom Windows and Features

All windows and features can be accessed from the `EZEditorTools` root menu in the Unity Editor. The following groups match the Sections in `EZMenuOrder`.

### Project and editor operations

| Menu name | Description |
| --- | --- |
| `Save Assets` | Forces serialized asset changes made in the Editor to be written to disk. |
| `Refresh AssetDatabase` | Refreshes the Unity AssetDatabase. |
| `Open Persistent Folder` | Opens Unity's persistent data directory. |
| `Clear Persistent Folder` | Clears Unity's persistent data directory. |
| `PlayerPrefs Editor` | Views, edits, and deletes PlayerPrefs data. Currently supported on Windows only. |

### Asset operations

| Menu name | Description |
| --- | --- |
| `Renamer` | Batch-renames assets with regular-expression matching, useful for organizing asset directories. |
| `Material Optimizer` | Views and removes material keywords and redundant properties, and can migrate legacy properties. |

### Asset relationships and build results

| Menu name | Description |
| --- | --- |
| `Asset Reference Viewer` | Views asset dependencies and scene references. |
| `Corresponding Object Viewer` | Views objects and their corresponding source objects and assets. |
| `Font Reference Viewer` | Finds UI text components that reference a specified font. |
| `Hierarchy Diff Checker` | Compares differences between two Transform hierarchies. |
| `Property Path Viewer` | Views serialized properties and property paths. |
| `Asset Bundle Manager` | Views and manages AssetBundle build results and related assets based on asset labels. |

### Debugging and utility tools

| Menu name | Description |
| --- | --- |
| `Guid Generator` | Generates system GUIDs and Unity GUIDs. |
| `Regex Tester` | Tests regular expressions and displays matches, groups, and captures. |
| `Color Blender` | Previews common color blending operations. |
| `Time Panel` | Views and parses current time information in multiple formats. |
| `Type Reflection Helper` | Uses reflection to view fields, properties, methods, and enum values, and infer information about otherwise inaccessible types from their member names. |

### Experimental features

| Menu name | Description |
| --- | --- |
| `Shader Keyword Manager` | Views and manages material and Shader keyword states. |
| `Animator Controller Combiner` | Combines Animator Controller data through an editor window. |

### Other editor features

- `Transform Context Menu`: Provides Transform Inspector context menu operations for resetting and aligning transforms.

## Custom Assets and Features

| Create menu name | Description |
| --- | --- |
| `Asset List Renamer` | Stores asset lists and renaming rules, then performs batch renaming. |
| `Script Statistics` | Stores script statistics settings and generates line-count and author-contribution reports. |
| `Image Capture` | Stores capture resolution, texture format, and output path settings for Game View or Camera captures. |
| `Bundle Builder` | Stores AssetBundle build settings and executes AssetBundle builds. |
| `Player Builder` | Stores Player build settings and executes the Player build workflow. |
| `Material Map` | Stores model material-to-asset mappings and applies them to model importers. |

## Directory Structure

The following tree lists the directories, scripts, and assembly definition in this package. Unity-generated `.meta` files are omitted for readability.

```text
EZEditorTools/                                    — EZEditorTools package root.
├── package.json                                  — Unity Package Manager metadata.
├── README.md                                     — Chinese package documentation.
├── README_EN.md                                  — English package documentation.
└── Editor/                                       — Editor scripts and assembly definition.
    ├── EZEditorToolsMenuItems.cs                 — Root menu entries for EZEditorTools windows and editor operations.
    ├── EZMenuOrder.cs                            — Display order definitions for menus and custom assets.
    ├── EZhex1991.EZEditorTools.Editor.asmdef     — Editor-only assembly definition.
    ├── Common/                                   — Shared editor utilities and supporting implementations.
    │   ├── EZButtonPropertyAttribute.cs           — Defines an Inspector button attribute for invoking object methods.
    │   ├── EZButtonPropertyDrawer.cs              — Draws Inspector buttons for `EZButtonPropertyAttribute`.
    │   ├── EZCopyList.cs                          — Defines file copy lists and their Inspector drawer.
    │   ├── EZEditorGUIUtility.cs                  — Common editor GUI drawing helpers.
    │   ├── EZEditorUtility.cs                     — Editor asset, type, and shader query helpers.
    │   ├── EZGameViewUtility.cs                   — Accesses the Unity Game View and its target Display.
    │   ├── EZShaderGUIUtility.cs                  — Shader property, keyword, and material optimization helpers.
    │   ├── MaterialExt.cs                         — Material keyword and property extension methods.
    │   └── TransformExt.cs                        — Transform extension methods, including TRS copying.
    ├── General/                                  — General-purpose editor windows and tools.
    │   ├── EZAnimatorControllerCombiner.cs        — Combines Animator Controller data.
    │   ├── EZAssetReferenceViewer.cs              — Views asset dependencies and scene references.
    │   ├── EZColorBlender.cs                      — Previews color blending results.
    │   ├── EZCorrespondingObjectViewer.cs         — Views objects and their corresponding source assets.
    │   ├── EZFontReferenceViewer.cs               — Finds UI text components that reference a font.
    │   ├── EZGuidGenerator.cs                     — Generates system and Unity GUIDs.
    │   ├── EZHierachyDiffChecker.cs               — Compares two Transform hierarchies.
    │   ├── EZMaterialOptimizer.cs                 — Batch-optimizes selected material properties.
    │   ├── EZPlayerPrefsEditor.cs                 — Views, edits, and deletes PlayerPrefs data.
    │   ├── EZPropertyPathViewer.cs                — Views serialized properties and property paths.
    │   ├── EZRegexTester.cs                       — Tests regular expressions.
    │   ├── EZShaderKeywordManager.cs              — Views and manages material and Shader keywords.
    │   ├── EZTimePanel.cs                         — Views and parses time information.
    │   ├── EZTransformContextMenu.cs              — Provides Transform Inspector context menu operations.
    │   └── EZTypeReflectionHelper.cs              — Views type reflection information.
    ├── MaterialMap/                              — Material mapping assets and inspectors.
    │   ├── EZMapAsset.cs                          — Base implementation for serializable key-value map assets.
    │   ├── EZMapAssetEditor.cs                    — Common Inspector and list drawer for map assets.
    │   ├── EZMaterialMap.cs                       — Stores model material-to-asset mappings.
    │   └── EZMaterialMapEditor.cs                 — Draws material mapping and related information.
    ├── Renamer/                                  — Asset renaming tool scripts.
    │   ├── EZAssetListRenamer.cs                  — Editor asset for asset lists and renaming rules.
    │   ├── EZAssetListRenamerEditor.cs            — Inspector for `EZAssetListRenamer`.
    │   └── EZRenamerWindow.cs                     — General-purpose renaming window.
    ├── ScriptStatistics/                         — Script statistics asset and editor scripts.
    │   ├── EZScriptStatisticResult.cs             — Stores a single script statistics result as a sub-asset.
    │   ├── EZScriptStatisticResultEditor.cs       — Inspector for `EZScriptStatisticResult`.
    │   ├── EZScriptStatistics.cs                  — Stores script statistics settings.
    │   └── EZScriptStatisticsEditor.cs            — Inspector and execution interface for script statistics.
    ├── Builder/                                  — AssetBundle and Player build tool scripts.
    │   ├── EZBuilderOptions.cs                    — Defines build-related options.
    │   ├── EZBundleBuilder.cs                     — Stores and executes AssetBundle build settings.
    │   ├── EZBundleBuilderEditor.cs               — Inspector for `EZBundleBuilder`.
    │   ├── EZBundleManager.cs                     — AssetBundle management window.
    │   ├── EZPlayerBuilder.cs                     — Stores and executes Player build settings.
    │   └── EZPlayerBuilderEditor.cs               — Inspector for `EZPlayerBuilder`.
    └── ImageCapture/                             — Image capture asset and editor scripts.
        ├── EZImageCapture.cs                     — Stores capture settings and executes captures.
        └── EZImageCaptureEditor.cs               — Inspector and operation interface for `EZImageCapture`.
```
