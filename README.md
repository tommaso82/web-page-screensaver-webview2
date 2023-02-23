# web-page-screensaver-webview2

Display a web page as your screensaver.  
That fork support multiple display and WebGL (Edge). 

![image](https://user-images.githubusercontent.com/96838258/148791663-cb466735-c6c2-45cc-ab1b-b205c0fef466.png)

## Dependencies

.NET Framework 4.7

## Usage

1. Find Web-Page-Screensaver.scr on your computer, right click it
2. Select "Install" to install, or "Test" to test it out without installing
3. If installing, the Screen Saver Settings dialog will pop up with the correct screen saver selected
4. Use the `Settings...` button in the same dialog to change the web page(s) displayed by the screen saver

## Notes on this Fork

In my fork i modified the event handler, moving from the IMessageFilter interface that was causing some stability issues to a more reliable low-level hooking library (MouseKeyHook). I also added a timer to handle the disappearance of the mouse and close button. I know that my code still have room for improvement, and I am open to any constructive feedback or suggestions you may have to help me continue to grow and develop as a programmer.

