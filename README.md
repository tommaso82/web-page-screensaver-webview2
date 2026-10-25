# Web Page Screensaver (WebView2)

Display web pages as a Windows screensaver, with WebGL support and separate, mirrored, or spanning multi-monitor layouts.

![Screenshot](https://user-images.githubusercontent.com/96838258/148791663-cb466735-c6c2-45cc-ab1b-b205c0fef466.png)

## Requirements

- .NET Framework 4.7.2
- Microsoft Edge WebView2 Runtime

## Install

Build in Release configuration. Keep `Web-Page-Screensaver.scr` and `Web-Page-Screensaver.scr.config` together. Right-click the `.scr` file to **Install** or **Test** it, then use Windows Screen Saver Settings to configure URLs and monitors.

## URL rotation

Use **Add** or **Edit** to set each site's URL and optional display time. The list shows both fields; double-click an entry to edit it. The screen's rotation interval applies when no custom time is set. Existing `URL;seconds` settings are still supported without changing the registry format.

## Fork changes

Uses Edge WebView2 in place of the legacy browser, a shared mouse/keyboard hook for reliable activity detection, and optional timed on-screen controls. The WebView2 user-data folder is stored under the current user's local application data.
