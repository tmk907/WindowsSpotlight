# WindowsSpotlight

Save Windows Spotlight lockscreen images, so they can be used as wallpapers.
Images are saved to folders:
- "Spotlight Wallpapers" (all images)
- "New wallpapers" (only new images)

## How to
### Download  
https://github.com/tmk907/WindowsSpotlight/releases

### Add program to startup  
- Win + R
- type: shell:startup
- copy WindowsSpotlight.exe to folder

### Set wallpaper
- Settings
- Personalization
- Background -> Slideshow
- Choose albums for your slideshow -> "New wallpapers"

## Build executable
You can also build executable from source
```
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true /p:IncludeNativeLibrariesForSelfExtract=true
```