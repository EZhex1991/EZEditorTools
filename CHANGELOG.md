# Changelog

## [1.3.0] - 2026-11-24

### Changed

- Bundle names now default to the selected folder name instead of the full `Assets` path.
- Split Bundle names are generated from folder names with underscores replacing path separators.

## [1.2.0] - 2026-11-24

### Added

- Added `Single`, `DirectSubfolders`, and `AllSubfolders` Bundle Builder modes.
- Added support for adding multiple Bundle Builder sources by dragging folders onto the Bundle List.

### Changed

- Bundle names now default to the selected folder path.
- Generated Bundle names replace path separators with underscores when using a split mode.
- Replaced the Bundle Builder file search option with an explicit Bundle build mode.

## [1.1.0] - 2026-11-24

### Added

- Added a current platform button to the Bundle Builder and Player Builder quick build controls.
- Added folder asset references for Bundle Builder bundle source directories.

### Changed

- Reordered Quick Build and Build controls in the Bundle Builder and Player Builder inspectors.
- Changed Bundle Builder file list output from JSON Lines to `_files.json`.
- Removed the configurable file list name and the copy to `Assets/Resources`.
- Updated the package sample to use folder references for bundle sources.

## [1.0.3] - 2026-11-20

### Added

- Added lightweight editor localization with Chinese and English language options.
- Moved localization text back into `EZLocalization.cs` for the bilingual implementation.
- Added the `EditorPrefs Editor` window for viewing, editing, and deleting EditorPrefs data on Windows.
- Added the `Scene Object Active Toggle` menu for enabling or disabling GameObject Active toggles in the Hierarchy.
- Added editor-local persistence for Image Canvas Tool preferences, Asset Reference Viewer recursion, and Material Optimizer property names.
- Grouped localization entries by editor window class names in `EZLocalization.cs`.
- Added Chinese localization support for the `Renamer` window.
- Removed the System language option; English is now the default language.
- Added the `Image Canvas Tool` editor window for checking image resolutions in a selected folder.
- Added an image Inspector button for expanding the current image canvas to a multiple of 4.
- Added three image collection scopes to the `Image Canvas Tool`.
- Added custom asset samples for Bundle Builder, Image Capture, Player Builder, and Script Statistics.
- Added sample documentation images and registered the demo in `package.json`.
- Added the Material Map sample with its model, materials, and test scene.

### Changed

- Renamed the window menu to `Image Canvas Tool` and grouped it with asset processing tools.
- Renamed the implementation scripts to match the `Image Canvas Tool` feature name.
- Improved Image Capture messages for Play Mode and Target Camera requirements.
- Clarified Script Statistics naming and result column headers.
- Added WebGL and explicit Win32/Win64 quick build buttons to Bundle Builder and Player Builder.
- Updated the README screenshots and custom asset descriptions.

## [1.0.1] - 2026-11-19

### Documentation

- Reorganized the Chinese and English README window descriptions by menu category.
- Updated window and custom asset names to use their Unity menu names.
- Improved the Chinese descriptions and synchronized the English documentation.
