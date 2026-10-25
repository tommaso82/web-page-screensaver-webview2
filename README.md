# Web Page Screensaver (WebView2)

Display web pages as a Windows screensaver, with WebGL support and separate, mirrored, or spanning multi-monitor layouts.

![Screenshot](https://user-images.githubusercontent.com/96838258/148791663-cb466735-c6c2-45cc-ab1b-b205c0fef466.png)

## Requirements

- .NET Framework 4.7.2
- Microsoft Edge WebView2 Runtime

## Install

Build in Release configuration. Keep `Web-Page-Screensaver.scr` and `Web-Page-Screensaver.scr.config` together: the latter enables per-monitor DPI support. Right-click the `.scr` file to **Install** or **Test** it, then use Windows Screen Saver Settings to configure URLs and monitors.

## URL rotation

Set a default rotation interval for each screen, or append `;seconds` to an individual URL to override it, for example `https://example.com/;45`. Use positive whole seconds. URLs containing a literal semicolon followed by digits are ambiguous with this legacy syntax.

## Fork changes

Uses Edge WebView2 in place of the legacy browser, a shared mouse/keyboard hook for reliable activity detection, and optional timed on-screen controls. The WebView2 user-data folder is stored under the current user's local application data.
