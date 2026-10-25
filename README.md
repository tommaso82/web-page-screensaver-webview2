# Web Page Screensaver

A Windows screensaver powered by Microsoft Edge WebView2. Displays websites with WebGL support, URL rotation, and separate, mirrored, or spanning multi-monitor layouts. Based on [cwc/web-page-screensaver](https://github.com/cwc/web-page-screensaver).

## Requirements

Windows, .NET Framework 4.7.2, and the [Microsoft Edge WebView2 Runtime](https://developer.microsoft.com/microsoft-edge/webview2/).

## Installation

Build `Web-Page-Screensaver.sln` in **Release**, or download a build from [GitHub Actions](https://github.com/tommaso82/web-page-screensaver-webview2/actions). Keep the `.scr`, `.scr.config`, and accompanying DLLs together. Right-click `Web-Page-Screensaver.scr` and select **Install**, then open Windows Screen Saver Settings to configure it.

## Configuration

Add and edit websites using the built-in URL editor. Set a display time per site or use the screen default; reorder or shuffle URLs as needed. Activity can close the screensaver or reveal temporary controls.

Existing `URL;seconds` settings remain compatible.
